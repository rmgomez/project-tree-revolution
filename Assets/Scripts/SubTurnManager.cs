using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubTurnManager : Singleton<SubTurnManager>
{
	[HideInInspector]
	public bool isUpdating = false;
	public AudioClip damage_sound;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartSubTurn();
		}
	}

	public void StartSubTurn()
	{
		if (!isUpdating)
		{
			isUpdating = true;
			Debug.Log("Start Sub Turn");
			StartCoroutine(SubLoop());
		}
	}

	public IEnumerator SubLoop()
	{
		GridManager gridManager = GridManager.Instance;

		//Reset action point for this turn
		for (int x = 0; x < gridManager.gridSize.x; x++)
		{
			for (int y = 0; y < gridManager.gridSize.y; y++)
			{
				Tile tile = gridManager.tileLines[x].tiles[y];

				if (tile.piece != null)
				{
					Piece actualPiece = tile.piece;

					var actionPointComponent = actualPiece.GetComponent<ActionPointComponent>();

					if (actionPointComponent != null)
					{
						actionPointComponent.ResetAction();
					}
				}
			}
		}

		int maxPriorityFind = 0;
		int actualPriority = 0;
		bool needRepass;

		while (actualPriority <= maxPriorityFind)
		{
			needRepass = false;

			for (int x = 0; x < gridManager.gridSize.x; x++)
			{
				for (int y = 0; y < gridManager.gridSize.y; y++)
				{
					Tile tile = gridManager.tileLines[x].tiles[y];

					if (tile.piece != null)
					{
						Piece actualPiece = tile.piece;

						var priorityComponent = actualPiece.GetComponent<PriorityComponent>();

						if (priorityComponent != null)
						{
							if (priorityComponent.priority == actualPriority)
							{
								var actionPoint = actualPiece.GetComponent<ActionPointComponent>();

								//Check if can do action
								if (actionPoint && actionPoint.actualActionPoints > 0)
								{
									//Check what is in front
									if (x + 1 < gridManager.gridSize.x)
									{
										var frontTile = gridManager.tileLines[x + 1].tiles[y];
										var frontPiece = frontTile.piece;

										CanWalkOnIt canWalkOnIt = null;

										if (frontPiece != null)
										{
											canWalkOnIt = frontPiece.GetComponent<CanWalkOnIt>();
										}

										if (frontPiece == null || canWalkOnIt != null)
										{
											var move = actualPiece.GetComponent<MovementComponent>();

											if (move != null)
											{
												//move
												yield return move.Action();

												frontTile.ChangePiece(tile.piece, tile.pieceType);
												tile.RemovePiece();

												//Check the side

												if (y > 0)
												{
													Tile leftTile = gridManager.tileLines[x + 1].tiles[y - 1];

													if (leftTile.piece && leftTile.piece.GetComponent<CanAttackAround>() != null)
													{
														// Sound effect to make damage
														if (GameObject.Find("LevelManager") != null && 
														GameObject.Find("LevelManager").GetComponent<AudioSource>() != null && 
														damage_sound != null)
														{
															GameObject.Find("LevelManager").GetComponent<AudioSource>().PlayOneShot(damage_sound);
														}


														yield return frontTile.piece.GetComponent<LifeComponent>()?.GetDamage(
															leftTile.piece.GetComponent<AttackReactionComponent>().damages
														);

														var life = frontTile.piece.GetComponent<LifeComponent>();

														if (!life.isAlive)
														{
															frontTile.DestroyPiece();
															continue;
														}
													}
												}

												if (y < gridManager.gridSize.y)
												{
													Tile rightTile = gridManager.tileLines[x + 1].tiles[y + 1];

													if (rightTile.piece && rightTile.piece.GetComponent<CanAttackAround>() != null)
													{
														// Sound effect to make damage
														if (GameObject.Find("LevelManager") != null && 
														GameObject.Find("LevelManager").GetComponent<AudioSource>() != null && 
														damage_sound != null)
														{
															GameObject.Find("LevelManager").GetComponent<AudioSource>().PlayOneShot(damage_sound);
														}

														yield return frontTile.piece.GetComponent<LifeComponent>()?.GetDamage(
															rightTile.piece.GetComponent<AttackReactionComponent>().damages
														);

														var life = frontTile.piece.GetComponent<LifeComponent>();

														if (!life.isAlive)
														{
															frontTile.DestroyPiece();
															continue;
														}
													}
												}


												//move on something
												if (canWalkOnIt)
												{
													yield return canWalkOnIt.OnWalkOnIt();

													switch (canWalkOnIt.walkReaction)
													{
														case WalkReaction.Explosion:

															var frontAttack = frontPiece?.GetComponent<AttackReactionComponent>();

															if (frontAttack)
															{
																// Sound effect to make damage
																if (GameObject.Find("LevelManager") != null && 
																GameObject.Find("LevelManager").GetComponent<AudioSource>() != null && 
																damage_sound != null)
																{
																	GameObject.Find("LevelManager").GetComponent<AudioSource>().PlayOneShot(damage_sound);
																}

																var life = frontTile.piece.GetComponent<LifeComponent>();
																if (life)
																{
																	yield return life.GetDamage(frontAttack.damages);

																	if (!life.isAlive)
																	{
																		frontTile.DestroyPiece();
																	}
																}
															}

															var frontLife = frontPiece?.GetComponent<LifeComponent>();

															yield return frontLife?.Death();

															if (!frontLife.isAlive)
															{
																Destroy(frontPiece.gameObject);
															}

															frontTile.CreateGround(GroundTypes.Explosed);
															break;

														case WalkReaction.DeleteAndReplace:

															frontPiece.GetComponent<SoundComponent>()?.PlayCuandoSeBorra();

															if (canWalkOnIt && canWalkOnIt.object_to_replace != null)
															{
																Instantiate(canWalkOnIt.object_to_replace, frontPiece.transform.position, Quaternion.identity);
																Destroy(frontPiece.gameObject);
															}

															break;
													}
												}
											}

											actionPoint.actualActionPoints--;
										}
										else
										{
											const string TagEnemy = "Enemy";
											if (frontPiece.CompareTag(TagEnemy))
											{
												//wait
												needRepass = true;
												continue;
											}
											else
											{
												const string TagPlant = "Plant";
												if (frontPiece.CompareTag(TagPlant))
												{
													//attack
													var attack = actualPiece.GetComponent<AttackComponent>();

													if (attack != null)
													{
														// Sound effect to make damage
														if (GameObject.Find("LevelManager") != null && 
														GameObject.Find("LevelManager").GetComponent<AudioSource>() != null && 
														damage_sound != null)
														{
															GameObject.Find("LevelManager").GetComponent<AudioSource>().PlayOneShot(damage_sound);
														}
														
														yield return attack.Action();

														var frontLife = frontPiece.GetComponent<LifeComponent>();

														if (frontLife != null)
														{
															yield return frontLife.GetDamage(attack.damages);

															if (!frontLife.isAlive)
															{
																if (!frontLife.KeepLastVisual)
																{
																	frontTile.DestroyPiece();
																}
																else
																{
																	frontTile.RemovePiece();
																	frontTile.canPlantOnIt = false;
																}
															}
															else
															{
																var attackReaction = frontPiece.GetComponent<AttackReactionComponent>();

																if (attackReaction != null)
																{
																	yield return attackReaction.Action();

																	var life = actualPiece.GetComponent<LifeComponent>();

																	if (life != null)
																	{
																		yield return life.GetDamage(attackReaction.damages);

																		if (!life.isAlive)
																		{
																			tile.DestroyPiece();
																		}
																	}
																}
															}
														}
													}

													actionPoint.actualActionPoints--;
												}
											}
										}

										if (actionPoint.actualActionPoints > 0)
										{
											needRepass = true;
										}

									}
								}
							}
							else if (priorityComponent.priority > maxPriorityFind)
							{
								maxPriorityFind = priorityComponent.priority;
							}
						}
						else
						{
							Debug.LogError("Need to add a ActionPointComponent", actualPiece);
						}
					}
				}
			}

			if (!needRepass)
			{
				actualPriority++;
			}
		}

		isUpdating = false;
		Debug.Log("End Sub Turn");
	}
}

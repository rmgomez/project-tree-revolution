using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubTurnManager : Singleton<SubTurnManager>
{
	[HideInInspector]
	public bool isUpdating = false;

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

								//var attack = go.GetComponent<AttackComponent>();
								//var life = go.GetComponent<LifeComponent>();

								//Check if can do action
								if (actionPoint && actionPoint.actualActionPoints > 0)
								{
									//Check what is in front

									if (x + 1 < gridManager.gridSize.x)
									{
										var frontTile = gridManager.tileLines[x + 1].tiles[y];
										var frontPiece = frontTile.piece;

										//Debug.Log("frontPiece [" + (x + 1) + "][" + y + "]" + frontPiece);

										// CanWalkOnIt canWalkOnIt = frontPiece?.GetComponent<CanWalkOnIt>();

										CanWalkOnIt canWalkOnIt = null;

										if (frontPiece != null)
										{
											canWalkOnIt = frontPiece.GetComponent<CanWalkOnIt>();
										}

										if (frontPiece == null || canWalkOnIt != null)
										{
											//move
											var move = actualPiece.GetComponent<MovementComponent>();

											if (move != null)
											{
												yield return move.Action();

												frontTile.ChangePiece(tile.piece, tile.pieceType);
												tile.RemovePiece();

												if (canWalkOnIt)
												{
													canWalkOnIt.OnWalkOnIt();

													switch (canWalkOnIt.walkReaction)
													{
														case WalkReaction.Explosion:

															var frontAttack = frontPiece?.GetComponent<AttackComponent>();

															if (frontAttack)
															{
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
														yield return attack.Action();

														var frontLife = frontPiece.GetComponent<LifeComponent>();

														if (frontLife != null)
														{
															yield return frontLife.GetDamage(attack.damages);

															if (!frontLife.isAlive)
															{
																frontTile.DestroyPiece();
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
																		yield return life.GetDamage(attackReaction.attackForce);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubTurnManager : Singleton<SubTurnManager>
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("Start Sub Turn");
			StartSubTurn();
		}
	}

	public void StartSubTurn()
	{
		StartCoroutine(SubLoop());
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
					GameObject actualGO = tile.piece;

					var actionPointComponent = actualGO.GetComponent<ActionPointComponent>();

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
						GameObject actualGO = tile.piece;

						var priorityComponent = actualGO.GetComponent<PriorityComponent>();

						if (priorityComponent != null)
						{
							if (priorityComponent.priority == actualPriority)
							{

								var actionPoint = actualGO.GetComponent<ActionPointComponent>();

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

										Debug.Log("frontPiece [" + (x + 1) + "][" + y + "]" + frontPiece);

										if (frontPiece == null)
										{
											//move

											var move = actualGO.GetComponent<MovementComponent>();

											if (move != null)
											{
												yield return move.Action();

												frontTile.ChangePiece(tile.piece, tile.pieceType);
												tile.RemovePiece();
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
													var attack = actualGO.GetComponent<AttackComponent>();

													if (attack != null)
													{
														yield return attack.Action();

														var frontLife = frontPiece.GetComponent<LifeComponent>();

														if (frontLife != null)
														{
															frontLife.GetDamage(attack.attackForce);

															if (!frontLife.isAlive)
															{
																var frontDeathReaction = frontPiece.GetComponent<DeathReaction>();

																if (frontDeathReaction != null)
																{
																	yield return frontDeathReaction.Action();
																}

																frontTile.DestroyPiece();
															}
															else
															{
																var attackReaction = frontPiece.GetComponent<AttackReactionComponent>();

																if (attackReaction != null)
																{
																	yield return attackReaction.Action();

																	var life = actualGO.GetComponent<LifeComponent>();

																	if (life != null)
																	{
																		life.GetDamage(attackReaction.attackForce);

																		if (!life.isAlive)
																		{
																			var deathReaction = actualGO.GetComponent<DeathReaction>();

																			if (deathReaction != null)
																			{
																				yield return deathReaction.Action();
																			}

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
							Debug.LogError("Need to add a ActionPointComponent", actualGO);
						}
					}
				}
			}

			if (!needRepass)
			{
				actualPriority++;
			}

			yield return new WaitForSeconds(0.2f);
		}

		Debug.Log("End Sub Turn");
	}
}

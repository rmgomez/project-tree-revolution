using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileLine
{
	public TileLine(int size)
	{
		tiles = new Tile[size];
	}

	public Tile[] tiles;
}

public class GridManager : Singleton<GridManager>
{
	private const string TagPlant = "Plant";
	private const string TagEnemy = "Enemy";

	public Vector2Int gridSize = new Vector2Int(5, 5);
	public int defaultY = 0;

	public Tile tilePrefab;

	public Ground[] groundPrefabs;
	public Piece[] piecePrefabs;

	public TileLine[] tileLines;

	public Tile GetTileByPos(int X, int Y)
	{
		if (X >= gridSize.x || Y >= gridSize.y)
		{
			Debug.LogWarning("You try to find a tile outside of the range [" + X + "][" + Y + "]");
			return null;
		}

		return tileLines[X].tiles[Y];
	}


	public GameObject GetGroundPrefab(GroundTypes groundType)
	{
		foreach (var item in groundPrefabs)
		{
			if (item.groundType == groundType)
			{
				return item.gameObject;
			}
		}

		Debug.LogWarning("No Ground Prefab find for " + groundType);

		return null;
	}

	public GameObject GetPiecePrefab(PieceTypes pieceType)
	{
		foreach (var item in piecePrefabs)
		{
			//Debug.Log(item.pieceType + " | " + pieceType);
			if (item.pieceType == pieceType)
			{
				return item.gameObject;
			}
		}

		Debug.LogWarning("No Piece Prefab find for " + pieceType);

		return null;
	}

	/*private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Keypad0))
		{
			Debug.Log("ShowInteratable(PlayerActions.Attack)");
			ShowInteratable(PlayerActions.Attack);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad1))
		{
			Debug.Log("ShowInteratable(PlayerActions.Heal)");
			ShowInteratable(PlayerActions.Heal);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad2))
		{
			Debug.Log("ShowInteratable(PlayerActions.Place)");
			ShowInteratable(PlayerActions.Place);
		}
	}*/


	public void ShowInteratable(PlayerActions playerAction)
	{
		switch (playerAction)
		{
			case PlayerActions.Heal:

				for (int x = 0; x < gridSize.x; x++)
				{
					for (int z = 0; z < gridSize.y; z++)
					{
						if (tileLines[x].tiles[z].piece && tileLines[x].tiles[z].piece.CompareTag(TagPlant))
						{
							var life = tileLines[x].tiles[z].piece.GetComponent<LifeComponent>();

							if (life != null && life.actualQuantity < life.maxQuantity)
							{
								tileLines[x].tiles[z].visualTileInfo.ChangeColor(VisualTileInfos.Valid);
								continue;
							}
						}

						tileLines[x].tiles[z].visualTileInfo.ChangeColor(VisualTileInfos.Hide);
					}
				}

				break;

			case PlayerActions.Place:
				for (int x = 0; x < gridSize.x; x++)
				{
					for (int z = 0; z < gridSize.y; z++)
					{
						if (tileLines[x].tiles[z].piece == null)
						{
							tileLines[x].tiles[z].visualTileInfo.ChangeColor(VisualTileInfos.Valid);
						}
						else
						{
							tileLines[x].tiles[z].visualTileInfo.ChangeColor(VisualTileInfos.Hide);
						}
					}
				}
				break;

			case PlayerActions.Attack:
				for (int x = 0; x < gridSize.x; x++)
				{
					for (int z = 0; z < gridSize.y; z++)
					{
						if (tileLines[x].tiles[z].piece && tileLines[x].tiles[z].piece.CompareTag(TagEnemy))
						{
							var life = tileLines[x].tiles[z].piece.GetComponent<LifeComponent>();

							if (life != null)
							{
								tileLines[x].tiles[z].visualTileInfo.ChangeColor(VisualTileInfos.Valid);
								continue;
							}
						}

						tileLines[x].tiles[z].visualTileInfo.ChangeColor(VisualTileInfos.Hide);
					}
				}
				break;
		}
	}



#if UNITY_EDITOR

	private void SpawnGrid()
	{
		DestroyGrid();

		tileLines = new TileLine[gridSize.x];

		for (int x = 0; x < gridSize.x; x++)
		{
			tileLines[x] = new TileLine(gridSize.y);

			for (int z = 0; z < gridSize.y; z++)
			{
				tileLines[x].tiles[z] = Instantiate(tilePrefab, new Vector3Int(x, defaultY, z), Quaternion.identity, transform);
				tileLines[x].tiles[z].name = $"Tile[{x}][{z}]";
			}
		}

		if (Camera.main)
		{
			Camera.main.transform.position = new Vector3(gridSize.x/2, 10, gridSize.y/2);
			Camera.main.transform.eulerAngles = new Vector3(90, 0, 0);
		}
	}

	private void DestroyGrid()
	{

		while (transform.childCount > 0)
		{
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}

	public bool debugCreateTile = false;

	private void OnDrawGizmosSelected()
	{
		if (debugCreateTile)
		{
			debugCreateTile = false;

			SpawnGrid();
		}
	}

#endif
}

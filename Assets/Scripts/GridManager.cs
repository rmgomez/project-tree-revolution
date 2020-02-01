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


#if UNITY_EDITOR

	private void SpawnGrid()
	{
		DestroyGrid();

		tileLines = new TileLine[(int)gridSize.x];

		for (int x = 0; x < gridSize.x; x++)
		{
			tileLines[x] = new TileLine((int)gridSize.y);

			for (int z = 0; z < gridSize.y; z++)
			{
				tileLines[x].tiles[z] = Instantiate(tilePrefab, new Vector3Int(x, defaultY, z), tilePrefab.transform.rotation, transform);
				tileLines[x].tiles[z].name = $"Tile[{x}][{z}]";
			}
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

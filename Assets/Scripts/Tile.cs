using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public GroundTypes groundTypes = GroundTypes.Void;
	[HideInInspector]
	public GameObject ground;

	public PieceTypes pieceTypes = PieceTypes.Nothing;
	[HideInInspector]
	public GameObject piece;


	private GroundTypes _groundTypes = GroundTypes.Void;
	private PieceTypes _pieceTypes = PieceTypes.Nothing;

	private void Awake()
	{
		//AutoCreateTileContent();
	}


	private void OnDrawGizmos()
	{
		AutoCreateTileContent();
	}

	private void AutoCreateTileContent()
	{
		if (groundTypes != _groundTypes)
		{
			_groundTypes = groundTypes;

			if (ground != null)
			{
				DestroyImmediate(ground);
			}

			if (_groundTypes != GroundTypes.Void)
			{
				GameObject prefab = GridManager.Instance.GetGroundPrefab(_groundTypes);

				if (prefab != null)
				{
					ground = Instantiate(prefab, transform.position, transform.rotation, transform);
				}
			}
		}

		if (pieceTypes != _pieceTypes)
		{
			_pieceTypes = pieceTypes;

			if (piece != null)
			{
				DestroyImmediate(piece);
			}

			if (_pieceTypes != PieceTypes.Nothing)
			{
				GameObject prefab = GridManager.Instance.GetPiecePrefab(_pieceTypes);

				if (prefab != null)
				{
					piece = Instantiate(prefab, transform.position, transform.rotation, transform);
				}
			}
		}
	}
}

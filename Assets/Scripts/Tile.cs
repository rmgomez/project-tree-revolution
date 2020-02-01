using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public GroundTypes groundType = GroundTypes.Void;
	[HideInInspector]
	public GameObject ground;

	public PieceTypes pieceType = PieceTypes.Nothing;
	[HideInInspector]
	public GameObject piece;

	private void CreateGround(GroundTypes newGroundType)
	{
		groundType = newGroundType;

		if (ground != null)
		{
			DestroyImmediate(ground);
		}

		if (groundType != GroundTypes.Void)
		{
			GameObject prefab = GridManager.Instance.GetGroundPrefab(groundType);

			if (prefab != null)
			{
				ground = Instantiate(prefab, transform.position, prefab.transform.rotation, transform);
			}
		}
	}


	public void CreatePiece(PieceTypes newPieceTypes)
	{
		pieceType = newPieceTypes;

		if (piece != null)
		{
			DestroyImmediate(piece);
		}

		if (pieceType != PieceTypes.Nothing)
		{
			GameObject prefab = GridManager.Instance.GetPiecePrefab(pieceType);

			if (prefab != null)
			{
				piece = Instantiate(prefab, transform.position, prefab.transform.rotation, transform);
			}
		}
	}

	public void ChangePiece(GameObject newPiece, PieceTypes newPieceTypes)
	{
		piece = newPiece;
		pieceType = newPieceTypes;

#if UNITY_EDITOR
		_pieceType = pieceType;
#endif
	}

	public void RemovePiece()
	{
		piece = null;
		pieceType = PieceTypes.Nothing;

#if UNITY_EDITOR
		_pieceType = pieceType;
#endif
	}

	public void DestroyPiece()
	{
		Destroy(piece);

		piece = null;
		pieceType = PieceTypes.Nothing;

#if UNITY_EDITOR
		_pieceType = pieceType;
#endif
	}

#if UNITY_EDITOR

	//[Header("DEBUG")]
	//[SerializeField]
	private GroundTypes _groundType = GroundTypes.Void;
	//[SerializeField]
	private PieceTypes _pieceType = PieceTypes.Nothing;

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			return;
		}

		if (pieceType != _pieceType)
		{
			CreatePiece(pieceType);
			_pieceType = pieceType;
		}

		if (groundType != _groundType)
		{
			CreateGround(groundType);
			_groundType = groundType;
		}
	}
#endif
}

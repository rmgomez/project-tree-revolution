using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public GroundTypes groundType = GroundTypes.Void;
	[HideInInspector]
	public Ground ground;

	public PieceTypes pieceType = PieceTypes.Nothing;
	[HideInInspector]
	public Piece piece;

	[HideInInspector]
	public VisualTileInfo visualTileInfo;

	public bool canPlantOnIt = true;
	public Material alternative_tile_material;

	private void Awake()
	{
		visualTileInfo = GetComponentInChildren<VisualTileInfo>();
	}

	public void CreateGround(GroundTypes newGroundType)
	{
		bool hay = false;
		if (gameObject.name.EndsWith("_b")) { hay = true; }

		groundType = newGroundType;

		if (ground != null)
		{
			DestroyImmediate(ground.gameObject);
		}

		if (groundType != GroundTypes.Void)
		{
			Ground prefab = GridManager.Instance.GetGroundPrefab(groundType);

			if (prefab != null)
			{
				// prefab.transform.GetChild(0).GetComponent<Renderer>().material = alternative_tile_material;

				ground = Instantiate(prefab, transform.position, prefab.transform.rotation, transform);
				// ground.transform.GetChild(0).GetComponent<Renderer>().material = alternative_tile_material;

				if (hay)
				ground.GetComponent<Renderer>().material = alternative_tile_material;

				canPlantOnIt = prefab.canPlantOnIt;
			}
		}
	}


	public void CreatePiece(PieceTypes newPieceTypes)
	{
		pieceType = newPieceTypes;

		if (piece != null)
		{
			DestroyImmediate(piece.gameObject);
		}

		if (pieceType != PieceTypes.Nothing)
		{
			Piece prefab = GridManager.Instance.GetPiecePrefab(pieceType);

			if (prefab != null)
			{
				piece = Instantiate(prefab, transform.position, prefab.transform.rotation, transform);
			}
		}
	}

	public void ChangePiece(Piece newPiece, PieceTypes newPieceTypes)
	{
		piece = newPiece;
		piece.transform.parent = transform;
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
		Destroy(piece.gameObject);

		piece = null;
		pieceType = PieceTypes.Nothing;

#if UNITY_EDITOR
		_pieceType = pieceType;
#endif
	}

	public void DoActionPlace(PieceTypes pieceType)
	{
		CreatePiece(pieceType);

		piece?.GetComponent<SoundComponent>()?.PlayPlaceOnGrid();
	}

	public void DoActionHeal( int healValue)
	{
		var life = piece?.GetComponent<LifeComponent>();

		if (life)
		{
			life.GetHeal(healValue);
		}
	}

	public void DoActionAttack(int attackValue)
	{
		var life = piece?.GetComponent<LifeComponent>();

		if (life)
		{
			life.GetDamage(attackValue);
		}
	}

	public bool TestIfCanDoAction(PlayerActions playerAction)
	{
		switch (playerAction)
		{
			case PlayerActions.Place:
				if (piece == null && canPlantOnIt)
				{
					return true;
				}
				break;

			case PlayerActions.Heal:
				if (piece != null && piece.CompareTag("Plant"))
				{
					var life = piece.GetComponent<LifeComponent>();

					if (life && life.CanBeHeal)
					{
						return true;
					}
				}
				break;

			case PlayerActions.Attack:
				if (piece != null && piece.CompareTag("Enemy"))
				{
					var life = piece.GetComponent<LifeComponent>();

					if (life)
					{
						return true;
					}
				}
				break;
		}

		return false;
	}

	private void OnMouseEnter()
	{
		visualTileInfo.ChangeColor(VisualTileInfos.Over, true);
	}

	private void OnMouseExit()
	{
		visualTileInfo.ChangeColor(VisualTileInfos.Hide, true);
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

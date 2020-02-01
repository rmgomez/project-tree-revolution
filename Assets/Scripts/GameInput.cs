using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    [SerializeField] private LayerMask _tileMask;
    private Camera _camera;

    private bool _allowInput;
    
    private void OnEnable()
    {
        LevelManager.OnPlayerTurnStarted += OnPlayerTurnStarted;
        LevelManager.OnPlayerTurnCompleted += OnPlayerTurnCompleted;
    }

    private void OnDisable()
    {
        LevelManager.OnPlayerTurnStarted -= OnPlayerTurnStarted;
        LevelManager.OnPlayerTurnCompleted -= OnPlayerTurnCompleted;
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
        if (_camera == null) Debug.LogError("No camera  found, please attach script to camera");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _allowInput && LevelManager.Instance.CanCastActiveSpell())
        {
            DetectTouch();
        }
    }

    private void DetectTouch()
    {
        Debug.Log("Player Clicked");

        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, 200, _tileMask))
        {
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();

            var data = LevelManager.Instance.GetActiveSpellData();
            Debug.Log($"Hit {tile.gameObject.name}");
            if (tile.TestIfCanDoAction(data.Action))
            {
                switch (data.Action)
                {
                    case PlayerActions.Place:
                        tile.DoActionPlace(data.PieceToSpawn);
                        break;
                    case PlayerActions.Heal:
                        tile.DoActionHeal(data.HealAmount);
                        break;
                    case PlayerActions.Attack:
                        tile.DoActionAttack(data.AttackAmount);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                LevelManager.Instance.CurrentNaturePoints -= data.NaturePoints;
            }
            
           
        }

    }

    private void SetTile(Tile tile)
    {
        //Tell tile to spawn
        //Tell tile to heal
    }

    private void OnPlayerTurnStarted()
    {
        _allowInput = true;
    }


    private void OnPlayerTurnCompleted()
    {
        _allowInput = false;
    }
}

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
        if (Input.GetMouseButtonDown(0) && _allowInput)
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
            Debug.Log($"Hit {hit.collider.gameObject.name}");
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

using UnityEngine;

public class GameInput : MonoBehaviour
{

    [SerializeField] private LayerMask _tileMask;
    private Camera _camera;
    
    void Start()
    {
        _camera = GetComponent<Camera>();
        if (_camera == null) Debug.LogError("No camera  found, please attach script to camera");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectTouch();
        }
    }

    void DetectTouch()
    {
        //Debug.Log("Player Clicked");

        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, 200, _tileMask))
        {
            Debug.Log($"Hit {hit.collider.gameObject.name}");
        }

    }
}

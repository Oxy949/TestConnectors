using UnityEngine;

/// <summary>
/// Drag the object with mouse 
/// </summary>
[RequireComponent(typeof(Collider))] // Needs a collider
public class DragHandler : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] private Transform platformRoot;

    // Is object dragging now?
    private bool _isDragging;
    
    // Save original mouse offset
    private Vector3 _originalHitPoint;
    private Vector3 _offset;

    // cache the cam
    private Camera _targetCamera;

    private void Start()
    {
        _targetCamera = Camera.main; // TODO: Replace if multiple cameras
    }

    private void OnMouseDown()
    {
        _originalHitPoint = platformRoot.position;
        
        Ray ray = _targetCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            _originalHitPoint = hit.point;
            _offset = platformRoot.position - _originalHitPoint;
        }
        
        _isDragging = true;
    }

    private void Update()
    {
        if (_isDragging)
        {
            Vector3 curPosition = GetDraggingPoint() + _offset;
            curPosition.y = platformRoot.position.y;
            platformRoot.position = curPosition; // set new position
        }
    }

    private Vector3 GetDraggingPoint()
    {
        Ray ray = _targetCamera.ScreenPointToRay(Input.mousePosition);
        Plane xzPlane = new Plane(Vector3.up, new Vector3(0, _originalHitPoint.y, 0));
        xzPlane.Raycast(ray, out var distance);
        var cursorWorldPosition = ray.GetPoint(distance);
        return cursorWorldPosition;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }
}
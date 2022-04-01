using UnityEngine;

/// <summary>
/// Line between 2 Connectable
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class ConnectorLine : MonoBehaviour
{
    private LineRenderer _lineRenderer; // LineRenderer ref

    // Save target Connectables
    private Connectable _fromConnectable;
    private Connectable _toConnectable;


    // cache Connectables position (for update when changed)
    private Vector3 _fromPosition;
    private Vector3 _toPosition;

    private bool _checkPositionChange = false;

    // cache the cam
    private Camera _targetCamera;

    private void Awake()
    {
        // Cache LineRenderer
        _lineRenderer = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// Init the spawned prefab
    /// </summary>
    /// <param name="from">Connectable 1</param>
    /// <param name="to"> Connectable 2</param>
    /// <param name="cam">Connector camera</param>
    public void Init(Connectable from, Connectable to, Camera cam = null)
    {
        _fromConnectable = from;
        _toConnectable = to;

        if (cam == null)
            _targetCamera = Camera.main;
        else
            _targetCamera = cam;

        _checkPositionChange = true;
    }

    private void Update()
    {
        if (_checkPositionChange) // Update only when needed
        {
            if (IsConnectablePositionsChanged()) // Redraws line when positions has changed
            {
                SaveCurrentPositions();
                RedrawLine();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>TRUE is positions has changed</returns>
    private bool IsConnectablePositionsChanged()
    {
        return GetPosition1() != _fromPosition || GetPosition2() != _toPosition;
    }

    /// <summary>
    /// Saves current positions to cache
    /// </summary>
    private void SaveCurrentPositions()
    {
        _fromPosition = GetPosition1();
        _toPosition = GetPosition2();
    }

    private Vector3 GetPosition1()
    {
        if (!_fromConnectable)
            return GetMousePosition();
        return _fromConnectable.Anchor.position;
    }

    private Vector3 GetPosition2()
    {
        if (!_toConnectable)
            return GetMousePosition();
        return _toConnectable.Anchor.position;
    }

    private Vector3 GetMousePosition()
    {
        var cursorWorldPosition = _targetCamera.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3()
        {
            x = cursorWorldPosition.x,
            y = _fromPosition.y,
            z = cursorWorldPosition.z
        };
    }

    /// <summary>
    /// Draws line between _fromPosition and _toPosition
    /// </summary>
    private void RedrawLine()
    {
        _lineRenderer.SetPositions(new[] {_fromPosition, _toPosition});
    }
}
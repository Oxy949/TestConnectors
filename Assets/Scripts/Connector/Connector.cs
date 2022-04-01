using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Connects the Connectable with line in 2 modes
/// </summary>
public class Connector : MonoBehaviour
{
    [Header("Options:")]
    [SerializeField] private bool showLineInSelectionMode1 = true;

    [Header("Prefabs:")]
    [SerializeField] private ConnectorLine connectedLinePrefab;

    [Tooltip("Use custom prefab for connection preview")]
    [SerializeField] private bool useCustomDrawConnection; // Use custom prefab for preview
    [SerializeField] private ConnectorLine customDrawConnectionPrefab; // in case of custom connection line

    // cache the cam
    private Camera _targetCamera;

    // Connectable logic
    private Connectable _currentSelected; // original (first) Connectable
    private Connectable _prevConnectable; // last highlighted Connectable
    
    // Line preview
    private bool _isDrawing; // Determinate if line is drawing right now 
    private ConnectorLine _drawingLine; // the drawing temp line 

    
    private void Start()
    {
        // Cache Camera
        _targetCamera = Camera.main; // TODO: Replace if multiple cameras
    }

    private void Update()
    {
        // check LMB pressed this frame
        if (Input.GetMouseButtonDown(0))
        {
            // if Connectable under cursor 
            if (IsConnectionAllowed())
            {
                var connectable = GetConnectableUnderMouse(); // get it

                // if no any selection before
                if (_currentSelected == null)
                {
                    HighlightAllConnectable();

                    StartDrawing(connectable);

                    _currentSelected = connectable;
                    _currentSelected.Select();
                }
                else // there was a selection before! Check pair 
                {
                    if (connectable != _currentSelected) // if not self
                    {
                        Connect(_currentSelected, connectable);
                    }

                    // Stop selection
                    StopDrawing();
                    ResetSelection();
                }
            }
            else // No object or not Connectable
            {
                // Stop selection
                ResetSelection();
                StopDrawing();
            }
        }

        if (_isDrawing) // if has a selected object
        {
            // Select 'Connectable' on mouse over and revert when mouse out
            if (IsConnectionAllowed()) // Connectable under cursor 
            {
                var connectable = GetConnectableUnderMouse(); // get it
                
                // fix if Connectable is colliding
                if (_prevConnectable != null) // clear old
                    _prevConnectable.Highlight();

                if (connectable != _currentSelected) // if not self
                {
                    _prevConnectable = connectable;
                    _prevConnectable.Select();
                }
            }
            else // Revert changes
            {
                if (_prevConnectable != null)
                {
                    _prevConnectable.Highlight();
                    _prevConnectable = null;
                }
            }
            

            // Check Mouse Button Up event to complete the selection
            if (Input.GetMouseButtonUp(0)) // released LMB
            {
                if (IsConnectionAllowed()) // Cursor over Connectable 
                {
                    var connectable = GetConnectableUnderMouse();

                    if (_prevConnectable != null) // has target for connection
                    {
                        Connect(_currentSelected, _prevConnectable); // connect them
                        
                        // Reset connection 
                        StopDrawing();
                        ResetSelection();
                    }
                    else // no target for connection
                    {
                        // Remove line if not needed
                        if (!showLineInSelectionMode1)
                            StopDrawing();
                    }

                    if (connectable != _currentSelected) // if not self
                    {
                        StopDrawing();
                        ResetSelection();
                    }
                }
                else // Cursor not over Connectable
                {
                    // Reset connection 
                    StopDrawing();
                    ResetSelection();
                }
            }
        }
    }

    /// <summary>
    /// Check if there is a Connectable under mouse
    /// </summary>
    /// <returns></returns>
    private bool IsConnectionAllowed()
    {
        return GetConnectableUnderMouse() != null;
    }

    private Connectable GetConnectableUnderMouse()
    {
        Ray ray = _targetCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            return hit.transform.GetComponent<Connectable>();
        }

        return null;
    }

    /// <summary>
    /// Draws a line from Connectable to mouse cursor
    /// </summary>
    /// <param name="connectable"></param>
    private void StartDrawing(Connectable connectable)
    {
        _isDrawing = true;

        _drawingLine = Instantiate(useCustomDrawConnection ? customDrawConnectionPrefab : connectedLinePrefab, Vector3.zero, Quaternion.identity);
        _drawingLine.Init(connectable, null, _targetCamera);
    }

    /// <summary>
    /// Remove the drawing line
    /// </summary>
    private void StopDrawing()
    {
        if (_drawingLine != null)
            Destroy(_drawingLine.gameObject);
        _isDrawing = false;
    }

    private void HighlightAllConnectable()
    {
        foreach (var connectable in FindObjectsOfType<Connectable>())
        {
            connectable.Highlight();
        }
    }

    private void ResetSelection()
    {
        foreach (var connectable in FindObjectsOfType<Connectable>())
        {
            connectable.Deselect();
        }

        _currentSelected = null;
        _prevConnectable = null;
    }

    /// <summary>
    /// Draw line between 'from' obj to 'to'. If 'to' == null, uses current mouse position
    /// </summary>
    /// <param name="from">Connectable 1</param>
    /// <param name="to">Connectable 2</param>
    private void Connect(Connectable from, Connectable to)
    {
        var lineObj = Instantiate(connectedLinePrefab, Vector3.zero, Quaternion.identity);
        lineObj.Init(from, to, _targetCamera);
    }
}
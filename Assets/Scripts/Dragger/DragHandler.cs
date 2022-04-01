﻿using UnityEngine;

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
    private Vector3 _screenPoint;
    private Vector3 _offset;

    // cache the cam
    private Camera _targetCamera;

    private void Start()
    {
        _targetCamera = Camera.main; // TODO: Replace if multiple cameras
    }

    private void OnMouseDown()
    {
        _screenPoint = _targetCamera.WorldToScreenPoint(platformRoot.position);
        _offset = platformRoot.position - _targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));

        _isDragging = true;
    }

    private void Update()
    {
        if (_isDragging)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            Vector3 curPosition = _targetCamera.ScreenToWorldPoint(curScreenPoint) + _offset;

            platformRoot.position = curPosition; // set new position
        }
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }
}
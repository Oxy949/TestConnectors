using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Renderer))]
public class ConnectableSphere : Connectable
{
    [Header("Materials:")]
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material highlightedMaterial;


    private Material _defaultMaterial; // TODO: Multiple materials?
    
    private Renderer _renderer;

    private void Awake()
    {
        // Cache Render
        _renderer = GetComponent<Renderer>();
        // Save material
        _defaultMaterial = _renderer.material;
    }

    public override void Select()
    {
        _renderer.material = selectedMaterial;
    }

    public override void Deselect()
    {
        _renderer.material = _defaultMaterial;
    }

    public override void Highlight()
    {
        _renderer.material = highlightedMaterial;
    }

}
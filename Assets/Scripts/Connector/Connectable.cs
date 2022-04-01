using UnityEngine;

/// <summary>
/// Base Connectable class. Used with Connector
/// </summary>
public abstract class Connectable : MonoBehaviour
{
    /// <summary>
    /// Called when the Connectable is marked for selected
    /// </summary>
    public abstract void Select();
    
    /// <summary>
    /// Called when the Connectable was deselected
    /// </summary>
    public abstract void Deselect();
    
    /// <summary>
    /// Called when the Connectable mean to be highlighted for selection
    /// </summary>
    public abstract void Highlight();

    /// <summary>
    /// Used for line placement
    /// </summary>
    public virtual Transform Anchor => transform;
}
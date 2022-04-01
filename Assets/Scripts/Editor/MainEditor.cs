using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Main))]
public class MainEditor : Editor
{
    /// <summary>
    /// Draw custom UI in scene view
    /// </summary>
    void OnSceneGUI()
    {
        var main = (Main)target;

        // Draw a red circle around object
        Handles.color = Color.red;
        Handles.DrawWireDisc(main.transform.position, main.transform.up, main.Radius);
    }
}
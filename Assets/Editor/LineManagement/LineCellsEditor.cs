using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineCells))]
public class LineCellsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LineCells lineCells = (LineCells)target;

        if (GUILayout.Button("Generate cells"))
        {
            lineCells.GenerateCells();
        }

        if (GUILayout.Button("Destroy cells"))
        {
            lineCells.DestroyCells();
        }
    }
}
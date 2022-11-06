using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineCellsGenerator))]
public class LineCellsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LineCellsGenerator lineCells = (LineCellsGenerator)target;

        if (GUILayout.Button("Generate cells"))
        {
            lineCells.GenerateCells();
        }

        if (GUILayout.Button("Destroy cells"))
        {
            lineCells.DestroyCells();
        }

        if (GUILayout.Button("Regenerate cells"))
        {
            lineCells.DestroyCells();
            lineCells.GenerateCells();
        }
    }
}
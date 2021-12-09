using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AmmoGui))]

public class AmmoGuiEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AmmoGui ammoGui = (AmmoGui)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Selected"))
        {
            ammoGui.Selected();
        }

        if (GUILayout.Button("Deselected"))
        {
            ammoGui.Deselected();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DisplayNoise))]
public class TempGenerateNoise : Editor
{
    public override void OnInspectorGUI()
    {
        DisplayNoise displayNoise = (DisplayNoise) target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            displayNoise.generateNoise();
            
        }
    }
}

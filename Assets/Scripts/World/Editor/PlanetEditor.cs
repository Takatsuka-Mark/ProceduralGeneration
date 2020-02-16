using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet planet;
    private Editor shapeEditor;
    private Editor colorEditor;
    
    
    public override void OnInspectorGUI()
    {
        using (var updateCheck = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (updateCheck.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.ShapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.ColorSettings, planet.OnColorSettingsUpdated, ref planet.colorSettingsFoldout, ref colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

            using (var updateCheck = new EditorGUI.ChangeCheckScope())
            {
                // true means that it is always open. Basically looks nicer in inspector
                // values aren't serialized here, so we can't store settings.

                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (updateCheck.changed)
                    {
                        onSettingsUpdated?.Invoke();
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        planet = (Planet) target;
    }
}

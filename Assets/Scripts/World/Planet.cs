using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Planet : MonoBehaviour
{
    [Range(2, 256)] public int resolution = 10;
    public bool autoUpdate = true;

    public enum FaceRenderMask
    {
        All,
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    }

    public FaceRenderMask faceRenderMask;


    public ShapeSettings ShapeSettings;
    public ColorSettings ColorSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colorSettingsFoldout;
    
    private ShapeGenerator _shapeGenerator;
    
    [SerializeField, HideInInspector]    // this hides, but also allows them to be saved.
    private MeshFilter[] meshFilters;
    private TerrainFace[] _terrainFaces;

    // This is an event function to work with the editor
    private void OnValidate()
    {
        init();
        GenerateMesh();
    }

    void init()
    {
        _shapeGenerator = new ShapeGenerator(ShapeSettings);
        
        // we only want to create new filters the first time
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];     // one for each face
        }

        _terrainFaces = new TerrainFace[6];
        // up, down, left, right, forward, back
        Vector3[] dir = {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back}; 

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            _terrainFaces[i] = new TerrainFace(_shapeGenerator, meshFilters[i].sharedMesh, resolution, dir[i]);

            bool renderFace = faceRenderMask == FaceRenderMask.All || (int) faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
        
    }

    public void GeneratePlanet()
    {
        init();
        GenerateMesh();
        GenerateColors();
    }

    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            init();
            GenerateColors();
        }
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            init();
            GenerateMesh();
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                _terrainFaces[i].GenerateMesh();
            }
        }
    }

    void GenerateColors()
    {
        foreach (var meshFilter in meshFilters)
        {
            meshFilter.GetComponent<MeshRenderer>().sharedMaterial.color = ColorSettings.color;
        }
    }
}

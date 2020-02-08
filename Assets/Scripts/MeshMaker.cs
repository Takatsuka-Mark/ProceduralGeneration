using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeshMaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh flatMesh = new Mesh();
        Vector3[] vertexArray = new Vector3[100];
        flatMesh.vertices = vertexArray;
    }

    Vector3[] convertImage(Texture2D aa)
    {
        return null;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}

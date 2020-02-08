using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using defaultNamespace;

public class MeshMaker
{
    private float[][] bwImage = FlatGenNoise.CalcNoise();
    void Start()
    {
              
        Mesh flatMesh = new Mesh();        //Instantiate new Mesh
        Vector3[] vertexArray = convertImage(bwImage);    
        flatMesh.vertices = vertexArray;
    }

    Vector3[] convertImage(float[][] bwImage)
    {
        foreach (var number in bwImage) //TODO Parallel foreach loop
        {
            
        }

        return null;
    }

    public float[][] GetbwImage()
    {
        return bwImage;
    }
}

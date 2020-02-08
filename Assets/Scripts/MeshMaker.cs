using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using defaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;
using UnityScript.Lang;

namespace defaultNamespace
{
    public class MeshMaker : MonoBehaviour
    {
        private FlatGenNoise noiseGen;
        private float[,] bwImage;
        private Vector3[] vertexArray;
        void Start()
        {
            noiseGen = new FlatGenNoise();
            bwImage = noiseGen.CalcNoise();
            
            Mesh flatMesh = GetComponent<MeshFilter>().mesh; //Instantiate new Mesh
            vertexArray = flatMesh.vertices;
            vertexArray = AddFloatNoise(vertexArray);
            flatMesh.SetVertices(vertexArray);
            flatMesh.UploadMeshData(false);
            flatMesh.RecalculateBounds();
        }

        Vector3[] AddFloatNoise(Vector3[] vertexArray)
        {
            var bwImage1D = FlatGenNoise.Convert2Dto1D(bwImage);
            for (int i = 0; i < vertexArray.Length; i++)
            {
                vertexArray[i] += Vector3.up * (bwImage1D[i] * (float).01);
            }
            return vertexArray;
        }
        public float[,] GetbwImage()
        {
            return bwImage;
        }
    }
}
// using UnityEngine;
//
// public class MeshMaker : MonoBehaviour
// {
//     Mesh mesh;
//     Vector3[] vertices;
//     void Start()
//     {
//         mesh = GetComponent<MeshFilter>().mesh;
//         vertices = mesh.vertices;
//     }
//
//     void Update()
//     {
//         for (var i = 0; i < vertices.Length; i++)
//         {
//             vertices[i] += Vector3.up * Time.deltaTime;
//         }
//
//         // assign the local vertices array into the vertices array of the Mesh.
//         mesh.vertices = vertices;
//         mesh.RecalculateBounds();
//     }
// }
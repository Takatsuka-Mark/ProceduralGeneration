using System;
using System.Collections;
using System.Collections.Generic;
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
        void Start()
        { 
            noiseGen = new FlatGenNoise();
            bwImage = noiseGen.CalcNoise();
            Mesh flatMesh = new Mesh(); //Instantiate new Mesh
            Vector3[] vertexArray = ConvertImage(bwImage);
            flatMesh.vertices = vertexArray;
        }

        Vector3[] ConvertImage(float[,] bwImage)
        {
            int vectorIndex = 0;
            Vector3[] vectorArray = new Vector3[Constants.ChunkHeight * Constants.ChunkWidth];
            foreach (var number in bwImage) //TODO Parallel foreach loop
            {
                Vector3 vec = new Vector3();
                vec.Set(0,number, 0);
                vectorArray[vectorIndex] = vec;
            }

            return null;
        }

        public float[,] GetbwImage()
        {
            return bwImage;
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using defaultNamespace;
using UnityEngine;
using static defaultNamespace.Constants;

/*
 * This will take in parameters, and from those parameters return a map of Perlin noise.
 */
namespace defaultNamespace
{
    public class FlatGenNoise
    {
        public float Persistance;
        public float Lacunarity;
        public Vector2Int CurrChunk;

        //TODO can do a Vector2Int for the 

        /// <summary>
        /// 
        /// </summary>
        public FlatGenNoise()
        {
            Persistance = 1.0f;
            Lacunarity = 1.0f;
            CurrChunk.x = 0;
            CurrChunk.y = 0;
            System.Random rnJesus = new System.Random(Constants.Seed);
        }

        public float[,] CalcNoise()
        {
            float[,] map = new float[Constants.ChunkHeight,Constants.ChunkWidth];

            var maxNoise = 255.0f;
            var minNoise = 0.0f;
            Vector2Int offset = new Vector2Int(CurrChunk.x * ChunkHeight, CurrChunk.y * ChunkWidth);
            
            for (int height = 0; height < Constants.ChunkHeight; height += 1)
            {
                for (int width = 0; width < Constants.ChunkWidth; width += 1)
                {
                    map[height, width] = Mathf.PerlinNoise(height + offset.x, width + offset.y);
                }
            }

            return map;
        }
    }
}

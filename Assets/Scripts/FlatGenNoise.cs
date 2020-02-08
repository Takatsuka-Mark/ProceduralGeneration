using System;
using System.Collections;
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
        private float Persistance;
        private float Lacunarity;
        private Vector2Int CurrChunk;

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
            // System.Random rnJesus = new System.Random(Constants.Seed);
        }

        public float[,] CalcNoise()
        {
            float[,] map = new float[Constants.ChunkHeight,Constants.ChunkWidth];

            var maxNoise = 255.0f;
            var minNoise = 0.0f;
            var scaleFactor = 255;
            
            Vector2Int offset = new Vector2Int(CurrChunk.x * ChunkHeight, CurrChunk.y * ChunkWidth);
            
            for (int height = 0; height < Constants.ChunkHeight; height += 1)
            {
                for (int width = 0; width < Constants.ChunkWidth; width += 1)
                {
                    Debug.Log(height);
                    // map[height, width] = Mathf.PerlinNoise((float)height / Constants.ChunkHeight, (float) width / Constants.ChunkWidth);
                    map[height, width] = Mathf.PerlinNoise((float)height/(float)Constants.ChunkHeight, (float)width/(float)Constants.ChunkWidth);
                    map[height, width] *= scaleFactor;
                    //map[height, width] = Mathf.Clamp(map[height, width], minNoise, maxNoise);
                    
                    Debug.Log(map[height, width]);
                }
            }
            return map;
        }

        public static float[] Convert2Dto1D(float[,] sacrifice)
        {
            float[] map = new float[Constants.ChunkHeight * Constants.ChunkWidth];
            for (int height = 0; height < Constants.ChunkHeight; height += 1)
            {
                for (int width = 0; width < Constants.ChunkWidth; width += 1)
                {
                    // Pixels[(int) height * Constants.ChunkWidth + (int) width] = Color.Lerp(Color.black, Color.white, value/255);
                    map[(int) height * Constants.ChunkWidth + (int) width] = sacrifice[height, width];
                }
            }

            return map;
        }
    }
}

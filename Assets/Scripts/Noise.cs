using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace defaultNamespace
{
    public class Noise
    {

        private float Persistance;
        private float Lacunarity;
        private Vector2Int CurrChunk;

        public Noise() {
            Persistance = 1.0f;
            Lacunarity = 1.0f;
            CurrChunk.x = 0;
            CurrChunk.y = 0;
            System.Random rnJesus = new System.Random(Constants.Seed);
        }
            
        public float[,] getNoise(){
            float[,] map = new float[Constants.ChunkHeight,Constants.ChunkWidth];

            var maxNoise = 255.0f;
            var minNoise = 0.0f;
            var scaleFactor = 0.1f;

            Vector2Int offset = new Vector2Int(CurrChunk.x * Constants.ChunkHeight, CurrChunk.y * Constants.ChunkWidth);
            
            for (int height = 0; height < Constants.ChunkHeight; height += 1)
            {
                for (int width = 0; width < Constants.ChunkWidth; width += 1)
                {
                    float sampleX = (float)height/(float)scaleFactor;
                    float sampleY = (float)width/(float)scaleFactor;
                    map[height, width] = Mathf.PerlinNoise(sampleX, sampleY);
                    map[height, width] *= scaleFactor;
                }
            }
            return map;
        }
    }

}
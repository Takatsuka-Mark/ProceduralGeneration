using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace defaultNamespace
{
    public class Noise
    {
        /// <summary>
        /// Literally scales up and down.
        /// </summary>
        private float ScaleFactor;

        /// <summary>
        /// Number of levels of detail?
        /// </summary>
        private int NumOctaves;
        
        /// <summary>
        /// How much each octive contributes to shape - adjustment of amplitude
        /// </summary>
        private float Persistance;
        
        /// <summary>
        /// How much detail is added / taken away throughout the octave
        /// </summary>
        private float Lacunarity;
        
        /// <summary>
        /// This is the current chunk that the camera is in. Each sequential chunk is indexed by 1
        /// </summary>
        private Vector2Int CurrChunk;

        private float minNoise = 0.0f;
        private float maxNoise = 1.0f;

        private System.Random rnJesus;

        public Noise() {
            CurrChunk.x = 0;
            CurrChunk.y = 0;
            
            NumOctaves = 4;
            Persistance = 0.5f;
            Lacunarity = 1.87f;
            ScaleFactor = 27.6f;
            
            rnJesus = new System.Random(Constants.Seed);
        }

        public Noise(int seed, Vector2Int currChunk, float persistance, float lacunarity, int numOctaves, float scaleFactor)
        {
            Persistance = persistance;
            Lacunarity = lacunarity;
            NumOctaves = numOctaves;
            ScaleFactor = scaleFactor;

            CurrChunk = currChunk;

            rnJesus = new System.Random(seed);
        }
        
        
        /// <summary>
        /// This function will generate noise, and output a float array with the noise.
        /// </summary>
        /// <returns> Float 2D array, indexed as [height][width], with values 0 to 1 </returns>
        public float[,] getNoise(){
            float[,] map = new float[Constants.ChunkHeight,Constants.ChunkWidth];

            Vector2[] offset = new Vector2[NumOctaves];
            for (int i = 0; i < NumOctaves; i++)
            {
                float x = rnJesus.Next(-10000, 10000);
                float y = rnJesus.Next(-10000, 10000);
                offset[i] = new Vector2(x, y);
            }

            if (ScaleFactor <= 0)
            {
                ScaleFactor = 0.0001f;
            }
            
            for (int height = 0; height < Constants.ChunkHeight; height += 1)
            {
                for (int width = 0; width < Constants.ChunkWidth; width += 1)
                {
                    var heightAccum = 0.0f;
                    var amplitude = 1.0f;
                    var frequency = 1.0f;
                    
                    // step through octaves
                    for (int i = 0; i < NumOctaves; i += 1)
                    {
                        var sampleX = height / ScaleFactor * frequency + offset[i].x;
                        var sampleY = width / ScaleFactor * frequency + offset[i].y;
                        var noiseValue = Mathf.PerlinNoise(sampleX, sampleY);    // figure out why * 2 - 1

                        heightAccum += noiseValue * amplitude;
                        amplitude *= Persistance;
                        frequency *= Lacunarity;
                    }

                    map[height, width] = Mathf.Clamp(heightAccum, minNoise, maxNoise);
                    
                    // Debug.Log(map[height,width]);
                    // map[height, width] = Mathf.Clamp(map[height, width], minNoise, maxNoise);
                }
            }

            // for (int height = 0; height < Constants.ChunkHeight; height += 1)
            // {
            //     for (int width = 0; width < Constants.ChunkWidth; width += 1)
            //     {
            //         map[height, width] = Mathf.InverseLerp(minNoise, maxNoise, map[height, width]);
            //         // Debug.Log(map[height,width]);
            //     }
            // }

            return map;
        }
    }

}
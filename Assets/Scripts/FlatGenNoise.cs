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
        private Noise noiseGen;
        
        //TODO can do a Vector2Int for the 
        /// <summary>
        /// 
        /// </summary>
        public FlatGenNoise()
        {
            noiseGen = new Noise();
        }

        public float[] CalcNoise()
        {
            return Convert2Dto1D(noiseGen.getNoise());
        }

        private float[] Convert2Dto1D(float[,] sacrifice)
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

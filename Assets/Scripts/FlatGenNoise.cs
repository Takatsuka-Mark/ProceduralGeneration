using System.Collections;
using System.Collections.Generic;
using defaultNamespace;
using UnityEngine;
using static defaultNamespace.Constants;

/*
 * This will take in parameters, and from those parameters return a map of Perlin noise.
 */

public class FlatGenNoise
{
    public float persistance;
    public float lacunarity;
    public float[,] map;
    public Vector2Int currChunk;

    //TODO can do a Vector2Int for the 
    
    /// <summary>
    /// 
    /// </summary>
    public FlatGenNoise()
    {
        persistance = 1.0f;
        lacunarity = 1.0f;
        currChunk.x = 0;
        currChunk.y = 0;
    }

    public float[,] CalcNoise()
    {
        float[,] map;
        for (int height = 0; height < Constants.ChunkHeight; height += 1)
        {
            for (int width = 0; width < Constants.ChunkWidth; width += 1)
            {
                
            }
        }

        return null;
    }
    
}

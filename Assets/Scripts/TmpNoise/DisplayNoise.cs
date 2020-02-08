using System.Collections;
using System.Collections.Generic;
using defaultNamespace;
using UnityEngine;

public class DisplayNoise : MonoBehaviour
{
    private FlatGenNoise NoiseGenerator;
    private Texture2D NoiseTexture;
    private Renderer Render;
    private Color[] Pixels;
    
    // Start is called before the first frame update
    void Start()
    {
        NoiseGenerator = new FlatGenNoise();
        NoiseTexture = new Texture2D(Constants.ChunkWidth, Constants.ChunkHeight);
        Pixels = new Color[Constants.ChunkWidth * Constants.ChunkHeight];
        Render.material.mainTexture = NoiseTexture;
        
        LoadMapIntoPixels();
    }

    // Update is called once per frame
    void Update()
    {
        NoiseTexture.SetPixels(Pixels);
        NoiseTexture.Apply();
    }

    void LoadMapIntoPixels()
    {
        var map = NoiseGenerator.CalcNoise();
        for (int height = 0; height < Constants.ChunkHeight; height += 1)
        {
            for (int width = 0; width < Constants.ChunkWidth; width += 1)
            {
                var value = map[height, width];
                Pixels[(int)height * Constants.ChunkHeight + (int)width] = new Color(value, value, value);
            }
        }
    }
}

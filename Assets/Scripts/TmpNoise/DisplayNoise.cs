using System.Collections;
using System.Collections.Generic;
using defaultNamespace;
using UnityEditor.UI;
using UnityEngine;

public class DisplayNoise : MonoBehaviour
{
    private FlatGenNoise NoiseGenerator;
    private Texture2D NoiseTexture;
    public Renderer textureRenderer;
    private Color[] Pixels;


    public int seed = 0;
    public int NumOctaves = 4;
    public float Persistance = 0.5f;
    public float Lacunarity = 1.87f;
    public float ScaleFactor = 27.6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        generateNoise();
        NoiseTexture.SetPixels(Pixels);
        NoiseTexture.Apply();
        textureRenderer.sharedMaterial.mainTexture = NoiseTexture;
        textureRenderer.transform.localScale = new Vector3(Constants.ChunkWidth,1, Constants.ChunkHeight);
        
        
    }

    public void generateNoise()
    {
        NoiseTexture = new Texture2D(Constants.ChunkWidth, Constants.ChunkHeight);
        Pixels = new Color[Constants.ChunkWidth * Constants.ChunkHeight];
        NoiseGenerator = new FlatGenNoise(seed, new Vector2Int(0, 0), Persistance, Lacunarity, NumOctaves, ScaleFactor);

        LoadMapIntoPixels();
        NoiseTexture.SetPixels(Pixels);
        NoiseTexture.Apply();
        textureRenderer.sharedMaterial.mainTexture = NoiseTexture;
        textureRenderer.transform.localScale = new Vector3(Constants.ChunkWidth,1, Constants.ChunkHeight);
    }

    void LoadMapIntoPixels()
    {
        float[] map = NoiseGenerator.CalcNoise();
        for (int i = 0; i < Constants.ChunkHeight * Constants.ChunkWidth; i += 1)
        {
            Pixels[i] = Color.Lerp(Color.black, Color.white, map[i]);
            //Pixels[(int) height * Constants.ChunkWidth + (int) width] = new Color(value/255, value/255, value/255, 1);
        }
    }
}

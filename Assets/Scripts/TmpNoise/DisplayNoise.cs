using System.Collections;
using System.Collections.Generic;
using defaultNamespace;
using UnityEngine;

public class DisplayNoise : MonoBehaviour
{
    private FlatGenNoise NoiseGenerator;
    private Texture2D NoiseTexture;
    public Renderer textureRenderer;
    private Color[] Pixels;


    // Start is called before the first frame update
    void Start()
    {

        
        // textureRenderer = GetComponent<Renderer>();
        // textureRenderer.material.SetTexture("_MainTex", NoiseTexture);
    }

    // Update is called once per frame
    void Update()
    {

        NoiseTexture.SetPixels(Pixels);
        NoiseTexture.Apply();
        textureRenderer.sharedMaterial.mainTexture = NoiseTexture;
        textureRenderer.transform.localScale = new Vector3(Constants.ChunkWidth,1, Constants.ChunkHeight);
        
        
    }

    public void generateNoise()
    {
        NoiseTexture = new Texture2D(Constants.ChunkWidth, Constants.ChunkHeight);
        Pixels = new Color[Constants.ChunkWidth * Constants.ChunkHeight];
        NoiseGenerator = new FlatGenNoise();

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
            var value = map[i];
                Pixels[i] = Color.Lerp(Color.black, Color.white, value/255);
                //Pixels[(int) height * Constants.ChunkWidth + (int) width] = new Color(value/255, value/255, value/255, 1);
        }
    }
}

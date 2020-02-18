using System.Collections;
using System.Collections.Generic;
using defaultNamespace;
using UnityEngine;

public class NoiseFilter
{
    private SimplexNoise noise = new SimplexNoise(0);
    // Noise noise = new Noise();
    private NoiseSettings settings;

    public NoiseFilter(NoiseSettings settings)
    {
        this.settings = settings;
    }
    public float Generate(Vector3 point)
    {
        // var noiseValue = noise.generateNoise(point); 
        float noiseValue = 0;
        float freqency = settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            var v = (float)noise.generateNoise(point * freqency + settings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            freqency *= settings.roughness;
            amplitude *= settings.persistence;
        }


        // noiseValue = (noise.generateNoise(point * settings.roughness + settings.center) + 1) * 0.5f;
        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return (float)noiseValue * settings.strength;    // TODO we can change this to a double
    }
}

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
        var noiseValue = (noise.generateNoise(point * settings.roughness + settings.center) + 1) * 0.5f;
        return (float)noiseValue * settings.strength;    // TODO we can change this to a double
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    private ShapeSettings settings;
    private NoiseFilter _noiseFilter;

    public ShapeGenerator(ShapeSettings settings)
    {
        this.settings = settings;
        _noiseFilter = new NoiseFilter(settings.noiseSettings);
    }

    public Vector3 CalcPointOnPlanet(Vector3 pointOnSphere)
    {
        float elevation = _noiseFilter.Generate(pointOnSphere);
        return pointOnSphere * (settings.radius * (1 + elevation));
    }
}

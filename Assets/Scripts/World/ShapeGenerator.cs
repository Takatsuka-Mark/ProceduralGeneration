using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    private ShapeSettings settings;
    private NoiseFilter[] _noiseFilters;

    public ShapeGenerator(ShapeSettings settings)
    {
        this.settings = settings;
        _noiseFilters = new NoiseFilter[settings.noiseLayers.Length];

        for (int i = 0; i < _noiseFilters.Length; i++)
        {
            _noiseFilters[i] = new NoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
    }

    public Vector3 CalcPointOnPlanet(Vector3 pointOnSphere)
    {
        float elevation = 0;
        float firstLayerValue = 0;

        if (_noiseFilters.Length > 0)
        {
            firstLayerValue = _noiseFilters[0].Generate(pointOnSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }
        
        for (int i = 1; i < _noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += _noiseFilters[i].Generate(pointOnSphere) * mask;
            }
        }

        return pointOnSphere * (settings.radius * (1 + elevation));
    }
}

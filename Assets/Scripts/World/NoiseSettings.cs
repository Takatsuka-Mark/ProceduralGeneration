using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType
    {
        Basic,
        Rigid
    }

    [ConditionalHide("filterType", 0)]
    public BasicNoiseSettings basicNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;

    public FilterType filterType;
    
    [System.Serializable]
    public class BasicNoiseSettings
    {
        public float strength = 1;
        [Range(1,8)]
        public int numLayers = 1;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = 0.5f;
        public Vector3 center;
        public float minValue;
    }

    [System.Serializable]
    public class RigidNoiseSettings : BasicNoiseSettings
    {
        public float weightMultiplier = 0.8f;
    }
    




}

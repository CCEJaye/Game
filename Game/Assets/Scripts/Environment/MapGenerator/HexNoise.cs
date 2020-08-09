using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Objects;

public class HexNoise
{

    public class NoiseValues
    {
        public float All { get; internal set; }
        public float[] Singles { get; internal set; }

        public NoiseValues(float all, float[] singles)
        {
            All = all;
            Singles = singles;
        }
    }

    public int Seed { get; }
    public int Octaves { get; }
    public float Persistance { get; }
    public float Lacunarity { get; }
    public float Scale { get; }
    public Vector2Int[] OctaveOffsets { get; }
    public float[] MinValues { get; }
    public float[] MaxValues { get; }
    public float MinValueAll { get; protected set; }
    public float MaxValueAll { get; protected set; }
    public Dictionary<Vector2, NoiseValues> ValueList { get; } = new Dictionary<Vector2, NoiseValues>();

    public HexNoise(int seed, int octaves, float persistance, float lacunarity, float scale)
    {
        Seed = seed;
        Octaves = octaves;
        Persistance = persistance;
        Lacunarity = lacunarity;
        Scale = scale;
        OctaveOffsets = new Vector2Int[octaves];
        MinValues = new float[octaves];
        MaxValues = new float[octaves];

        System.Random rand = new System.Random(seed);
        for (int i = 0; i < octaves; i++)
        {
            OctaveOffsets[i] = new Vector2Int(rand.Next(-100000, 100000), rand.Next(-100000, 100000));
            MinValues[i] = 1f;
            MaxValues[i] = 0f;
        }
    }

    public NoiseValues StoreRawValues(Vector2 realPos)
    {
        if (ValueList.ContainsKey(realPos)) return null;
        float[] values = new float[Octaves];
        float amplitude = 1f;
        float frequency = 1f;
        float value = 0f;

        for (int i = 0; i < Octaves; i++)
        {
            float aX = realPos.x * frequency / Scale + OctaveOffsets[i].x;
            float aY = realPos.y * frequency / Scale + OctaveOffsets[i].y;
            float aValue = Mathf.PerlinNoise(aX, aY);

            value += aValue * amplitude;
            amplitude *= Persistance;
            frequency *= Lacunarity;

            values[i] = aValue;

            if (aValue < MinValues[i]) MinValues[i] = aValue;
            if (aValue > MaxValues[i]) MaxValues[i] = aValue;
        }
        if (value < MinValueAll) MinValueAll = value;
        if (value > MaxValueAll) MaxValueAll = value;

        NoiseValues newValues = new NoiseValues(value, values);
        ValueList.Add(realPos, newValues);
        return newValues;
    }

    public void NormaliseAll()
    {
        foreach(KeyValuePair<Vector2, NoiseValues> value in ValueList)
        {
            value.Value.All = Mathf.InverseLerp(MinValueAll, MaxValueAll, value.Value.All);
            for (int i = 0; i < Octaves; i++)
            {
                value.Value.Singles[i] = Mathf.InverseLerp(MinValues[i], MaxValues[i], value.Value.Singles[i]);
            }
        }
    }
}

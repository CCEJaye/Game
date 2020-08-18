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
    public Vector2 HexBoundMin = new Vector2(float.MaxValue, float.MaxValue);
    public Vector2 HexBoundMax = new Vector2(float.MinValue, float.MinValue);
    public Dictionary<Vector2, NoiseValues> ValueList { get; } = new Dictionary<Vector2, NoiseValues>();
    public bool IsMeridianFading;
    public bool IsParallelFading;

    public HexNoise(int seed, int octaves, float persistance, float lacunarity, float scale, bool isEdgeFading, bool isParallelFading)
    {
        Seed = seed;
        Octaves = octaves;
        Persistance = persistance;
        Lacunarity = lacunarity;
        Scale = scale;
        OctaveOffsets = new Vector2Int[octaves];
        MinValues = new float[octaves];
        MaxValues = new float[octaves];
        IsMeridianFading = isEdgeFading;
        IsParallelFading = isParallelFading;

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
        if (realPos.x < HexBoundMin.x) HexBoundMin.x = realPos.x;
        if (realPos.y < HexBoundMin.y) HexBoundMin.y = realPos.y;
        if (realPos.x > HexBoundMax.x) HexBoundMax.x = realPos.x;
        if (realPos.y > HexBoundMax.y) HexBoundMax.y = realPos.y;

        NoiseValues newValues = new NoiseValues(value, values);
        ValueList.Add(realPos, newValues);
        return newValues;
    }

    public void NormaliseAll()
    {
        foreach(KeyValuePair<Vector2, NoiseValues> value in ValueList)
        {
            float meridianModifier = 1f;
            float parallelModifier = 1f;
            if (IsMeridianFading)
            {
                float amplitude = 2f;
                if (value.Key.x < HexBoundMin.x + Generation.ChunkRadius * amplitude)
                {
                    meridianModifier = (value.Key.x - HexBoundMin.x) / Generation.ChunkRadius / amplitude;
                }
                if (value.Key.x > HexBoundMax.x - Generation.ChunkRadius * amplitude)
                {
                    meridianModifier = (HexBoundMax.x - value.Key.x) / Generation.ChunkRadius / amplitude;
                }
            }
            if (IsParallelFading)
            {
                float amplitude = 1.5f;
                if (value.Key.y < HexBoundMin.y + Generation.ChunkRadius * amplitude)
                {
                    parallelModifier = (value.Key.y - HexBoundMin.y) / Generation.ChunkRadius / amplitude;
                }
                if (value.Key.y > HexBoundMax.y - Generation.ChunkRadius * amplitude)
                {
                    parallelModifier = (HexBoundMax.y - value.Key.y) / Generation.ChunkRadius / amplitude;
                }
            }
            value.Value.All = Mathf.InverseLerp(MinValueAll, MaxValueAll, value.Value.All) * meridianModifier * parallelModifier;
            for (int i = 0; i < Octaves; i++)
            {
                value.Value.Singles[i] = Mathf.InverseLerp(MinValues[i], MaxValues[i], value.Value.Singles[i]) * meridianModifier * parallelModifier;
            }
        }
    }
}

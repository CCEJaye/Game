using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

public class NoiseGenerator
{
    public class NoiseSettings
    {
        public int Seed;
        public int Width;
        public int Height;
        public int Octaves;
        public float Persistance;
        public float Lacunarity;
        public float Scale;
    }

    public class NoiseMap
    {
        public float[,] Combined;
        public List<float[,]> Individuals;

        public NoiseMap(NoiseSettings s)
        {
            Combined = new float[s.Width, s.Height];
            Individuals = new List<float[,]>();

            System.Random rand = new System.Random(s.Seed);
            Point[] octaveOffsets = new Point[s.Octaves];
            float[] minValues = new float[s.Octaves];
            float[] maxValues = new float[s.Octaves];
            for (int i = 0; i < s.Octaves; i++)
            {
                Individuals.Add(new float[s.Width, s.Height]);
                octaveOffsets[i] = new Point(rand.Next(-100000, 100000), rand.Next(-100000, 100000));
                minValues[i] = 1f;
                maxValues[i] = 0f;
            }

            PointF[,] hexPoints = HexTools.GetSideTopArray(s.Width, s.Height);
            float scale = (s.Width + s.Height) / 40 * s.Scale;
            float totalMinValue = 1f;
            float totalMaxValue = 0f;

            for (int y = 0; y < s.Height; y++)
            {
                for (int x = 0; x < s.Width; x++)
                {
                    float amplitude = 1f;
                    float frequency = 1f;
                    float value = 0f;

                    for (int i = 0; i < s.Octaves; i++)
                    {
                        float aX = (hexPoints[x, y].X - s.Width / 2) * frequency / scale + octaveOffsets[i].X;
                        float aY = (hexPoints[x, y].Y - s.Height / 2) * frequency / scale + octaveOffsets[i].Y;
                        float aValue = Mathf.PerlinNoise(aX, aY);

                        value += aValue * amplitude;
                        amplitude *= s.Persistance;
                        frequency *= s.Lacunarity;

                        Individuals[i][x, y] = aValue;

                        if (value < minValues[i]) minValues[i] = value;
                        if (value > maxValues[i]) maxValues[i] = value;
                    }

                    if (value < totalMinValue) totalMinValue = value;
                    if (value > totalMaxValue) totalMaxValue = value;

                    Combined[x, y] = value;
                }
            }

            for (int y = 0; y < s.Height; y++)
            {
                for (int x = 0; x < s.Width; x++)
                {
                    Combined[x, y] = Mathf.InverseLerp(totalMinValue, totalMaxValue, Combined[x, y]);
                    for (int i = 0; i < s.Octaves; i++)
                    {
                        Individuals[i][x, y] = Mathf.InverseLerp(minValues[i], maxValues[i], Individuals[i][x, y]);
                    }
                }
            }
        }
    }
}

using System;
using UnityEditor;
using UnityEngine;

public static class HexTools
{
    public static Vector2[,] GetPointTopArray(int width, int height)
    {
        Vector2[,] array = new Vector2[width, height];

        float xStep = 0.5f;
        float yStep = Mathf.Sqrt(3);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float aX = x * xStep;
                float aY = y * yStep + (x & 1) * yStep / 2;

                array[x, y] = new Vector2(aX, aY);
            }
        }

        return array;
    }

    public static Vector2[,] GetSideTopArray(int width, int height)
    {
        Vector2[,] array = new Vector2[width, height];

        float xStep = Mathf.Sqrt(3) / 2;
        float yStep = 0.5f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float aX = x * xStep;
                float aY = y + (x & 1) * yStep;

                array[x, y] = new Vector2(aX, aY);
            }
        }

        return array;
    }
}
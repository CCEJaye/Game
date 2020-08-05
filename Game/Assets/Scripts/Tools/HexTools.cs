using System;
using UnityEditor;
using UnityEngine;
using static Structs;

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

    public static PointF[,] GetSideTopArray(int width, int height)
    {
        PointF[,] array = new PointF[width, height];

        float xStep = Mathf.Sqrt(3) / 2;
        float yStep = 0.5f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float aX = x * xStep;
                float aY = y + (x & 1) * yStep;

                array[x, y] = new PointF(aX, aY);
            }
        }

        return array;
    }

    public static HexRefData GetRelativeHexCoords(int x, int y, int radius, Point chunk)
    {
        int countAll = 6 * (radius + (radius * radius - radius) / 2) + 1;
        int countCore = countAll - radius * 6;
        int countBorder = countAll - countCore;

        Point[] all = new Point[countAll];
        Point[] core = new Point[countCore];
        Point[] border = new Point[countBorder];
        Point[] corners = new Point[6];

        int indexAll = 0;
        int indexCore = 0;
        int indexBorder = 0;
        int indexCorner = 0;

        for (int c = -radius; c <= radius; c++)
        {
            int rows = 2 * radius + 1 - Math.Abs(c);
            int yMin = -(int) Math.Floor(rows / 2d);
            int yMax = (int) Math.Floor((rows - 1) / 2d);
            bool isEdge = c == -radius || c == radius;
            for (int r = yMin; r <= yMax; r++)
            {
                Point hex = new Point(x + c, y + r);
                all[indexAll] = hex;
                indexAll++;

                if (isEdge || r == yMin || r == yMax)
                {
                    border[indexBorder] = hex;
                    indexBorder++;
                } else
                {
                    core[indexCore] = hex;
                    indexCore++;
                }

                if ((isEdge || c == 0) && (r == yMin || r == yMax))
                {
                    corners[indexCorner] = hex;
                    indexCorner++;
                }

            }
        }

        return new HexRefData(new Point(x, y), all, core, border, corners, radius, chunk);
    }
}

public class HexRefData
{
    public Point Centre;
    public Point[] All;
    public Point[] Core;
    public Point[] Border;
    public Point[] Corners;
    public int Radius;
    public Point Chunk;

    public HexRefData(Point centre, Point[] all, Point[] core, Point[] border, Point[] corners, int radius, Point chunk)
    {
        Centre = centre;
        All = all;
        Core = core;
        Border = border;
        Corners = corners;
        Radius = radius;
        Chunk = chunk;
    }

    public HexRefData DuplicateWithOffset(int colShift, int rowShift)
    {
        Point[] all = new Point[All.Length];
        Point[] core = new Point[Core.Length];
        Point[] border = new Point[Border.Length];
        Point[] corners = new Point[Corners.Length];

        int yStep = 2 * (int)Math.Floor(Radius * 1.5d);
        int dX = colShift * Radius;
        int dY = rowShift * yStep + (colShift & 1) * yStep / 2;

        for (int i = 0; i < All.Length; i++)
        {
            all[i] = All[i].Offset(dX, dY);
        }
        for (int i = 0; i < Core.Length; i++)
        {
            core[i] = Core[i].Offset(dX, dY);
        }
        for (int i = 0; i < Border.Length; i++)
        {
            border[i] = Border[i].Offset(dX, dY);
        }
        for (int i = 0; i < Corners.Length; i++)
        {
            corners[i] = Corners[i].Offset(dX, dY);
        }

        return new HexRefData(new Point(Centre.X + dX, Centre.Y + dY), 
            all, core, border, corners, Radius, new Point(Chunk.X + colShift, Chunk.Y + rowShift));
    }
}

public class HexRefMetaData
{
    //Even radius is preffered because there are less wasted hexes
    public int ChunkRadius;
    //Micro
    public int Columns;
    public int Rows;
    //Macro
    public int Meridians;
    public int Parallels;

    public HexRefData[,] HexChunks;


    public HexRefMetaData(int columns, int rows, int chunkRadius)
    {
        ChunkRadius = chunkRadius;
        Columns = columns;
        Rows = rows;
        Meridians = (int) Math.Floor((Columns - 1d) / ChunkRadius) - 1;
        Parallels = (Rows - ChunkRadius + (int)Math.Floor((ChunkRadius - (ChunkRadius & 1)) / 3d)) / ChunkRadius / 3;
    }

    public void Populate()
    {
        HexChunks = new HexRefData[Meridians, Parallels];
        HexRefData initialChunk = HexTools.GetRelativeHexCoords(ChunkRadius, ChunkRadius, ChunkRadius, new Point(0, 0));
        HexChunks[0, 0] = initialChunk;
        for (int x = 0; x < Meridians; x++)
        {
            for (int y = 0; y < Parallels; y++)
            {
                if (x == 0 && y == 0) continue;
                HexChunks[x, y] = initialChunk.DuplicateWithOffset(x, y);
            }
        }
    }
}
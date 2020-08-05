using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

public class HexGenerator : MonoBehaviour
{
    public const int Meridians = 6;
    public const int Parallels = 4;
    public const int ChunkRadius = 12;

    public static WorldMeta Meta { get; } = new WorldMeta(Meridians, Parallels, ChunkRadius);

    public static readonly float XStepFlat = Mathf.Sqrt(3) / 2f;
    public static readonly float YStepFlat = 0.5f;
    public static readonly float XStepPoint = 0.5f;
    public static readonly float YStepPoint = Mathf.Sqrt(3);



    public GameObject Hex = null;
    public Worlds.WorldParameters WorldParams;
    public HexCell[,] Cells;
    private List<GameObject> Hexes = new List<GameObject>();

    public void GenerateMap(Worlds.WorldParameters p, int seed)
    {
        WorldParams = p;
        Dictionary<Vector2, HexCell> Cells = new Dictionary<Vector2, HexCell>();
        HashSet<Point> knownBorders = new HashSet<Point>();

        System.Random rand = new System.Random(seed);
        Point[] octaveOffsets = new Point[p.Octaves];
        float[] minValues = new float[p.Octaves];
        float[] maxValues = new float[p.Octaves];
        for (int i = 0; i < p.Octaves; i++)
        {
            octaveOffsets[i] = new Point(rand.Next(-100000, 100000), rand.Next(-100000, 100000));
            minValues[i] = 1f;
            maxValues[i] = 0f;
        }

        float scale = (Meta.Columns + Meta.Rows) / 40 * p.Scale;
        float totalMinValue = 1f;
        float totalMaxValue = 0f;

        foreach (ChunkData chunk in Meta.Chunks)
        {
            foreach (HexData hex in chunk.All)
            {
                if (hex.IsBorder && knownBorders.Contains(hex.Pos)) continue;
                if (hex.IsBorder) knownBorders.Add(hex.Pos);

                HexCell cell = new HexCell(p.Octaves);
                cell.Data = hex;
                float amplitude = 1f;
                float frequency = 1f;
                float value = 0f;

                for (int i = 0; i < p.Octaves; i++)
                {
                    float aX = (hex.RealPos.X - Meta.Columns / 2) * frequency / scale + octaveOffsets[i].X;
                    float aY = (hex.RealPos.Y - Meta.Rows / 2) * frequency / scale + octaveOffsets[i].Y;
                    float aValue = Mathf.PerlinNoise(aX, aY);

                    value += aValue * amplitude;
                    amplitude *= p.Persistance;
                    frequency *= p.Lacunarity;

                    cell.NoiseOctaves[i] = aValue;

                    if (value < minValues[i]) minValues[i] = value;
                    if (value > maxValues[i]) maxValues[i] = value;
                }

                if (value < totalMinValue) totalMinValue = value;
                if (value > totalMaxValue) totalMaxValue = value;

                cell.NoiseAll = value;
                Cells.Add(new Vector2(hex.Pos.X, hex.Pos.Y), cell);
            }
        }

        foreach (KeyValuePair<Vector2, HexCell> cell in Cells)
        {
            cell.Value.NoiseAll = Mathf.InverseLerp(totalMinValue, totalMaxValue, cell.Value.NoiseAll);
            for (int i = 0; i < p.Octaves; i++)
            {
                cell.Value.NoiseOctaves[i] = Mathf.InverseLerp(minValues[i], maxValues[i], cell.Value.NoiseOctaves[i]);

                Draw(cell.Value, p);
            }
        }
    }

    private void Draw(HexCell cell, Worlds.WorldParameters p)
    {
        GameObject hex = Instantiate(Hex, new Vector3(cell.Data.RealPos.X,
                cell.NoiseAll * 14.9f, cell.Data.RealPos.Y), Quaternion.identity);
        Hexes.Add(hex);
        //MeshRenderer renderer = hex.GetComponentInChildren<MeshRenderer>();
            
        //renderer.material.color = Color.Lerp(Color.black, Color.white, cell.NoiseAll);
    }

    public void ClearHexes()
    {
        foreach (GameObject hex in Hexes)
        {
            DestroyImmediate(hex);
        }
        Hexes.Clear();
    }

    public class HexCell
    {
        public HexData Data;
        public Color Colour;
        public float NoiseAll;
        public float[] NoiseOctaves;
        public GameObject Model;

        public HexCell(int octaves)
        {
            NoiseOctaves = new float[octaves];
        }
    }

    public class WorldMeta
    {
        public ChunkData[,] Chunks;
        public int Columns;
        public int Rows;
        public int Meridians;
        public int Parallels;
        public int ChunkRadius;

        public WorldMeta(int meridians, int parallels, int chunkRadius)
        {
            Chunks = new ChunkData[meridians, parallels];
            ChunkRadius = chunkRadius;
            Columns = chunkRadius * meridians * 2 + chunkRadius + 1;
            Rows = chunkRadius * parallels / 2 * 3 + chunkRadius / 2 + 1;
            Meridians = meridians;
            Parallels = parallels;

            ChunkData initialChunk = new ChunkData(chunkRadius);
            Chunks[0, 0] = initialChunk;
            for (int x = 0; x < meridians; x++)
            {
                for (int y = 0; y < parallels; y++)
                {
                    if (x == 0 && y == 0) continue;
                    Chunks[x, y] = initialChunk.OffsetCopy(x, y);
                }
            }
        }
    }

    public class ChunkData
    {
        public Point Centre;
        public HexData[] All;
        public HexData[] Core;
        public HexData[] Border;
        public HexData[] Corners;
        public int Radius;
        public Point Chunk;

        public ChunkData(Point centre, HexData[] all, HexData[] core,
            HexData[] border, HexData[] corners, int radius, Point chunk)
        {
            Centre = centre;
            All = all;
            Core = core;
            Border = border;
            Corners = corners;
            Radius = radius;
            Chunk = chunk;
        }

        public ChunkData(int radius) : this(new Point(radius, radius), radius, new Point(0, 0)) { }

        public ChunkData(Point centre, int radius, Point chunk)
        {
            int countAll = 6 * (radius + (radius * radius - radius) / 2) + 1;
            int countCore = countAll - radius * 6;
            int countBorder = countAll - countCore;

            HexData[] all = new HexData[countAll];
            HexData[] core = new HexData[countCore];
            HexData[] border = new HexData[countBorder];
            HexData[] corners = new HexData[6];

            int iAll = 0;
            int iCore = 0;
            int iBorder = 0;
            int iCorner = 0;

            for (int dX = -radius; dX <= radius; dX++)
            {
                int rows = 2 * radius + 1 - Math.Abs(dX);
                int yMin = -(int)Math.Floor(rows / 2d);
                int yMax = (int)Math.Floor((rows - 1) / 2d);
                bool isEdge = dX == -radius || dX == radius;

                for (int dY = yMin; dY <= yMax; dY++)
                {
                    HexData hex = new HexData(centre.X + dX, centre.Y + dY, Chunk);
                    all[iAll] = hex;
                    iAll++;

                    if (isEdge || dY == yMin || dY == yMax)
                    {
                        hex.IsBorder = true;
                        border[iBorder] = hex;
                        iBorder++;
                    }
                    else
                    {
                        hex.IsCore = true;
                        core[iCore] = hex;
                        iCore++;
                    }

                    if ((isEdge || dX == 0) && (dY == yMin || dY == yMax))
                    {
                        hex.IsCorner = true;
                        corners[iCorner] = hex;
                        iCorner++;
                    }
                }
            }

            Centre = centre;
            All = all;
            Core = core;
            Border = border;
            Corners = corners;
            Radius = radius;
            Chunk = chunk;
        }

        public ChunkData OffsetCopy(int colShift, int rowShift)
        {
            HexData[] all = new HexData[All.Length];
            HexData[] core = new HexData[Core.Length];
            HexData[] border = new HexData[Border.Length];
            HexData[] corners = new HexData[Corners.Length];

            int yStep = (int)Math.Floor(Radius * 1.5d);
            int dY = rowShift * yStep * 2 + (colShift & 1) * yStep;
            int dX = colShift * Radius;
            Point newChunk = Chunk.Offset(colShift, rowShift);

            for (int i = 0; i < All.Length; i++)
            {
                all[i] = All[i].OffsetCopy(dX, dY, newChunk);
            }
            for (int i = 0; i < Core.Length; i++)
            {
                core[i] = Core[i].OffsetCopy(dX, dY, newChunk);
            }
            for (int i = 0; i < Border.Length; i++)
            {
                border[i] = Border[i].OffsetCopy(dX, dY, newChunk);
            }
            for (int i = 0; i < Corners.Length; i++)
            {
                corners[i] = Corners[i].OffsetCopy(dX, dY, newChunk);
            }

            return new ChunkData(Centre.Offset(dX, dY), all, core, 
                border, corners, Radius, newChunk);
        }
    }

    public class HexData
    {
        public Point Pos;
        public PointF RealPos;
        public Point Chunk;
        public bool IsCore = false;
        public bool IsBorder = false;
        public bool IsCorner = false;

        public HexData(int x, int y, Point chunk)
        {
            Pos = new Point(x, y);
            RealPos = new PointF(x * XStepFlat, y + (x & 1) * YStepFlat);
            Chunk = chunk;
        }

        public HexData OffsetCopy(int dX, int dY, Point chunk)
        {
            int x = Pos.X + dX;
            int y = Pos.Y + dY;
            return new HexData(x, y, chunk)
            {
                RealPos = new PointF(x * XStepFlat, y + (x & 1) * YStepFlat),
                IsCore = IsCore,
                IsBorder = IsBorder,
                IsCorner = IsCorner
            };
        }
    }
}

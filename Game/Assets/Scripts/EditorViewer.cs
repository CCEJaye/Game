using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static PositionalMetaData;

public class EditorViewer : MonoBehaviour
{
    public GameObject Hex;
    public int Seed;
    public int Width;
    public int Height;
    public int Octaves;
    public float Persistance;
    public float Lacunarity;
    public float Scale;
    public int CurrentOctave;
    public bool Combined;
    public Renderer Renderer;
    public bool Colour;
    public TerrainRange[] Ranges;
    public MeshFilter MeshFilter;
    public MeshRenderer MeshRenderer;
    public Texture2D Texture;

    private List<GameObject> EditorObjects = new List<GameObject>();

    public void Start()
    {
        GenerateHexes();
    }

    public NoiseGenerator.NoiseSettings GetSettings() 
    {
        return new NoiseGenerator.NoiseSettings()
        {
            Seed = Seed,
            Width = Width,
            Height = Height,
            Octaves = Octaves,
            Persistance = Persistance,
            Lacunarity = Lacunarity,
            Scale = Scale
        };
    }

    public void ClearHexes()
    {
        foreach(GameObject hex in EditorObjects)
        {
            DestroyImmediate(hex);
        }
        EditorObjects.Clear();
    }

    public void Generate()
    {    
        NoiseGenerator.NoiseMap map = new NoiseGenerator.NoiseMap(GetSettings());
        Draw(Combined ? map.Combined : map.Individuals[CurrentOctave]);
    }

    public void GenerateHexes()
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;
        // smaller scale = zoom out
        HexNoise noise = new HexNoise(356736743, 4, 0.4f, 3f, 100f);
        Dictionary<Vector2Int, ChunkMeta> worldMeta = GetWorldMeta;
        foreach (KeyValuePair<Vector2Int, ChunkMeta> chunk in worldMeta)
        {
            foreach (KeyValuePair<Vector2Int, HexMeta> hex in chunk.Value.HexMeta)
            {
                /*GameObject obj = Instantiate(Hex, new Vector3(
                    hex.Value.RealPos.x, 0, hex.Value.RealPos.y), Quaternion.identity);
                EditorObjects.Add(obj);*/
                noise.StoreRawValues(hex.Value.RealPos);

                foreach (KeyValuePair<Vector2Int, VertexMeta> vert in hex.Value.VertexMeta)
                {
                    if (hex.Value.ParentChunk.RelPos == new Vector2Int(0, 0))
                    {
                        if (vert.Value.RealPos.x < minX) minX = vert.Value.RealPos.x;
                        if (vert.Value.RealPos.x > maxX) maxX = vert.Value.RealPos.x;
                        if (vert.Value.RealPos.y < minY) minY = vert.Value.RealPos.y;
                        if (vert.Value.RealPos.y > maxY) maxY = vert.Value.RealPos.y;
                    }
                }
            }
        }
        noise.NormaliseAll();

        List<Vector3> allVertices = new List<Vector3>();
        List<int> allTriangles = new List<int>();
        List<Vector2> allUVs = new List<Vector2>();
        int vertexCount = 0;
        int triangleCount = 0;

        for (int dX = 0; dX < 2; dX++)
        {
            for (int dY = 0; dY < 3; dY++)
            {
                ChunkMeta chunk = worldMeta[new Vector2Int(dX, dY)];
                List<Vector3> chunkVertices = new List<Vector3>();
                List<int> chunkTriangles = new List<int>();
                foreach (KeyValuePair<Vector2Int, HexMeta> h in chunk.HexMeta)
                {
                    HexMeta hex = h.Value;
                    List<Vector3> hexVertices = new List<Vector3>();
                    List<int> hexTriangles = new List<int>();
                    foreach (Vector3Int triangle in HexTriangleLookup)
                    {
                        hexTriangles.Add(triangle.x + vertexCount);
                        hexTriangles.Add(triangle.y + vertexCount);
                        hexTriangles.Add(triangle.z + vertexCount);
                        triangleCount += 3;
                    }
                    foreach (KeyValuePair<Vector2Int, VertexMeta> v in hex.VertexMeta)
                    {
                        VertexMeta vert = v.Value;
                        float y = 0f;
                        Vector2 sup1;
                        Vector2 sup2;
                        if (vert.IsFlat)
                        {
                            y = noise.ValueList[hex.RealPos].All;
                        }
                        else if (vert.IsCorner)
                        {
                            if (vert.IsNE)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.N)].RealPos;
                                sup2 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.NE)].RealPos;
                            }
                            else if (vert.IsE)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.NE)].RealPos;
                                sup2 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.SE)].RealPos;
                            }
                            else if (vert.IsSE)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.SE)].RealPos;
                                sup2 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.S)].RealPos;
                            }
                            else if (vert.IsSW)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.S)].RealPos;
                                sup2 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.SW)].RealPos;
                            }
                            else if (vert.IsW)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.SW)].RealPos;
                                sup2 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.NW)].RealPos;
                            }
                            else if (vert.IsNW)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.NW)].RealPos;
                                sup2 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.N)].RealPos;
                            }
                            else
                            {
                                throw new Exception();
                            }
                            if (sup1 == Vector2Int.zero && sup2 == Vector2Int.zero)
                            {
                                y = noise.ValueList[hex.RealPos].All;
                            }
                            else if (sup1 == Vector2Int.zero || sup2 == Vector2Int.zero)
                            {
                                y = (noise.ValueList[hex.RealPos].All
                                    + noise.ValueList[sup1].All
                                    + noise.ValueList[sup2].All) / 2f;
                            }
                            else
                            {
                                y = (noise.ValueList[hex.RealPos].All
                                    + noise.ValueList[sup1].All
                                    + noise.ValueList[sup2].All) / 3f;
                            }
                        }
                        else if (vert.IsSide)
                        {
                            if (vert.IsN)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.N)].RealPos;
                            }
                            else if (vert.IsNE)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.NE)].RealPos;
                            }
                            else if (vert.IsSE)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.SE)].RealPos;
                            }
                            else if (vert.IsS)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.S)].RealPos;
                            }
                            else if (vert.IsSW)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.SW)].RealPos;
                            }
                            else if (vert.IsNW)
                            {
                                sup1 = chunk.HexMeta[GetHexNeighbour(hex, HexDirection.NW)].RealPos;
                            }
                            else
                            {
                                throw new Exception();
                            }
                            if (sup1 == Vector2Int.zero)
                            {
                                y = noise.ValueList[hex.RealPos].All;
                            }
                            else
                            {
                                y = (noise.ValueList[hex.RealPos].All
                                    + noise.ValueList[sup1].All) / 2f;
                            }
                        }

                        hexVertices.Add(new Vector3(vert.RealPos.x, y * 120f, vert.RealPos.y));
                        allUVs.Add(new Vector2(vert.RealPos.x / maxX, vert.RealPos.y / maxY));
                        vertexCount++;
                    }
                    chunkVertices.AddRange(hexVertices);
                    chunkTriangles.AddRange(hexTriangles);
                }
                allVertices.AddRange(chunkVertices);
                allTriangles.AddRange(chunkTriangles);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = allVertices.ToArray();
        mesh.triangles = allTriangles.ToArray();
        mesh.uv = allUVs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter.sharedMesh = mesh;
        //MeshRenderer.sharedMaterial.mainTexture = Texture;

        /*HexRefMetaData metaData = new HexRefMetaData(Width, Height, 12);
        metaData.Populate();
        NoiseGenerator.NoiseMap map = new NoiseGenerator.NoiseMap(GetSettings());
        Structs.PointF[,] hexGrid = HexTools.GetSideTopArray(Width, Height);
        Modifiers.CurveModifier modifier = new Modifiers.CurveModifier(0.1f, 1f, Modifiers.CurveModifier.ModifyMode.Multiplication | Modifiers.CurveModifier.ModifyMode.Addition);
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                GameObject hex = Instantiate(Hex, new Vector3(hexGrid[x, y].X,
                    *//*map.Combined[x, y] * **//*14f, hexGrid[x, y].Y), Quaternion.identity);
                Hexes.Add(hex);
                MeshRenderer renderer = hex.GetComponentInChildren<MeshRenderer>();
                Color colour = Color.white;

                foreach (TerrainRange range in Ranges)
                {
                    if (modifier.GetModifiedValue(map.Combined[x, y]) >= range.Start)
                    {
                        colour = range.Color;
                        break;
                    }
                }

                renderer.material.color = colour;
            }
        }

        foreach (HexRefData chunk in metaData.HexChunks)
        {
            foreach (Structs.Point cell in chunk.Border)
            {
                GameObject hex = Instantiate(Hex, new Vector3(hexGrid[cell.X, cell.Y].X,
                    15f, hexGrid[cell.X, cell.Y].Y), Quaternion.identity);
                Hexes.Add(hex);
                MeshRenderer renderer = hex.GetComponentInChildren<MeshRenderer>();
                renderer.material.color = Color.red;
            }
            *//*foreach (Structs.Point cell in chunk.Core)
            {
                GameObject hex = Instantiate(Hex, new Vector3(hexGrid[cell.X, cell.Y].X,
                    map.Combined[cell.X, cell.Y] * 15f, hexGrid[cell.X, cell.Y].Y), Quaternion.identity);
                Hexes.Add(hex);
                MeshRenderer renderer = hex.GetComponentInChildren<MeshRenderer>();
                renderer.material.color = Color.blue;
            }*//*
        }*/
    }

    public void Draw(float[,] noise)
    {
        int width = noise.GetLength(0);
        int height = noise.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        Color[] colours = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Colour)
                {
                    foreach(TerrainRange range in Ranges)
                    {
                        if (noise[x, y] >= range.Start)
                        {
                            colours[y * width + x] = range.Color;
                            break;
                        }
                    }
                }
                else
                {
                    colours[y * width + x] = Color.Lerp(Color.black, Color.white, noise[x, y]);
                }
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colours);
        texture.Apply();

        Renderer.sharedMaterial.mainTexture = texture;
        Renderer.transform.localScale = new Vector3(width, 1, height);
    }

    public void PopulateRefData()
    {
        HexRefMetaData data = new HexRefMetaData(1333, 2616, 36);
        data.Populate();
    }

    [Serializable]
    public struct TerrainRange
    {
        public float Start;
        public Color Color;
    }

    public static Vector3Int[] HexTriangleLookup = new Vector3Int[24]
    {
        new Vector3Int(0, 3, 4), new Vector3Int(0, 4, 1), new Vector3Int(1, 4, 5), new Vector3Int(1, 5, 2),
        new Vector3Int(2, 5, 6), new Vector3Int(3, 7, 8), new Vector3Int(3, 8, 4), new Vector3Int(4, 8, 9),
        new Vector3Int(4, 9, 5), new Vector3Int(5, 9, 10), new Vector3Int(5, 10, 6), new Vector3Int(6, 10, 11),
        new Vector3Int(7, 12, 8), new Vector3Int(8, 12, 13), new Vector3Int(8, 13, 9), new Vector3Int(9, 13, 14),
        new Vector3Int(9, 14, 10), new Vector3Int(10, 14, 15), new Vector3Int(10, 15, 11), new Vector3Int(12, 16, 13),
        new Vector3Int(13, 16, 17), new Vector3Int(13, 17, 14), new Vector3Int(14, 17, 18), new Vector3Int(14, 18, 15)
    };

    public Vector2Int GetHexNeighbour(HexMeta hex, HexDirection direction)
    {
        Vector2Int offset = Vector2Int.zero;
        if (direction.Equals(HexDirection.N)) offset = GetHexOffsetN;
        if (direction.Equals(HexDirection.S)) offset = GetHexOffsetS;
        if ((hex.RelPos.x & 1) == 0)
        {
            if (direction.Equals(HexDirection.NE)) offset = GetHexOffsetNEEven;
            if (direction.Equals(HexDirection.SE)) offset = GetHexOffsetSEEven;
            if (direction.Equals(HexDirection.SW)) offset = GetHexOffsetSWEven;
            if (direction.Equals(HexDirection.NW)) offset = GetHexOffsetNWEven;
        }
        else
        {
            if (direction.Equals(HexDirection.NE)) offset = GetHexOffsetNEOdd;
            if (direction.Equals(HexDirection.SE)) offset = GetHexOffsetSEOdd;
            if (direction.Equals(HexDirection.SW)) offset = GetHexOffsetSWOdd;
            if (direction.Equals(HexDirection.NW)) offset = GetHexOffsetNWOdd;
        }
        if (!hex.ParentChunk.HexMeta.ContainsKey(hex.RelPos + offset))
        {
            return Vector2Int.zero;
        }
        else
        {
            return hex.RelPos + offset;
        }
    }

    public enum HexDirection
    {
        N, NE, SE, S, SW, NW
    }

    public static Vector2Int GetHexOffsetN = new Vector2Int(0, 1);
    public static Vector2Int GetHexOffsetS = new Vector2Int(0, -1);


    public static Vector2Int GetHexOffsetNEEven = new Vector2Int(1, 0);
    public static Vector2Int GetHexOffsetNEOdd = new Vector2Int(1, 1);

    public static Vector2Int GetHexOffsetSEEven = new Vector2Int(1, -1);
    public static Vector2Int GetHexOffsetSEOdd = new Vector2Int(1, 0);

    public static Vector2Int GetHexOffsetSWEven = new Vector2Int(-1, -1);
    public static Vector2Int GetHexOffsetSWOdd = new Vector2Int(-1, 0);

    public static Vector2Int GetHexOffsetNWEven = new Vector2Int(-1, 0);
    public static Vector2Int GetHexOffsetNWOdd = new Vector2Int(-1, 1);
}

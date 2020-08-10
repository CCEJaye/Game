using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Objects;

public class Data
{
    public const int Meridians = 6;
    public const int Parallels = 4;
    public const int ChunkRadius = 16;
    public static readonly float XStepHex = Mathf.Sqrt(3) / 2f;
    public static readonly float YStepHex = 1f;
    public static readonly float XStepChunk = ChunkRadius * Mathf.Sqrt(3);
    public static readonly float YStepChunk = (int)Math.Floor(ChunkRadius * 1.5d);
    public static readonly float XStepVector = Mathf.Sqrt(3) / 6f;
    public static readonly float YStepVector = 0.25f;

    public static readonly Vector2Int NullVector = new Vector2Int(int.MaxValue, int.MaxValue);
    public static readonly Vector2Int HexOffsetN = new Vector2Int(0, 1);
    public static readonly Vector2Int HexOffsetS = new Vector2Int(0, -1);
    public static readonly Vector2Int HexOffsetNEEven = new Vector2Int(1, 0);
    public static readonly Vector2Int HexOffsetNEOdd = new Vector2Int(1, 1);
    public static readonly Vector2Int HexOffsetSEEven = new Vector2Int(1, -1);
    public static readonly Vector2Int HexOffsetSEOdd = new Vector2Int(1, 0);
    public static readonly Vector2Int HexOffsetSWEven = new Vector2Int(-1, -1);
    public static readonly Vector2Int HexOffsetSWOdd = new Vector2Int(-1, 0);
    public static readonly Vector2Int HexOffsetNWEven = new Vector2Int(-1, 0);
    public static readonly Vector2Int HexOffsetNWOdd = new Vector2Int(-1, 1);

    private static VertexMeta Vertex0 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-1, -2),
        RealPos = new Vector2(-XStepVector, -2f * YStepVector),
        IsCorner = true,
        Position = HexDirection.SW,
        HexNeighbourA = HexDirection.S,
        HexNeighbourB = HexDirection.SW,
        RelUV = new Vector2(XStepVector, 0)
    };

    private static VertexMeta Vertex1 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, -2),
        RealPos = new Vector2(0f, -2f * YStepVector),
        IsSide = true,
        Position = HexDirection.S,
        HexNeighbourA = HexDirection.S,
        RelUV = new Vector2(2f * XStepVector, 0)
    };

    private static VertexMeta Vertex2 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(1, -2),
        RealPos = new Vector2(XStepVector, -2f * YStepVector),
        IsCorner = true,
        Position = HexDirection.SE,
        HexNeighbourA = HexDirection.SE,
        HexNeighbourB = HexDirection.S,
        RelUV = new Vector2(3f * XStepVector, 0f)
    };

    private static VertexMeta Vertex3 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-2, -1),
        RealPos = new Vector2(-2f * XStepVector + XStepVector / 2f, -YStepVector),
        IsSide = true,
        Position = HexDirection.SW,
        HexNeighbourA = HexDirection.SW,
        RelUV = new Vector2(XStepVector / 2f, YStepVector)
    };

    private static VertexMeta Vertex4 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-1, -1),
        RealPos = new Vector2(-XStepVector + XStepVector / 2f, -YStepVector),
        IsFlat = true,
        RelUV = new Vector2(XStepVector + XStepVector / 2f, YStepVector)
    };

    private static VertexMeta Vertex6 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, -1),
        RealPos = new Vector2(XStepVector / 2f, -YStepVector),
        IsFlat = true,
        RelUV = new Vector2(2f * XStepVector + XStepVector / 2f, YStepVector)
    };

    private static VertexMeta Vertex5 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 5),
        RealPos = Vector2.Lerp(Vertex4.RealPos, Vertex6.RealPos, 0.5f),
        IsFlat = true,
        RelUV = Vector2.Lerp(Vertex4.RelUV, Vertex6.RelUV, 0.5f)
    };

    private static VertexMeta Vertex7 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(1, -1),
        RealPos = new Vector2(1.5f * XStepVector, -YStepVector),
        IsSide = true,
        Position = HexDirection.SE,
        HexNeighbourA = HexDirection.SE,
        RelUV = new Vector2(3f * XStepVector + XStepVector / 2f, YStepVector)
    };

    private static VertexMeta Vertex10 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-2, 0),
        RealPos = new Vector2(-2f * XStepVector, 0f),
        IsCorner = true,
        Position = HexDirection.W,
        HexNeighbourA = HexDirection.SW,
        HexNeighbourB = HexDirection.NW,
        RelUV = new Vector2(0f, 2f * YStepVector)
    };

    private static VertexMeta Vertex11 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-1, 0),
        RealPos = new Vector2(-XStepVector, 0f),
        IsFlat = true,
        RelUV = new Vector2(XStepVector, 2f * YStepVector)
    };

    private static VertexMeta Vertex8 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 8),
        RealPos = Vector2.Lerp(Vertex4.RealPos, Vertex11.RealPos, 0.5f),
        IsFlat = true,
        RelUV = Vector2.Lerp(Vertex4.RelUV, Vertex11.RelUV, 0.5f)
    };

    private static VertexMeta Vertex12 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 0),
        RealPos = new Vector2(0f, 0f),
        IsFlat = true,
        IsCentre = true,
        RelUV = new Vector2(2f * XStepVector, 2f * YStepVector)
    };

    private static VertexMeta Vertex13 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(1, 0),
        RealPos = new Vector2(XStepVector, 0f),
        IsFlat = true,
        RelUV = new Vector2(3f * XStepVector, 2f * YStepVector)
    };

    private static VertexMeta Vertex9 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 9),
        RealPos = Vector2.Lerp(Vertex6.RealPos, Vertex13.RealPos, 0.5f),
        IsFlat = true,
        RelUV = Vector2.Lerp(Vertex6.RelUV, Vertex13.RelUV, 0.5f)
    };

    private static VertexMeta Vertex14 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(2, 0),
        RealPos = new Vector2(2f * XStepVector, 0f),
        IsCorner = true,
        Position = HexDirection.E,
        HexNeighbourA = HexDirection.NE,
        HexNeighbourB = HexDirection.SE,
        RelUV = new Vector2(4f * XStepVector, 2f * YStepVector)
    };

    private static VertexMeta Vertex17 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-2, 1),
        RealPos = new Vector2(-2f * XStepVector + XStepVector / 2f, YStepVector),
        IsSide = true,
        Position = HexDirection.NW,
        HexNeighbourA = HexDirection.NW,
        RelUV = new Vector2(XStepVector / 2f, 3f * YStepVector)
    };

    private static VertexMeta Vertex18 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-1, 1),
        RealPos = new Vector2(-XStepVector + XStepVector / 2f, YStepVector),
        IsFlat = true,
        RelUV = new Vector2(XStepVector + XStepVector / 2f, 3f * YStepVector)
    };

    private static VertexMeta Vertex15 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 15),
        RealPos = Vector2.Lerp(Vertex11.RealPos, Vertex18.RealPos, 0.5f),
        IsFlat = true,
        RelUV = Vector2.Lerp(Vertex11.RelUV, Vertex18.RelUV, 0.5f)
    };

    private static VertexMeta Vertex20 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 1),
        RealPos = new Vector2(XStepVector / 2f, YStepVector),
        IsFlat = true,
        RelUV = new Vector2(2f * XStepVector + XStepVector / 2f, 3f * YStepVector)
    };

    private static VertexMeta Vertex16 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 16),
        RealPos = Vector2.Lerp(Vertex13.RealPos, Vertex20.RealPos, 0.5f),
        IsFlat = true,
        RelUV = Vector2.Lerp(Vertex13.RelUV, Vertex20.RelUV, 0.5f)
    };

    private static VertexMeta Vertex19 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 19),
        RealPos = Vector2.Lerp(Vertex18.RealPos, Vertex20.RealPos, 0.5f),
        IsFlat = true,
        RelUV = Vector2.Lerp(Vertex18.RelUV, Vertex20.RelUV, 0.5f)
    };

    private static VertexMeta Vertex21 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(1, 1),
        RealPos = new Vector2(1.5f * XStepVector, YStepVector),
        IsSide = true,
        Position = HexDirection.NE,
        HexNeighbourA = HexDirection.NE,
        RelUV = new Vector2(3f * XStepVector + XStepVector / 2f, 3f * YStepVector)
    };

    private static VertexMeta Vertex22 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-1, 2),
        RealPos = new Vector2(-XStepVector, 2f * YStepVector),
        IsCorner = true,
        Position = HexDirection.NW,
        HexNeighbourA = HexDirection.NW,
        HexNeighbourB = HexDirection.N,
        RelUV = new Vector2(XStepVector, 4f * YStepVector)
    };

    private static VertexMeta Vertex23 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 2),
        RealPos = new Vector2(0f, 2f * YStepVector),
        IsSide = true,
        Position = HexDirection.N,
        HexNeighbourA = HexDirection.N,
        RelUV = new Vector2(2f * XStepVector, 4f * YStepVector)
    };

    private static VertexMeta Vertex24 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(1, 2),
        RealPos = new Vector2(XStepVector, 2f * YStepVector),
        IsCorner = true,
        Position = HexDirection.NE,
        HexNeighbourA = HexDirection.N,
        HexNeighbourB = HexDirection.NE,
        RelUV = new Vector2(3f * XStepVector, 4f * YStepVector)
    };

    public static VertexMeta[] CoreVertices { get; } = new VertexMeta[19]
    {
        Vertex0, Vertex1, Vertex2, Vertex3, Vertex4, Vertex6, Vertex7, Vertex10,
        Vertex11, Vertex12, Vertex13, Vertex14, Vertex17, Vertex18, Vertex20, Vertex21,
        Vertex22, Vertex23, Vertex24
    };

    public static VertexMeta[] SideNEVertices { get; } = new VertexMeta[13]
    {
        Vertex0, Vertex1, Vertex2, Vertex3, Vertex4, Vertex6, Vertex7, Vertex9,
        Vertex10, Vertex11, Vertex12, Vertex15, Vertex17
    };

    public static VertexMeta[] SideEVertices { get; } = new VertexMeta[13]
    {
        Vertex0, Vertex1, Vertex3, Vertex4, Vertex5, Vertex10, Vertex11, Vertex12,
        Vertex17, Vertex18, Vertex19, Vertex22, Vertex23
    };

    public static VertexMeta[] SideSEVertices { get; } = new VertexMeta[13]
    {
        Vertex3, Vertex8, Vertex10, Vertex11, Vertex12, Vertex16, Vertex17, Vertex18,
        Vertex20, Vertex21, Vertex22, Vertex23, Vertex24
    };

    public static VertexMeta[] SideSWVertices { get; } = new VertexMeta[13]
    {
        Vertex7, Vertex9, Vertex12, Vertex13, Vertex14, Vertex15, Vertex17, Vertex18,
        Vertex20, Vertex21, Vertex22, Vertex23, Vertex24
    };

    public static VertexMeta[] SideWVertices { get; } = new VertexMeta[13]
    {
        Vertex1, Vertex2, Vertex5, Vertex6, Vertex7, Vertex12, Vertex13, Vertex14,
        Vertex19, Vertex20, Vertex21, Vertex23, Vertex24
    };

    public static VertexMeta[] SideNWVertices { get; } = new VertexMeta[13]
    {
        Vertex0, Vertex1, Vertex2, Vertex3, Vertex4, Vertex6, Vertex7, Vertex8,
        Vertex12, Vertex13, Vertex14, Vertex16, Vertex21
    };

    public static VertexMeta[] CornerNVertices { get; } = new VertexMeta[10]
    {
        Vertex0, Vertex1, Vertex2, Vertex3, Vertex4, Vertex6, Vertex7, Vertex8,
        Vertex9, Vertex12
    };

    public static VertexMeta[] CornerNEVertices { get; } = new VertexMeta[10]
    {
        Vertex0, Vertex1, Vertex3, Vertex4, Vertex5, Vertex10, Vertex11, Vertex12,
        Vertex15, Vertex17
    };

    public static VertexMeta[] CornerSEVertices { get; } = new VertexMeta[10]
    {
        Vertex3, Vertex8, Vertex10, Vertex11, Vertex12, Vertex17, Vertex18, Vertex19,
        Vertex22, Vertex23
    };

    public static VertexMeta[] CornerSVertices { get; } = new VertexMeta[10]
    {
        Vertex12, Vertex15, Vertex16, Vertex17, Vertex18, Vertex20, Vertex21, Vertex22,
        Vertex23, Vertex24
    };

    public static VertexMeta[] CornerSWVertices { get; } = new VertexMeta[10]
    {
        Vertex7, Vertex9, Vertex12, Vertex13, Vertex14, Vertex19, Vertex20, Vertex21,
        Vertex23, Vertex24
    };

    public static VertexMeta[] CornerNWVertices { get; } = new VertexMeta[10]
    {
        Vertex1, Vertex2, Vertex5, Vertex6, Vertex7, Vertex12, Vertex13, Vertex14,
        Vertex16, Vertex21
    };

    public static int[] CoreTriangles = new int[72]
    {
        0, 3, 4, 0, 4, 1, 1, 4, 5, 1, 5, 2,
        2, 5, 6, 3, 7, 8, 3, 8, 4, 4, 8, 9,
        4, 9, 5, 5, 9, 10, 5, 10, 6, 6, 10, 11,
        7, 12, 8, 8, 12, 13, 8, 13, 9, 9, 13, 14,
        9, 14, 10, 10, 14, 15, 10, 15, 11, 12, 16, 13,
        13, 16, 17, 13, 17, 14, 14, 17, 18, 14, 18, 15
    };

    public static int[] SideNETriangles = new int[42]
    {
        0, 3, 4, 0, 4, 1, 1, 4, 5, 1, 5, 2,
        2, 5, 6, 3, 8, 9, 3, 9, 4, 4, 9, 10,
        4, 10, 5, 5, 7, 6, 5, 10, 7, 8, 12, 9,
        9, 12, 11, 9, 11, 10
    };

    public static int[] SideETriangles = new int[42]
    {
        0, 2, 3, 0, 3, 1, 1, 3, 4, 2, 5, 6,
        2, 6, 3, 3, 6, 7, 3, 7, 4, 5, 8, 6,
        6, 8, 9, 6, 9, 7, 7, 9, 10, 8, 11, 9,
        9, 11, 12, 9, 12, 10
    };

    public static int[] SideSETriangles = new int[42]
    {
        0, 2, 3, 0, 3, 1, 1, 3, 4, 2, 6, 3,
        3, 6, 7, 3, 7, 4, 4, 7, 8, 4, 8, 5,
        5, 8, 9, 6, 10, 7, 7, 10, 11, 7, 11, 8,
        8, 11, 12, 8, 12, 9
    };

    public static int[] SideSWTriangles = new int[42]
    {
        0, 1, 3, 0, 3, 4, 1, 2, 3, 2, 5, 7,
        2, 7, 8, 2, 8, 3, 3, 8, 9, 3, 9, 4,
        5, 6, 7, 6, 10, 7, 7, 10, 11, 7, 11, 8,
        8, 11, 12, 8, 12, 9
    };

    public static int[] SideWTriangles = new int[42]
    {
        0, 2, 3, 0, 3, 1, 1, 3, 4, 2, 5, 3,
        3, 5, 6, 3, 6, 4, 4, 6, 7, 5, 8, 9,
        5, 9, 6, 6, 9, 10, 6, 10, 7, 8, 11, 9,
        9, 11, 12, 9, 12, 10
    };

    public static int[] SideNWTriangles = new int[42]
    {
        0, 3, 4, 0, 4, 1, 1, 4, 5, 1, 5, 2,
        2, 5, 6, 3, 7, 4, 4, 7, 8, 4, 8, 5,
        5, 8, 9, 5, 9, 6, 6, 9, 10, 8, 11, 9,
        9, 11, 12, 9, 12, 10
    };

    public static int[] CornerNTriangles = new int[30]
    {
        0, 3, 4, 0, 4, 1, 1, 4, 5, 1, 5, 2,
        2, 5, 6, 3, 7, 4, 4, 7, 9, 4, 9, 5,
        5, 9, 8, 5, 8, 6
    };

    public static int[] CornerNETriangles = new int[30]
    {
        0, 2, 3, 0, 3, 1, 1, 3, 4, 2, 5, 6,
        2, 6, 3, 3, 6, 7, 3, 7, 4, 5, 9, 6,
        6, 9, 8, 6, 8, 7
    };

    public static int[] CornerSETriangles = new int[30]
    {
        0, 2, 3, 0, 3, 1, 1, 3, 4, 2, 5, 3,
        3, 5, 6, 3, 6, 4, 4, 6, 7, 5, 8, 6,
        6, 8, 9, 6, 9, 7
    };

    public static int[] CornerSTriangles = new int[30]
    {
        0, 1, 4, 0, 4, 5, 0, 5, 2, 1, 3, 4,
        2, 5, 6, 3, 7, 4, 4, 7, 8, 4, 8, 5,
        5, 8, 9, 5, 9, 6
    };

    public static int[] CornerSWTriangles = new int[30]
    {
        0, 1, 3, 0, 3, 4, 1, 2, 3, 2, 5, 6,
        2, 6, 3, 3, 6, 7, 3, 7, 4, 5, 8, 6,
        6, 8, 9, 6, 9, 7
    };

    public static int[] CornerNWTriangles = new int[30]
    {
        0, 2, 3, 0, 3, 1, 1, 3, 4, 2, 5, 3,
        3, 5, 6, 3, 6, 4, 4, 6, 7, 5, 8, 6,
        6, 8, 9, 6, 9, 7
    };
}

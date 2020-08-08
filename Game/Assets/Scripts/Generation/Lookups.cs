using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Data;

public class Lookups
{

    private static VertexMeta Vertex0 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-1, -2),
        RealPos = new Vector2(-XStepVector, -2f * YStepVector),
        IsCorner = true,
        Position = HexDirection.SW,
        HexNeighbourA = HexDirection.S,
        HexNeighbourB = HexDirection.SW
    };

    private static VertexMeta Vertex1 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, -2),
        RealPos = new Vector2(0f, -2f * YStepVector),
        IsSide = true,
        Position = HexDirection.S,
        HexNeighbourA = HexDirection.S
    };

    private static VertexMeta Vertex2 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(1, -2),
        RealPos = new Vector2(XStepVector, -2f * YStepVector),
        IsCorner = true,
        Position = HexDirection.SE,
        HexNeighbourA = HexDirection.SE,
        HexNeighbourB = HexDirection.S
    };

    private static VertexMeta Vertex3 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-2, -1),
        RealPos = new Vector2(-2f * XStepVector + XStepVector / 2f, -YStepVector),
        IsSide = true,
        Position = HexDirection.SW,
        HexNeighbourA = HexDirection.SW
    };

    private static VertexMeta Vertex4 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-1, -1),
        RealPos = new Vector2(-XStepVector + XStepVector / 2f, -YStepVector),
        IsFlat = true
    };

    private static VertexMeta Vertex6 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, -1),
        RealPos = new Vector2(XStepVector / 2f, -YStepVector),
        IsFlat = true
    };

    private static VertexMeta Vertex5 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 5),
        RealPos = Vector2.Lerp(Vertex4.RealPos, Vertex6.RealPos, 0.5f),
        IsFlat = true
    };

    private static VertexMeta Vertex7 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(1, -1),
        RealPos = new Vector2(1.5f * XStepVector, -YStepVector),
        IsSide = true,
        Position = HexDirection.SE,
        HexNeighbourA = HexDirection.SE
    };

    private static VertexMeta Vertex10 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-2, 0),
        RealPos = new Vector2(-2f * XStepVector, 0f),
        IsCorner = true,
        Position = HexDirection.W,
        HexNeighbourA = HexDirection.SW,
        HexNeighbourB = HexDirection.NW
    };

    private static VertexMeta Vertex11 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-1, 0),
        RealPos = new Vector2(-XStepVector, 0f),
        IsFlat = true
    };

    private static VertexMeta Vertex8 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 8),
        RealPos = Vector2.Lerp(Vertex4.RealPos, Vertex11.RealPos, 0.5f),
        IsFlat = true
    };

    private static VertexMeta Vertex12 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 0),
        RealPos = new Vector2(0f, 0f),
        IsFlat = true,
        IsCentre = true
    };

    private static VertexMeta Vertex13 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(1, 0),
        RealPos = new Vector2(XStepVector, 0f),
        IsFlat = true
    };

    private static VertexMeta Vertex9 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 9),
        RealPos = Vector2.Lerp(Vertex6.RealPos, Vertex13.RealPos, 0.5f),
        IsFlat = true
    };

    private static VertexMeta Vertex14 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(2, 0),
        RealPos = new Vector2(2f * XStepVector, 0f),
        IsCorner = true,
        Position = HexDirection.E,
        HexNeighbourA = HexDirection.NE,
        HexNeighbourB = HexDirection.SE
    };

    private static VertexMeta Vertex17 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-2, 1),
        RealPos = new Vector2(-2f * XStepVector + XStepVector / 2f, YStepVector),
        IsSide = true,
        Position = HexDirection.NW,
        HexNeighbourA = HexDirection.NW
    };

    private static VertexMeta Vertex18 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-1, 1),
        RealPos = new Vector2(-XStepVector + XStepVector / 2f, YStepVector),
        IsFlat = true
    };

    private static VertexMeta Vertex15 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 15),
        RealPos = Vector2.Lerp(Vertex11.RealPos, Vertex18.RealPos, 0.5f),
        IsFlat = true
    };

    private static VertexMeta Vertex20 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 1),
        RealPos = new Vector2(XStepVector / 2f, YStepVector),
        IsFlat = true
    };

    private static VertexMeta Vertex16 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 16),
        RealPos = Vector2.Lerp(Vertex13.RealPos, Vertex20.RealPos, 0.5f),
        IsFlat = true
    };

    private static VertexMeta Vertex19 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 19),
        RealPos = Vector2.Lerp(Vertex18.RealPos, Vertex20.RealPos, 0.5f),
        IsFlat = true
    };

    private static VertexMeta Vertex21 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(1, 1),
        RealPos = new Vector2(1.5f * XStepVector, YStepVector),
        IsSide = true,
        Position = HexDirection.NE,
        HexNeighbourA = HexDirection.NE
    };

    private static VertexMeta Vertex22 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(-1, 2),
        RealPos = new Vector2(-XStepVector, 2f * YStepVector),
        IsCorner = true,
        Position = HexDirection.NW,
        HexNeighbourA = HexDirection.NW,
        HexNeighbourB = HexDirection.N
    };

    private static VertexMeta Vertex23 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(0, 2),
        RealPos = new Vector2(0f, 2f * YStepVector),
        IsSide = true,
        Position = HexDirection.N,
        HexNeighbourA = HexDirection.N
    };

    private static VertexMeta Vertex24 { get; } = new VertexMeta()
    {
        RelPos = new Vector2Int(1, 2),
        RealPos = new Vector2(XStepVector, 2f * YStepVector),
        IsCorner = true,
        Position = HexDirection.NE,
        HexNeighbourA = HexDirection.N,
        HexNeighbourB = HexDirection.NE
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

    public static Vector3Int[] CoreTriangles = new Vector3Int[24]
    {
        new Vector3Int(0, 3, 4), new Vector3Int(0, 4, 1), new Vector3Int(1, 4, 5), new Vector3Int(1, 5, 2),
        new Vector3Int(2, 5, 6), new Vector3Int(3, 7, 8), new Vector3Int(3, 8, 4), new Vector3Int(4, 8, 9),
        new Vector3Int(4, 9, 5), new Vector3Int(5, 9, 10), new Vector3Int(5, 10, 6), new Vector3Int(6, 10, 11),
        new Vector3Int(7, 12, 8), new Vector3Int(8, 12, 13), new Vector3Int(8, 13, 9), new Vector3Int(9, 13, 14),
        new Vector3Int(9, 14, 10), new Vector3Int(10, 14, 15), new Vector3Int(10, 15, 11), new Vector3Int(12, 16, 13),
        new Vector3Int(13, 16, 17), new Vector3Int(13, 17, 14), new Vector3Int(14, 17, 18), new Vector3Int(14, 18, 15)
    };

    public static Vector3Int[] SideNETriangles = new Vector3Int[14]
    {
        new Vector3Int(0, 3, 4), new Vector3Int(0, 4, 1), new Vector3Int(1, 4, 5), new Vector3Int(1, 5, 2),
        new Vector3Int(2, 5, 6), new Vector3Int(3, 8, 9), new Vector3Int(3, 9, 4), new Vector3Int(4, 9, 10),
        new Vector3Int(4, 10, 5), new Vector3Int(5, 7, 6), new Vector3Int(5, 10, 7), new Vector3Int(8, 12, 9),
        new Vector3Int(9, 12, 11), new Vector3Int(9, 11, 10)
    };

    public static Vector3Int[] SideETriangles = new Vector3Int[14]
    {
        new Vector3Int(0, 2, 3), new Vector3Int(0, 3, 1), new Vector3Int(1, 3, 4), new Vector3Int(2, 5, 6),
        new Vector3Int(2, 6, 3), new Vector3Int(3, 6, 7), new Vector3Int(3, 7, 4), new Vector3Int(5, 8, 6),
        new Vector3Int(6, 8, 9), new Vector3Int(6, 9, 7), new Vector3Int(7, 9, 10), new Vector3Int(8, 11, 9),
        new Vector3Int(9, 11, 12), new Vector3Int(9, 12, 10)
    };

    public static Vector3Int[] SideSETriangles = new Vector3Int[14]
    {
        new Vector3Int(0, 2, 3), new Vector3Int(0, 3, 1), new Vector3Int(1, 3, 4), new Vector3Int(2, 6, 3),
        new Vector3Int(3, 6, 7), new Vector3Int(3, 7, 4), new Vector3Int(4, 7, 8), new Vector3Int(4, 8, 5),
        new Vector3Int(5, 8, 9), new Vector3Int(6, 10, 7), new Vector3Int(7, 10, 11), new Vector3Int(7, 11, 8),
        new Vector3Int(8, 11, 12), new Vector3Int(8, 12, 9)
    };

    public static Vector3Int[] SideSWTriangles = new Vector3Int[14]
    {
        new Vector3Int(0, 1, 3), new Vector3Int(0, 3, 4), new Vector3Int(1, 2, 3), new Vector3Int(2, 5, 7),
        new Vector3Int(2, 7, 8), new Vector3Int(2, 8, 3), new Vector3Int(3, 8, 9), new Vector3Int(3, 9, 4),
        new Vector3Int(5, 6, 7), new Vector3Int(6, 10, 7), new Vector3Int(7, 10, 11), new Vector3Int(7, 11, 8),
        new Vector3Int(8, 11, 12), new Vector3Int(8, 12, 9)
    };

    public static Vector3Int[] SideWTriangles = new Vector3Int[14]
    {
        new Vector3Int(0, 2, 3), new Vector3Int(0, 3, 1), new Vector3Int(1, 3, 4), new Vector3Int(2, 5, 3),
        new Vector3Int(3, 5, 6), new Vector3Int(3, 6, 4), new Vector3Int(4, 6, 7), new Vector3Int(5, 8, 9),
        new Vector3Int(5, 9, 6), new Vector3Int(6, 9, 10), new Vector3Int(6, 10, 7), new Vector3Int(8, 11, 9),
        new Vector3Int(9, 11, 12), new Vector3Int(9, 12, 10)
    };

    public static Vector3Int[] SideNWTriangles = new Vector3Int[14]
    {
        new Vector3Int(0, 3, 4), new Vector3Int(0, 4, 1), new Vector3Int(1, 4, 5), new Vector3Int(1, 5, 2),
        new Vector3Int(2, 5, 6), new Vector3Int(3, 7, 4), new Vector3Int(4, 7, 8), new Vector3Int(4, 8, 5),
        new Vector3Int(5, 8, 9), new Vector3Int(5, 9, 6), new Vector3Int(6, 9, 10), new Vector3Int(8, 11, 9),
        new Vector3Int(9, 11, 12), new Vector3Int(9, 12, 10)
    };

    public static Vector3Int[] CornerNTriangles = new Vector3Int[10]
    {
        new Vector3Int(0, 3, 4), new Vector3Int(0, 4, 1), new Vector3Int(1, 4, 5), new Vector3Int(1, 5, 2),
        new Vector3Int(2, 5, 6), new Vector3Int(3, 7, 4), new Vector3Int(4, 7, 9), new Vector3Int(4, 9, 5),
        new Vector3Int(5, 9, 8), new Vector3Int(5, 8, 6)
    };

    public static Vector3Int[] CornerNETriangles = new Vector3Int[10]
    {
        new Vector3Int(0, 2, 3), new Vector3Int(0, 3, 1), new Vector3Int(1, 3, 4), new Vector3Int(2, 5, 6),
        new Vector3Int(2, 6, 3), new Vector3Int(3, 6, 7), new Vector3Int(3, 7, 4), new Vector3Int(5, 9, 6),
        new Vector3Int(6, 9, 8), new Vector3Int(6, 8, 7)
    };

    public static Vector3Int[] CornerSETriangles = new Vector3Int[10]
    {
        new Vector3Int(0, 2, 3), new Vector3Int(0, 3, 1), new Vector3Int(1, 3, 4), new Vector3Int(2, 5, 3),
        new Vector3Int(3, 5, 6), new Vector3Int(3, 6, 4), new Vector3Int(4, 6, 7), new Vector3Int(5, 8, 6),
        new Vector3Int(6, 8, 9), new Vector3Int(6, 9, 7)
    };

    public static Vector3Int[] CornerSTriangles = new Vector3Int[10]
    {
        new Vector3Int(0, 1, 4), new Vector3Int(0, 4, 5), new Vector3Int(0, 5, 2), new Vector3Int(1, 3, 4),
        new Vector3Int(2, 5, 6), new Vector3Int(3, 7, 4), new Vector3Int(4, 7, 8), new Vector3Int(4, 8, 5),
        new Vector3Int(5, 8, 9), new Vector3Int(5, 9, 6)
    };

    public static Vector3Int[] CornerSWTriangles = new Vector3Int[10]
    {
        new Vector3Int(0, 1, 3), new Vector3Int(0, 3, 4), new Vector3Int(1, 2, 3), new Vector3Int(2, 5, 6),
        new Vector3Int(2, 6, 3), new Vector3Int(3, 6, 7), new Vector3Int(3, 7, 4), new Vector3Int(5, 8, 6),
        new Vector3Int(6, 8, 9), new Vector3Int(6, 9, 7)
    };

    public static Vector3Int[] CornerNWTriangles = new Vector3Int[10]
    {
        new Vector3Int(0, 2, 3), new Vector3Int(0, 3, 1), new Vector3Int(1, 3, 4), new Vector3Int(2, 5, 3),
        new Vector3Int(3, 5, 6), new Vector3Int(3, 6, 4), new Vector3Int(4, 6, 7), new Vector3Int(5, 8, 6),
        new Vector3Int(6, 8, 9), new Vector3Int(6, 9, 7)
    };
}

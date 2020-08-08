using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Data;
using static Lookups;

public static class PositionalMetaData
{
    // Real positions are ALWAYS absolute
    // Rel positions are hierarchical

    public static Dictionary<Vector2Int, HexMeta> LookupChunkHexes { get; } = GetChunkHexMetaLookup();
    public static Dictionary<Vector2Int, ChunkMeta> LookupWorldChunks { get; } = GetWorldChunkMetaLookup();

    public static Dictionary<Vector2Int, ChunkMeta> GetWorldMeta { get; }
        = GetWorldChunkMeta(new Vector2(
            (ChunkRadius * 2f + 1) * XStepHex / 2f + XStepHex / 6f, 
            ChunkRadius + YStepVector * 2f));

    public static Dictionary<Vector2Int, ChunkMeta> GetWorldChunkMeta(Vector2 realWorldPos)
    {
        Dictionary<Vector2Int, ChunkMeta> dict = new Dictionary<Vector2Int, ChunkMeta>();
        foreach (KeyValuePair<Vector2Int, ChunkMeta> chunkMeta in LookupWorldChunks)
        {
            ChunkMeta oldChunk = chunkMeta.Value;
            ChunkMeta newChunk = new ChunkMeta();
            newChunk.RelPos = oldChunk.RelPos;
            newChunk.RealPos = oldChunk.RealPos + realWorldPos;
            newChunk.HexMeta = GetChunkHexMeta(newChunk, newChunk.RealPos);
            newChunk.IsEdge = oldChunk.IsEdge;
            dict.Add(newChunk.RelPos, newChunk);
        }
        return dict;
    }

    public static Dictionary<Vector2Int, HexMeta> GetChunkHexMeta(ChunkMeta parentChunk, Vector2 realChunkPos)
    {
        Dictionary<Vector2Int, HexMeta> dict = new Dictionary<Vector2Int, HexMeta>();
        foreach (KeyValuePair<Vector2Int, HexMeta> hexMeta in LookupChunkHexes)
        {
            HexMeta newHex = hexMeta.Value.ChunkOffset(parentChunk, realChunkPos);
            newHex.VertexMeta = GetHexVertexMeta(newHex, newHex.RealPos);
            dict.Add(newHex.RelPos, newHex);
        }
        return dict;
    }

    public static Dictionary<Vector2Int, VertexMeta> GetHexVertexMeta(HexMeta parentHex, Vector2 realHexPos)
    {
        Dictionary<Vector2Int, VertexMeta> dict = new Dictionary<Vector2Int, VertexMeta>();
        foreach (VertexMeta vertexMeta in GetRelevantVertices(parentHex))
        {
            VertexMeta newVert = vertexMeta.HexOffset(parentHex, realHexPos);
            dict.Add(newVert.RelPos, newVert);
        }    
        return dict;
    }

    public static Dictionary<Vector2Int, ChunkMeta> GetWorldChunkMetaLookup()
    {
        Dictionary<Vector2Int, ChunkMeta> dict = new Dictionary<Vector2Int, ChunkMeta>();

        for (int dX = 0; dX < Meridians; dX++)
        {
            for (int dY = 0; dY < Parallels; dY++)
            {
                ChunkMeta chunk = new ChunkMeta();
                chunk.RelPos = new Vector2Int(dX, dY);
                chunk.RealPos = new Vector2(dX * XStepChunk + (dY & 1) * XStepChunk / 2f, dY * YStepChunk);
                chunk.IsEdge = dX == 0 || dX == Meridians - 1;
                dict.Add(chunk.RelPos, chunk);
            }
        }
        return dict;
    }

    public static Dictionary<Vector2Int, HexMeta> GetChunkHexMetaLookup()
    {
        Dictionary<Vector2Int, HexMeta> dict = new Dictionary<Vector2Int, HexMeta>();

        for (int dX = -ChunkRadius; dX <= ChunkRadius; dX++)
        {
            int rows = 2 * ChunkRadius + 1 - Math.Abs(dX);
            int yMin = -(int)Math.Floor(rows / 2d);
            int yMax = (int)Math.Floor((rows - 1) / 2d);
            bool isEdge = dX == -ChunkRadius || dX == ChunkRadius;
            for (int dY = yMin; dY <= yMax; dY++)
            {
                HexMeta hex = new HexMeta();
                hex.RelPos = new Vector2Int(dX, dY);
                hex.RealPos = new Vector2(dX * XStepHex, dY * YStepHex + (dX & 1) * YStepHex / 2f);
                if (dX == 0 && dY == 0) hex.IsCentre = true;
                if (isEdge || dY == yMin || dY == yMax)
                {
                    if ((isEdge || dX == 0) && (dY == yMin || dY == yMax)) hex.IsCorner = true;
                    else hex.IsSide = true;
                }
                else hex.IsCore = true;
                if (hex.IsSide)
                {
                    if (isEdge)
                    {
                        if (dX < 0) hex.Position = HexDirection.W;
                        else hex.Position = HexDirection.E;
                    }
                    else if (dY == yMin)
                    {
                        if (dX < 0) hex.Position = HexDirection.SW;
                        else hex.Position = HexDirection.SE;
                    }
                    else if (dY == yMax)
                    {
                        if (dX < 0) hex.Position = HexDirection.NW;
                        else hex.Position = HexDirection.NE;
                    }
                }
                else if (hex.IsCorner)
                {
                    if (dX == 0)
                    {
                        if (dY == yMin) hex.Position = HexDirection.S;
                        else hex.Position = HexDirection.N;
                    }
                    else if (dX < 0)
                    {
                        if (dY == yMin) hex.Position = HexDirection.SW;
                        else hex.Position = HexDirection.NW;
                    }
                    else if (dX > 0)
                    {
                        if (dY == yMin) hex.Position = HexDirection.SE;
                        else hex.Position = HexDirection.NE;
                    }
                }
                hex.Triangles = GetRelevantTriangles(hex);
                dict.Add(hex.RelPos, hex);
            }
        }
        return dict;
    }

    private static VertexMeta[] GetRelevantVertices(HexMeta parentHex)
    {
        if (parentHex.IsCore) return CoreVertices;
        if (parentHex.IsSide)
        {
            if (parentHex.Position.Equals(HexDirection.NE)) return SideNEVertices;
            if (parentHex.Position.Equals(HexDirection.E)) return SideEVertices;
            if (parentHex.Position.Equals(HexDirection.SE)) return SideSEVertices;
            if (parentHex.Position.Equals(HexDirection.SW)) return SideSWVertices;
            if (parentHex.Position.Equals(HexDirection.W)) return SideWVertices;
            if (parentHex.Position.Equals(HexDirection.NW)) return SideNWVertices;
        }
        else if (parentHex.IsCorner)
        {
            if (parentHex.Position.Equals(HexDirection.N)) return CornerNVertices;
            if (parentHex.Position.Equals(HexDirection.NE)) return CornerNEVertices;
            if (parentHex.Position.Equals(HexDirection.SE)) return CornerSEVertices;
            if (parentHex.Position.Equals(HexDirection.S)) return CornerSVertices;
            if (parentHex.Position.Equals(HexDirection.SW)) return CornerSWVertices;
            if (parentHex.Position.Equals(HexDirection.NW)) return CornerNWVertices;
        }
        throw new Exception();
    }

    private static Vector3Int[] GetRelevantTriangles(HexMeta parentHex)
    {
        if (parentHex.IsCore) return CoreTriangles;
        if (parentHex.IsSide)
        {
            if (parentHex.Position.Equals(HexDirection.NE)) return SideNETriangles;
            if (parentHex.Position.Equals(HexDirection.E)) return SideETriangles;
            if (parentHex.Position.Equals(HexDirection.SE)) return SideSETriangles;
            if (parentHex.Position.Equals(HexDirection.SW)) return SideSWTriangles;
            if (parentHex.Position.Equals(HexDirection.W)) return SideWTriangles;
            if (parentHex.Position.Equals(HexDirection.NW)) return SideNWTriangles;
        }
        else if (parentHex.IsCorner)
        {
            if (parentHex.Position.Equals(HexDirection.N)) return CornerNTriangles;
            if (parentHex.Position.Equals(HexDirection.NE)) return CornerNETriangles;
            if (parentHex.Position.Equals(HexDirection.SE)) return CornerSETriangles;
            if (parentHex.Position.Equals(HexDirection.S)) return CornerSTriangles;
            if (parentHex.Position.Equals(HexDirection.SW)) return CornerSWTriangles;
            if (parentHex.Position.Equals(HexDirection.NW)) return CornerNWTriangles;
        }
        throw new Exception();
    }
}

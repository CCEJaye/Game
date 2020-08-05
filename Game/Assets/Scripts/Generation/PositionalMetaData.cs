using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalMetaData
{
    // Real positions are ALWAYS absolute
    // Rel positions are hierarchical


    public const int Meridians = 6;
    public const int Parallels = 4;
    public const int ChunkRadius = 12;
    public static readonly float XStepHex = Mathf.Sqrt(3) / 2f * 10f;
    public static readonly float YStepHex = 1f * 10f;
    public static readonly float XStepChunk = ChunkRadius * Mathf.Sqrt(3) * 10f;
    public static readonly float YStepChunk = (int)Math.Floor(ChunkRadius * 1.5d) * 10f;
    public static readonly float XStepVector = Mathf.Sqrt(3) / 6f * 10f;
    public static readonly float YStepVector = 0.25f * 10f;

    public static Dictionary<Vector2Int, VertexMeta> LookupHexVertices { get; } = GetHexVertexMetaLookup();
    public static Dictionary<Vector2Int, HexMeta> LookupChunkHexes { get; } = GetChunkHexMetaLookup();
    public static Dictionary<Vector2Int, ChunkMeta> LookupWorldChunks { get; } = GetWorldChunkMetaLookup();

    public static Dictionary<Vector2Int, ChunkMeta> GetWorldMeta { get; }
        = GetWorldChunkMeta(new Vector2((ChunkRadius * 2f + 1) * XStepHex / 2f 
            + XStepHex / 6f, ChunkRadius + YStepVector * 2f));

    public class ChunkMeta
    {
        public Vector2Int RelPos;
        public Vector2 RealPos;
        public Dictionary<Vector2Int, HexMeta> HexMeta;
        public bool IsEdge = false;
    }

    public class HexMeta
    {
        public ChunkMeta ParentChunk;
        public Vector2Int RelPos;
        public Vector2 RealPos;
        public Dictionary<Vector2Int, VertexMeta> VertexMeta;
        public bool IsCentre = false;
        public bool IsCorner = false;
        public bool IsSide = false;
        public bool IsCore = false;
    }

    public class VertexMeta
    {
        public HexMeta ParentHex;
        public Vector2Int RelPos;
        public Vector2 RealPos;
        public bool IsCentre = false;
        public bool IsCorner = false;
        public bool IsSide = false;
        public bool IsFlat = false;
        public bool IsN = false;
        public bool IsNE = false;
        public bool IsE = false;
        public bool IsSE = false;
        public bool IsS = false;
        public bool IsSW = false;
        public bool IsW = false;
        public bool IsNW = false;
    }

    public static Dictionary<Vector2Int, VertexMeta> GetHexVertexMetaLookup()
    {
        Dictionary<Vector2Int, VertexMeta> dict = new Dictionary<Vector2Int, VertexMeta>();
        for (int dY = -2; dY <= 2; dY++)
        {
            int columns = 5 - Math.Abs(dY);
            int xMin = -(int)Math.Floor(columns / 2d);
            int xMax = (int)Math.Floor((columns - 1) / 2d);
            bool isEdge = dY == -2 || dY == 2;

            for (int dX = xMin; dX <= xMax; dX++)
            {
                if ((dX == -2 && Math.Abs(dY) == 2) || (dX > 2 && Math.Abs(dY) > 0)) continue;
                VertexMeta vertex = new VertexMeta();
                vertex.RelPos = new Vector2Int(dX, dY);
                vertex.RealPos = new Vector2(dX * XStepVector + (dY & 1) * XStepVector / 2f, dY * YStepVector);
                if (vertex.RealPos.x > 0f && vertex.RealPos.x < 0.0001f) vertex.RealPos.x = 0f;
                if (dX == 0 && dY == 0) vertex.IsCentre = true;
                if (isEdge || dX == xMin || dX == xMax)
                {
                    if (dX == 0 || (dY & 1) == 1)
                    {
                        vertex.IsSide = true;
                    }
                    else
                    {
                        vertex.IsCorner = true;
                    }
                }
                else
                {
                    vertex.IsFlat = true;
                }
                if (vertex.IsCorner)
                {
                    if (dX == -2) vertex.IsW = true;
                    if (dX == 2) vertex.IsE = true;
                    if (dY > 0)
                    {
                        if (dX == -1) vertex.IsNW = true;
                        if (dX == 1) vertex.IsNE = true;
                    }
                    else
                    {
                        if (dX == -1) vertex.IsSW = true;
                        if (dX == 1) vertex.IsSE = true;
                    }
                }
                else if (vertex.IsSide)
                {
                    if (dY == -2) vertex.IsS = true;
                    if (dY == 2) vertex.IsN = true;
                    if (dX > 0)
                    {
                        if (dY == -1) vertex.IsSE = true;
                        if (dY == 1) vertex.IsNE = true;
                    }
                    else
                    {
                        if (dY == -1) vertex.IsSW = true;
                        if (dY == 1) vertex.IsNW = true;
                    }
                }
                dict.Add(vertex.RelPos, vertex);
            }
        }
        return dict;
    }

    public static Dictionary<Vector2Int, VertexMeta> GetHexVertexMeta(HexMeta parentHex, Vector2 realHexPos)
    {
        Dictionary<Vector2Int, VertexMeta> dict = new Dictionary<Vector2Int, VertexMeta>();
        foreach (KeyValuePair<Vector2Int, VertexMeta> vertexMeta in LookupHexVertices)
        {      
            VertexMeta oldVert = vertexMeta.Value;
            VertexMeta newVert = new VertexMeta();
            newVert.ParentHex = parentHex;
            newVert.RelPos = oldVert.RelPos;
            newVert.RealPos = oldVert.RealPos + realHexPos;
            newVert.IsCentre = oldVert.IsCentre;
            newVert.IsCorner = oldVert.IsCorner;
            newVert.IsSide = oldVert.IsSide;
            newVert.IsFlat = oldVert.IsFlat;
            newVert.IsN = oldVert.IsN;
            newVert.IsNE = oldVert.IsNE;
            newVert.IsE = oldVert.IsE;
            newVert.IsSE = oldVert.IsSE;
            newVert.IsS = oldVert.IsS;
            newVert.IsSW = oldVert.IsSW;
            newVert.IsW = oldVert.IsW;
            newVert.IsNW = oldVert.IsNW;
            dict.Add(newVert.RelPos, newVert);
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
                dict.Add(hex.RelPos, hex);
            }
        }
        return dict;
    }

    public static Dictionary<Vector2Int, HexMeta> GetChunkHexMeta(ChunkMeta parentChunk, Vector2 realChunkPos)
    {
        Dictionary<Vector2Int, HexMeta> dict = new Dictionary<Vector2Int, HexMeta>();
        foreach (KeyValuePair<Vector2Int, HexMeta> hexMeta in LookupChunkHexes)
        {
            HexMeta oldHex = hexMeta.Value;
            HexMeta newHex = new HexMeta();
            newHex.ParentChunk = parentChunk;
            newHex.RelPos = oldHex.RelPos;
            newHex.RealPos = oldHex.RealPos + realChunkPos;
            newHex.VertexMeta = GetHexVertexMeta(newHex, newHex.RealPos);
            newHex.IsCentre = oldHex.IsCentre;
            newHex.IsCorner = oldHex.IsCorner;
            newHex.IsSide = oldHex.IsSide;
            newHex.IsCore = oldHex.IsCore;
            dict.Add(newHex.RelPos, newHex);
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
                chunk.IsEdge = dX == 0 | dX == Meridians - 1;
                dict.Add(chunk.RelPos, chunk);
            }
        }
        return dict;
    }

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
}

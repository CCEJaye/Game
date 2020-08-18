using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Generation;

public class Objects
{
    public enum HexDirection
    {
        None, N, NE, E, SE, S, SW, W, NW
    }

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
        public int[] Triangles;
        public bool IsCentre = false;
        public bool IsCorner = false;
        public bool IsSide = false;
        public bool IsCore = false;
        public HexDirection Position;

        public HexMeta ChunkOffset(ChunkMeta parentChunk, Vector2 realChunkPos)
        {
            return new HexMeta()
            {
                ParentChunk = parentChunk,
                RelPos = new Vector2Int(RelPos.x, RelPos.y),
                RealPos = RealPos + realChunkPos,
                Triangles = Triangles,
                IsCentre = IsCentre,
                IsCorner = IsCorner,
                IsSide = IsSide,
                IsCore = IsCore,
                Position = Position
            };
        }
    }

    public class VertexMeta
    {
        public HexMeta ParentHex;
        public Vector2Int RelPos;
        public Vector2 RealPos;
        public HexDirection HexNeighbourA;
        public HexDirection HexNeighbourB;
        public bool IsCentre = false;
        public bool IsCorner = false;
        public bool IsSide = false;
        public bool IsFlat = false;
        public HexDirection Position;
        public Vector2 RelUV;

        public VertexMeta HexOffset(HexMeta parentHex, Vector2 realHexPos)
        {
            return new VertexMeta()
            {
                ParentHex = parentHex,
                RelPos = new Vector2Int(RelPos.x, RelPos.y),
                RealPos = RealPos + realHexPos,
                HexNeighbourA = HexNeighbourA,
                HexNeighbourB = HexNeighbourB,
                IsCentre = IsCentre,
                IsCorner = IsCorner,
                IsSide = IsSide,
                IsFlat = IsFlat,
                Position = Position,
                RelUV = new Vector2(RelUV.x, RelUV.y)
            };
        }
    }
}

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Data
{

    public const int Meridians = 6;
    public const int Parallels = 4;
    public const int ChunkRadius = 12;
    public static readonly float XStepHex = Mathf.Sqrt(3) / 2f * 10f;
    public static readonly float YStepHex = 1f * 10f;
    public static readonly float XStepChunk = ChunkRadius * Mathf.Sqrt(3) * 10f;
    public static readonly float YStepChunk = (int)Math.Floor(ChunkRadius * 1.5d) * 10f;
    public static readonly float XStepVector = Mathf.Sqrt(3) / 6f * 10f;
    public static readonly float YStepVector = 0.25f * 10f;
    public static Vector2Int NullVector = new Vector2Int(int.MaxValue, int.MaxValue);

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

    public static Vector2Int HexOffsetN = new Vector2Int(0, 1);
    public static Vector2Int HexOffsetS = new Vector2Int(0, -1);
    public static Vector2Int HexOffsetNEEven = new Vector2Int(1, 0);
    public static Vector2Int HexOffsetNEOdd = new Vector2Int(1, 1);
    public static Vector2Int HexOffsetSEEven = new Vector2Int(1, -1);
    public static Vector2Int HexOffsetSEOdd = new Vector2Int(1, 0);
    public static Vector2Int HexOffsetSWEven = new Vector2Int(-1, -1);
    public static Vector2Int HexOffsetSWOdd = new Vector2Int(-1, 0);
    public static Vector2Int HexOffsetNWEven = new Vector2Int(-1, 0);
    public static Vector2Int HexOffsetNWOdd = new Vector2Int(-1, 1);

    public class HexMeta
    {
        public ChunkMeta ParentChunk;
        public Vector2Int RelPos;
        public Vector2 RealPos;
        public Dictionary<Vector2Int, VertexMeta> VertexMeta;
        public Vector3Int[] Triangles;
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

        public Vector2Int GetNeighbourRelOffset(HexDirection direction)
        {
            Vector2Int offset = NullVector;
            if (direction.Equals(HexDirection.N)) offset = HexOffsetN;
            if (direction.Equals(HexDirection.S)) offset = HexOffsetS;
            if ((RelPos.x & 1) == 0)
            {
                if (direction.Equals(HexDirection.NE)) offset = HexOffsetNEEven;
                if (direction.Equals(HexDirection.SE)) offset = HexOffsetSEEven;
                if (direction.Equals(HexDirection.SW)) offset = HexOffsetSWEven;
                if (direction.Equals(HexDirection.NW)) offset = HexOffsetNWEven;
            }
            else
            {
                if (direction.Equals(HexDirection.NE)) offset = HexOffsetNEOdd;
                if (direction.Equals(HexDirection.SE)) offset = HexOffsetSEOdd;
                if (direction.Equals(HexDirection.SW)) offset = HexOffsetSWOdd;
                if (direction.Equals(HexDirection.NW)) offset = HexOffsetNWOdd;
            }
            return offset;
        }

        public Vector2 GetNeighbourRealPos(HexDirection vertexPosition)
        {
            Vector2Int offset = GetNeighbourRelOffset(vertexPosition);
            if (offset == NullVector) return NullVector;
            return ParentChunk.HexMeta[RelPos + offset].RealPos;
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

        public VertexMeta Copy()
        {
            return new VertexMeta()
            {
                ParentHex = ParentHex,
                RelPos = new Vector2Int(RelPos.x, RelPos.y),
                RealPos = new Vector2(RealPos.x, RealPos.y),
                HexNeighbourA = HexNeighbourA,
                HexNeighbourB = HexNeighbourB,
                IsCentre = IsCentre,
                IsCorner = IsCorner,
                IsSide = IsSide,
                IsFlat = IsFlat,
                Position = Position
            };
        }

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
                Position = Position
            };
        }
    }
}

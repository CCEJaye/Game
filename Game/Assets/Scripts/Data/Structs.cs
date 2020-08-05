using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Worlds.WorldCollection;

public static class Structs
{

    public struct Range
    {
        private float Start { get; }
        private float End { get; }

        public Range(float start, float end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(float value)
        {
            return value == Start || (value >= Start && value < End);
        }
    }

    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point Offset(int dX, int dY)
        {
            return new Point(X + dX, Y + dY);
        }
    }

    public struct PointF
    {
        public float X;
        public float Y;

        public PointF(float x, float y)
        {
            X = x;
            Y = y;
        }

        public PointF Offset(float dX, float dY)
        {
            return new PointF(X + dX, Y + dY);
        }
    }
}

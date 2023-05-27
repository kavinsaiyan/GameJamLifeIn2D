using System;
using Microsoft.Xna.Framework;

namespace SimplePhysics
{
    public static class MathHelper
    {
        public const float VERY_SMALL_AMOUNT = 0.005f;
        public static float Clamp(float value, float min, float max)
        {
            if (min == max)
                return min;

            if (min > max)
                throw new ArgumentOutOfRangeException("Min is greater than max.");

            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }

        public static float Length(Vector2 v)
        {
            return MathF.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;
            return MathF.Sqrt(dx * dx + dy * dy);
        }
        public static float LengthSquared(Vector2 v)
        {
            return (v.X * v.X + v.Y * v.Y);
        }

        public static float DistanceSquared(Vector2 a, Vector2 b)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;
            return (dx * dx + dy * dy);
        }

        public static Vector2 Normalize(Vector2 v)
        {
            float length = Length(v);
            return new Vector2(v.X / length, v.Y / length);
        }

        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static float Cross(Vector2 a, Vector2 b)
        {
            //ax * by - ay*bx;
            return a.X * b.Y - a.Y * b.X;
        }

        public static bool NearlyEqual(float a, float b)
        {
            return MathF.Abs(a - b) < VERY_SMALL_AMOUNT;
        }

        public static bool NearlyEqual(Vector2 a, Vector2 b)
        {
            return DistanceSquared(a, b) < VERY_SMALL_AMOUNT * VERY_SMALL_AMOUNT;
        }
    }
}
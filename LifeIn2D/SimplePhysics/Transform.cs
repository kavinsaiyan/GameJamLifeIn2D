using Microsoft.Xna.Framework;

namespace SimplePhysics
{
    public struct Transform
    {
        public readonly float PositionX;
        public readonly float PositionY;
        public readonly float Cos;
        public readonly float Sin;
        public static readonly Transform Zero = new Transform(0, 0, 0);
        public Transform(Vector2 pos, float angle)
        {
            Sin = (float)System.Math.Sin(angle);
            Cos = (float)System.Math.Cos(angle);
            PositionX = pos.X;
            PositionY = pos.Y;
        }

        public Transform(float x, float y, float angle)
        {
            Sin = (float)System.Math.Sin(angle);
            Cos = (float)System.Math.Cos(angle);
            PositionX = x;
            PositionY = y;
        }
    }
}
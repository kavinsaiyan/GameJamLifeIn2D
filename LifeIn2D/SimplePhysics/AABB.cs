using Microsoft.Xna.Framework;

namespace SimplePhysics
{
    public readonly struct AABB
    {
        public readonly Vector2 Min;
        public readonly Vector2 Max;
        public AABB(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }
        public AABB(float minX, float minY, float maxX, float maxY)
        {
            Min = new Vector2(minX, minY);
            Max = new Vector2(maxX, maxY);
        }
    }
}
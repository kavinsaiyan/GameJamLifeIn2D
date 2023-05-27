using Microsoft.Xna.Framework;
namespace SimplePhysics
{
    public readonly struct CollisionManifold
    {
        public readonly Rigidbody BodyA;
        public readonly Rigidbody BodyB;
        public readonly Vector2 Normal;
        public readonly float Depth;
        public readonly Vector2 Contact1;
        public readonly Vector2 Contact2;
        public readonly int ContactCount;

        public CollisionManifold(Rigidbody bodyA, Rigidbody bodyB, Vector2 normal, float depth, Vector2 contact1, Vector2 contact2, int contactCount) : this()
        {
            this.BodyA = bodyA;
            this.BodyB = bodyB;
            this.Normal = normal;
            this.Depth = depth;
            this.Contact1 = contact1;
            this.Contact2 = contact2;
            this.ContactCount = contactCount;
        }
    }
}
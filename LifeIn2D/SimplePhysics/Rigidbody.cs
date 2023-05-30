using System;
using Microsoft.Xna.Framework;

namespace SimplePhysics
{
    public enum ShapeType
    {
        Circle = 0,
        Box = 1,
    }
    public class Rigidbody
    {
        private Vector2 position;
        private Vector2 linearVelocity;
        private float angle;
        public float Angle { get => angle; }
        private float angularVelocity;
        private Vector2[] transformedVertices;
        private bool transformUpdateRequired;
        private bool aabbUpdateRequired;
        private Vector2 force;
        private AABB aabb;
        private bool useGravity = true;
        public bool UseGravity { get => useGravity; set => useGravity = value; }

        public readonly float Density;
        public readonly float Mass;
        public readonly float Restituion;
        public readonly float Area;
        public readonly float MomentOfInertia;
        public readonly bool IsStatic;
        public readonly float Radius;
        public readonly float Width;
        public readonly float Height;
        public readonly float InvMass;
        public readonly float InvMomentOfInertia;
        public readonly float StaticFriction;
        public readonly float DynamicFriction;
        private readonly Vector2[] vertices;
        public readonly ShapeType ShapeType;

        public Vector2 Position => position;
        public Vector2 LinearVelocity
        {
            get => linearVelocity;
            set => linearVelocity = value;
        }

        public float AngularVelocity
        {
            get => angularVelocity;
            set => angularVelocity = value;
        }

        public Rigidbody(float density, float mass, float momentOfInertia, float restituion,
            float area, bool isStatic, float radius, float width,
            float height, Vector2[] vertices, ShapeType shpaeType)
        {
            this.position = Vector2.Zero;
            linearVelocity = Vector2.Zero;
            angle = 0;
            angularVelocity = 0;
            StaticFriction = 0.6f;
            DynamicFriction = 0.4f;

            Density = density;
            Mass = mass;
            InvMass = mass > 0 ? 1f / mass : 0;
            MomentOfInertia = momentOfInertia;
            InvMomentOfInertia = momentOfInertia > 0 ? 1f / momentOfInertia : 0;
            Restituion = restituion;
            Area = area;
            IsStatic = isStatic;
            Radius = radius;
            Width = width;
            Height = height;
            ShapeType = shpaeType;

            if (ShapeType is ShapeType.Box)
            {
                this.vertices = vertices;
                transformedVertices = new Vector2[vertices.Length];
            }
            else
            {
                vertices = null;
                transformedVertices = null;
            }
            transformUpdateRequired = true;
            aabbUpdateRequired = true;
        }

        public AABB GetAABB()
        {
            if (aabbUpdateRequired)
            {
                float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
                if (ShapeType is ShapeType.Box)
                {
                    Vector2[] transformedVertices = GetTransformedVertices();
                    for (int i = 0; i < transformedVertices.Length; i++)
                    {
                        if (minX > transformedVertices[i].X)
                            minX = transformedVertices[i].X;
                        if (minY > transformedVertices[i].Y)
                            minY = transformedVertices[i].Y;
                        if (maxX < transformedVertices[i].X)
                            maxX = transformedVertices[i].X;
                        if (maxY < transformedVertices[i].Y)
                            maxY = transformedVertices[i].Y;
                    }
                }
                else
                {
                    minX = position.X - Radius;
                    minY = position.Y - Radius;
                    maxX = position.X + Radius;
                    maxY = position.Y + Radius;
                }
                aabb = new AABB(minX, minY, maxX, maxY);
            }
            aabbUpdateRequired = false;
            return aabb;
        }

        public void Step(float time, Vector2 gravity, int iterations)
        {
            if (IsStatic)
                return;
            time /= iterations;
            Vector2 acceleration = force * InvMass;
            // System.Console.WriteLine("acc : " + acceleration);
            linearVelocity += acceleration * time;
            if (useGravity)
                linearVelocity += gravity * time;
            position += linearVelocity * time;
            angle += angularVelocity * time;
            linearVelocity *= 0.99f; // for friction
            force *= 0.80f;
            transformUpdateRequired = true;
            aabbUpdateRequired = true;
        }

        public void AddForce(Vector2 amount)
        {
            force += amount;
            this.transformUpdateRequired = true;
            this.aabbUpdateRequired = true;
        }

        private static int[] CreateBoxTriangles()
        {
            int[] triangles = new int[6];
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
            return triangles;
        }
        public static Vector2[] CreateBoxVertices(float width, float height)
        {
            float left = -width / 2f;
            float right = left + width;
            float bottom = -height / 2f;
            float top = bottom + height;

            Vector2[] vertices = new Vector2[4];
            vertices[0] = new Vector2(left, top);
            vertices[1] = new Vector2(right, top);
            vertices[2] = new Vector2(right, bottom);
            vertices[3] = new Vector2(left, bottom);
            return vertices;
        }
        public Vector2[] GetTransformedVertices()
        {
            if (transformUpdateRequired)
            {
                Matrix transform = Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(position.X, position.Y, 0);
                if (vertices == null || vertices.Length < 1)
                    System.Console.WriteLine("vertices null");
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector2 v = vertices[i];
                    transformedVertices[i] = Vector2.Transform(v, transform);
                }
            }
            transformUpdateRequired = false;
            return transformedVertices;
        }

        public void Move(Vector2 amount)
        {
            this.position += amount;
            this.transformUpdateRequired = true;
            this.aabbUpdateRequired = true;
        }
        public void MoveTo(Vector2 position)
        {
            this.position = position;
            this.transformUpdateRequired = true;
            this.aabbUpdateRequired = true;
        }

        public void Rotate(float amount)
        {
            this.angle += amount;
            this.transformUpdateRequired = true;
            this.aabbUpdateRequired = true;
        }
        public void RotateTo(float amount)
        {
            this.angle = amount;
            this.transformUpdateRequired = true;
            this.aabbUpdateRequired = true;
        }
        public static bool CreateCircleBody(float radius, float density, bool isStatic, float restituion, out Rigidbody body, out string errorMessage)
        {
            body = null;
            errorMessage = string.Empty;

            float area = radius * radius * MathF.PI;

            if (area < PhysicsWorld.MinBodySize)
            {
                errorMessage = $"Circle radius is too small. Min circle area is {PhysicsWorld.MinBodySize}";
                return false;
            }
            if (area > PhysicsWorld.MaxBodySize)
            {
                errorMessage = $"Circle radius is too large. Max circle area is {PhysicsWorld.MaxBodySize}";
                return false;
            }

            if (density < PhysicsWorld.MinDensity)
            {
                errorMessage = $"Density is too small. Min density is {PhysicsWorld.MinDensity}";
                return false;
            }
            if (density > PhysicsWorld.MaxDensity)
            {
                errorMessage = $"Density is too large. Max density is {PhysicsWorld.MaxDensity}";
                return false;
            }

            restituion = MathHelper.Clamp(restituion, 0, 1);

            //mass = area * density * depth 
            float mass = 0;
            float inertia = 0;
            if (isStatic == false)
            {
                mass = area * density;// here depth is 1
                inertia = mass * radius * radius;
            }
            body = new Rigidbody(density, mass, inertia, restituion, area, isStatic, radius, 0, 0, null, ShapeType.Circle);
            return true;
        }

        public static bool CreateBoxBody(float width, float height, float density, bool isStatic, float restituion, out Rigidbody body, out string errorMessage)
        {
            body = null;
            errorMessage = string.Empty;

            float area = width * height;

            if (area < PhysicsWorld.MinBodySize)
            {
                errorMessage = $"Area is too small. Min area is {PhysicsWorld.MinBodySize}";
                return false;
            }
            if (area > PhysicsWorld.MaxBodySize)
            {
                errorMessage = $"Area is too large. Max area is {PhysicsWorld.MaxBodySize}";
                return false;
            }

            if (density < PhysicsWorld.MinDensity)
            {
                errorMessage = $"Density is too small. Min density is {PhysicsWorld.MinDensity}";
                return false;
            }
            if (density > PhysicsWorld.MaxDensity)
            {
                errorMessage = $"Density is too large. Max density is {PhysicsWorld.MaxDensity}";
                return false;
            }

            restituion = MathHelper.Clamp(restituion, 0, 1);

            float mass = 0f;
            float inertia = 0f;
            if (isStatic == false)
            {
                //mass = area * density * depth 
                mass = area * density;// here depth is 1
                inertia = (1 / 12f) * mass * (4 * height * height + width * width); // the fix was the '12' to '12f'! spent six hours on this!
                // System.Console.WriteLine("inertia is " + inertia + " height is " + height + " width is " + width + " mass is " + mass);
            }

            Vector2[] vertices = CreateBoxVertices(width, height);
            body = new Rigidbody(density, mass, inertia, restituion, area, isStatic, 0, width, height, vertices, ShapeType.Box);
            return true;
        }
    }
}
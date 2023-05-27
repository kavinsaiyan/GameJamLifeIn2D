using Microsoft.Xna.Framework;
namespace SimplePhysics
{
    public static class Collisions
    {
        public static bool IntersectPolygons(Vector2 centerA, Vector2[] verticesA, Vector2 centerB, Vector2[] verticesB, out float depth, out Vector2 normal)
        {
            depth = float.MaxValue;
            normal = new Vector2();
            for (int i = 0; i < verticesA.Length - 1; i++)
            {
                Vector2 va = verticesA[i];
                Vector2 vb = verticesA[(i + 1) % verticesA.Length];
                Vector2 edge = va - vb;
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis = Vector2.Normalize(axis);

                ProjectVertices(axis, verticesA, out float minA, out float maxA);
                ProjectVertices(axis, verticesB, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                    return false;

                float axisDepth = System.MathF.Min(maxB - minA, maxA - minB);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }

            }
            for (int i = 0; i < verticesB.Length - 1; i++)
            {
                Vector2 va = verticesB[i];
                Vector2 vb = verticesB[(i + 1) % verticesB.Length];
                Vector2 edge = vb - va;
                Vector2 axis = new Vector2(-edge.Y, edge.X);

                ProjectVertices(axis, verticesA, out float minA, out float maxA);
                ProjectVertices(axis, verticesB, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                    return false;

                float axisDepth = System.MathF.Min(maxB - minA, maxA - minB);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }

            Vector2 direction = centerB - centerA;
            if (Vector2.Dot(direction, normal) < 0)
            {
                normal = -normal;
            }

            return true;
        }

        public static bool IntersectCircleAndPolygon(Vector2 circleCenter, float radius, Vector2 polygonCenter, Vector2[] vertices, out float depth, out Vector2 normal)
        {
            depth = float.MaxValue;
            normal = new Vector2();
            Vector2 axis;
            float axisDepth;
            float minA, maxA, minB, maxB;
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                Vector2 va = vertices[i];
                Vector2 vb = vertices[(i + 1) % vertices.Length];
                Vector2 edge = va - vb;
                axis = new Vector2(-edge.Y, edge.X);
                axis = Vector2.Normalize(axis);

                ProjectVertices(axis, vertices, out minA, out maxA);
                ProjectCircle(circleCenter, radius, axis, out minB, out maxB);

                // System.Console.WriteLine($"intersect {minA}, {maxA} : {minB}, {maxB} ");
                if (minA >= maxB || minB >= maxA)
                {
                    // System.Console.WriteLine($"intersect f 1 : c1 : " + (minA >= maxB));
                    // System.Console.WriteLine($"intersect f 1 : c2 : " + (minB >= maxA));
                    return false;
                }

                axisDepth = System.MathF.Min(maxB - minA, maxA - minB);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }
            int index = ClosestPointToCenter(circleCenter, vertices);

            axis = vertices[index] - circleCenter;

            ProjectVertices(axis, vertices, out minA, out maxA);
            ProjectCircle(circleCenter, radius, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
            {
                // System.Console.WriteLine("intersect f 2");
                return false;
            }

            axisDepth = System.MathF.Min(maxB - minA, maxA - minB);
            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }

            Vector2 direction = polygonCenter - circleCenter;
            if (Vector2.Dot(direction, normal) < 0)
            {
                normal = -normal;
            }

            return true;
        }
        public static bool IntersectPolygons(Vector2[] verticesA, Vector2[] verticesB, out float depth, out Vector2 normal)
        {
            depth = float.MaxValue;
            normal = new Vector2();
            for (int i = 0; i < verticesA.Length - 1; i++)
            {
                Vector2 va = verticesA[i];
                Vector2 vb = verticesA[(i + 1) % verticesA.Length];
                Vector2 edge = va - vb;
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis = Vector2.Normalize(axis);

                ProjectVertices(axis, verticesA, out float minA, out float maxA);
                ProjectVertices(axis, verticesB, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                    return false;

                float axisDepth = System.MathF.Min(maxB - minA, maxA - minB);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }

            }
            for (int i = 0; i < verticesB.Length - 1; i++)
            {
                Vector2 va = verticesB[i];
                Vector2 vb = verticesB[(i + 1) % verticesB.Length];
                Vector2 edge = vb - va;
                Vector2 axis = new Vector2(-edge.Y, edge.X);

                ProjectVertices(axis, verticesA, out float minA, out float maxA);
                ProjectVertices(axis, verticesB, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                    return false;

                float axisDepth = System.MathF.Min(maxB - minA, maxA - minB);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }

            Vector2 centerA = FindArithmeticMean(verticesA);
            Vector2 centerB = FindArithmeticMean(verticesB);

            Vector2 direction = centerB - centerA;
            if (Vector2.Dot(direction, normal) < 0)
            {
                normal = -normal;
            }

            return true;
        }

        public static bool IntersectCircleAndPolygon(Vector2 circleCenter, float radius, Vector2[] vertices, out float depth, out Vector2 normal)
        {
            depth = float.MaxValue;
            normal = new Vector2();
            Vector2 axis;
            float axisDepth;
            float minA, maxA, minB, maxB;
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                Vector2 va = vertices[i];
                Vector2 vb = vertices[(i + 1) % vertices.Length];
                Vector2 edge = va - vb;
                axis = new Vector2(-edge.Y, edge.X);
                axis = Vector2.Normalize(axis);

                ProjectVertices(axis, vertices, out minA, out maxA);
                ProjectCircle(circleCenter, radius, axis, out minB, out maxB);

                // System.Console.WriteLine($"intersect {minA}, {maxA} : {minB}, {maxB} ");
                if (minA >= maxB || minB >= maxA)
                {
                    // System.Console.WriteLine($"intersect f 1 : c1 : " + (minA >= maxB));
                    // System.Console.WriteLine($"intersect f 1 : c2 : " + (minB >= maxA));
                    return false;
                }

                axisDepth = System.MathF.Min(maxB - minA, maxA - minB);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }
            int index = ClosestPointToCenter(circleCenter, vertices);

            axis = vertices[index] - circleCenter;

            ProjectVertices(axis, vertices, out minA, out maxA);
            ProjectCircle(circleCenter, radius, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
            {
                // System.Console.WriteLine("intersect f 2");
                return false;
            }

            axisDepth = System.MathF.Min(maxB - minA, maxA - minB);
            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }

            Vector2 polygonCenter = FindArithmeticMean(vertices);
            Vector2 direction = polygonCenter - circleCenter;
            if (Vector2.Dot(direction, normal) < 0)
            {
                normal = -normal;
            }

            return true;
        }
        public static void ProjectCircle(Vector2 center, float radius, Vector2 axis, out float min, out float max)
        {
            Vector2 directionAndRadius = Vector2.Normalize(axis) * radius;
            Vector2 p1 = center + directionAndRadius;
            Vector2 p2 = center - directionAndRadius;
            min = Vector2.Dot(p1, axis);
            max = Vector2.Dot(p2, axis);
            if (min > max)
                (min, max) = (max, min);
        }
        public static int ClosestPointToCenter(Vector2 center, Vector2[] vertices)
        {
            if (vertices == null || vertices.Length == 0)
                throw new System.Exception("vertices are not initialized!");
            float min = float.MaxValue;
            int index = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                float dist = Vector2.Distance(center, vertices[i]);
                if (dist < min)
                {
                    min = dist;
                    index = i;
                }
            }
            return index;
        }
        public static Vector2 FindArithmeticMean(Vector2[] vertices)
        {
            float sumX = 0;
            float sumY = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 v = vertices[i];
                sumX += v.X;
                sumY += v.Y;
            }
            return new Vector2(sumX / (float)vertices.Length, sumY / (float)vertices.Length);
        }

        public static void ProjectVertices(Vector2 normal, Vector2[] vertices, out float min, out float max)
        {
            min = float.MaxValue;
            max = float.MinValue;
            for (int i = 0; i < vertices.Length; i++)
            {
                float dot = Vector2.Dot(vertices[i], normal);
                if (dot > max)
                    max = dot;
                if (dot < min)
                    min = dot;
            }
        }

        public static bool IntersectCircles(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB, out Vector2 normal, out float depth)
        {
            normal = Vector2.Zero;
            depth = 0;

            float distance = Vector2.Distance(centerB, centerA);
            float radii = radiusA + radiusB;

            if (distance >= radii)
            {
                return false;
            }

            normal = Vector2.Normalize(centerB - centerA);
            depth = radii - distance;

            return true;
        }
        public static void GetContactPointBetweenCircleBodies(Vector2 centerA, float radiusA, Vector2 centerB, out Vector2 contactPoint)
        {
            Vector2 direction = centerB - centerA;
            direction = Vector2.Normalize(direction);
            contactPoint = centerA + direction * radiusA;
        }
        public static void GetContactPointsBetweenCircleAndPolygon(Vector2 center, Vector2[] vertices, out Vector2 contact1)
        {
            contact1 = Vector2.Zero;
            float minDistance = float.MaxValue;
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                Vector2 a = vertices[i];
                Vector2 b = vertices[i + 1];

                Collisions.ClosestPointInSegment(a, b, center, out float distanceSquared, out Vector2 closestPoint);
                if (distanceSquared < minDistance)
                {
                    minDistance = distanceSquared;
                    contact1 = closestPoint;
                }
            }
        }
        public static void GetContactPointsBetweenPolygons(Vector2[] verticesA, Vector2[] verticesB,
                    out Vector2 contact1, out Vector2 contact2, out int contactCount)
        {
            contact1 = Vector2.Zero;
            contact2 = Vector2.Zero;
            contactCount = 0;
            float minDistance = float.MaxValue;
            for (int i = 0; i < verticesA.Length; i++)
            {
                Vector2 v = verticesA[i];

                for (int j = 0; j < verticesB.Length; j++)
                {
                    Vector2 va = verticesB[j];
                    Vector2 vb = verticesB[(j + 1) % verticesB.Length];

                    ClosestPointInSegment(va, vb, v, out float distanceSquared, out Vector2 closestPoint);

                    if (MathHelper.NearlyEqual(distanceSquared, minDistance) == true)
                    {
                        if (MathHelper.NearlyEqual(closestPoint, contact1) == false)
                        {
                            contact2 = closestPoint;
                            contactCount = 2;
                        }
                    }
                    else if (distanceSquared < minDistance)
                    {
                        minDistance = distanceSquared;
                        contact1 = closestPoint;
                        contactCount = 1;
                    }
                }
            }
            for (int i = 0; i < verticesB.Length; i++)
            {
                Vector2 v = verticesB[i];

                for (int j = 0; j < verticesA.Length; j++)
                {
                    Vector2 va = verticesA[j];
                    Vector2 vb = verticesA[(j + 1) % verticesA.Length];

                    ClosestPointInSegment(va, vb, v, out float distanceSquared, out Vector2 closestPoint);

                    if (MathHelper.NearlyEqual(distanceSquared, minDistance) == true)
                    {
                        if (MathHelper.NearlyEqual(closestPoint, contact1) == false)
                        {
                            contact2 = closestPoint;
                            contactCount = 2;
                        }
                    }
                    else if (distanceSquared < minDistance)
                    {
                        minDistance = distanceSquared;
                        contact1 = closestPoint;
                        contactCount = 1;
                    }
                }
            }
        }
        public static void GetContactPoints(Rigidbody bodyA, Rigidbody bodyB, out Vector2 contact1, out Vector2 contact2, out int count)
        {
            contact1 = Vector2.Zero;
            contact2 = Vector2.Zero;
            count = 0;
            if (bodyA.ShapeType is ShapeType.Box)
            {
                if (bodyB.ShapeType is ShapeType.Box)
                {
                    GetContactPointsBetweenPolygons(bodyA.GetTransformedVertices(), bodyB.GetTransformedVertices(), out contact1, out contact2, out count);
                }
                else if (bodyB.ShapeType is ShapeType.Circle)
                {
                    count = 1;
                    GetContactPointsBetweenCircleAndPolygon(bodyB.Position, bodyA.GetTransformedVertices(), out contact1);
                }
            }
            else if (bodyA.ShapeType is ShapeType.Circle)
            {
                if (bodyB.ShapeType is ShapeType.Box)
                {
                    count = 1;
                    GetContactPointsBetweenCircleAndPolygon(bodyA.Position, bodyB.GetTransformedVertices(), out contact1);
                }
                else if (bodyB.ShapeType is ShapeType.Circle)
                {
                    count = 1;
                    GetContactPointBetweenCircleBodies(bodyA.Position, bodyA.Radius, bodyB.Position, out contact1);
                }
            }
        }
        public static bool Collide(Rigidbody bodyA, Rigidbody bodyB, out Vector2 normal, out float depth)
        {
            normal = Vector2.Zero;
            depth = 0;
            if (bodyA.ShapeType is ShapeType.Box)
            {
                if (bodyB.ShapeType is ShapeType.Box)
                {
                    return Collisions.IntersectPolygons(
                        bodyA.Position, bodyA.GetTransformedVertices(),
                        bodyB.Position, bodyB.GetTransformedVertices(),
                        out depth, out normal
                    );
                }
                else if (bodyB.ShapeType is ShapeType.Circle)
                {
                    bool res = Collisions.IntersectCircleAndPolygon(bodyB.Position, bodyB.Radius,
                        bodyA.Position, bodyA.GetTransformedVertices(),
                        out depth, out normal
                    );
                    normal = -normal;
                    return res;
                }
            }
            else if (bodyA.ShapeType is ShapeType.Circle)
            {
                if (bodyB.ShapeType is ShapeType.Box)
                {
                    return Collisions.IntersectCircleAndPolygon(
                        bodyA.Position, bodyA.Radius,
                        bodyB.Position, bodyB.GetTransformedVertices(),
                        out depth, out normal
                    );
                }
                else if (bodyB.ShapeType is ShapeType.Circle)
                {
                    return Collisions.IntersectCircles(
                        bodyA.Position, bodyA.Radius,
                        bodyB.Position, bodyB.Radius,
                        out normal, out depth
                    );
                }
            }
            return false;
        }

        public static bool Collide(AABB bodyA, AABB bodyB)
        {
            if (bodyA.Max.X <= bodyB.Min.X || bodyB.Max.X <= bodyA.Min.X)
                return false;
            if (bodyA.Max.Y <= bodyB.Min.Y || bodyB.Max.Y <= bodyA.Min.Y)
                return false;
            return true;
        }

        public static void ClosestPointInSegment(Vector2 a, Vector2 b, Vector2 p, out float distanceSquared, out Vector2 closestPoint)
        {
            closestPoint = Vector2.Zero;
            Vector2 ab = b - a;
            Vector2 ap = p - a;

            float dot = Vector2.Dot(ab, ap);
            float lengthSq = ab.LengthSquared();
            float d = dot / lengthSq;

            if (d <= 0)
                closestPoint = a;
            if (d >= 1)
                closestPoint = b;
            if (d > 0 && d < 1)
                closestPoint = a + d * ab;

            distanceSquared = Vector2.DistanceSquared(p, closestPoint);
        }

    }
}
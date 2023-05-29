using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace SimplePhysics
{
    public sealed class PhysicsWorld
    {
        public static readonly float MinBodySize = 0.01f * 0.01f;
        public static readonly float MaxBodySize = 256 * 256f;

        public static readonly float MinDensity = 0.5f;        //g/cm^3
        public static readonly float MaxDensity = 21.4f;
        public static readonly int MinIterations = 1;
        public static readonly int MaxIterations = 128;
        public List<Rigidbody> _bodyList;
        private List<(int, int)> contactPairs;
        Vector2[] contacts;
        Vector2[] impulses;
        Vector2[] frictionImpulses;
        Vector2[] raList;
        Vector2[] rbList;
        float[] jList;
        public Vector2 gravity;
        public int Count
        {
            get
            {
                return _bodyList.Count;
            }
        }
        public PhysicsWorld()
        {
            gravity = new Vector2(0, -9.807f);
            _bodyList = new List<Rigidbody>();
            contactPairs = new List<(int, int)>();
            contacts = new Vector2[2];
            raList = new Vector2[contacts.Length];
            rbList = new Vector2[contacts.Length];
            impulses = new Vector2[contacts.Length];
            frictionImpulses = new Vector2[contacts.Length];
            jList = new float[contacts.Length];
        }

        public void AddBody(Rigidbody flatBody)
        {
            _bodyList.Add(flatBody);
        }

        public void RemoveBody(Rigidbody flatBody)
        {
            _bodyList.Remove(flatBody);
        }

        public bool GetBody(int index, out Rigidbody body)
        {
            body = null;
            if (index < 0 || index >= _bodyList.Count)
                return false;
            body = _bodyList[index];
            return true;
        }

        public void Step(float time, int iterations)
        {
            for (int currentIteration = 0; currentIteration < iterations; currentIteration++)
            {
                contactPairs.Clear();
                StepBodies(time, iterations);
                BroadPhase();
                NarrowPhase();
            }
        }

        private void StepBodies(float time, int iterations)
        {
            //movement step
            for (int i = 0; i < _bodyList.Count; i++)
            {
                _bodyList[i].Step(time, gravity, iterations);
            }
        }

        public void BroadPhase()
        {
            //collision step
            for (int i = 0; i < _bodyList.Count; i++)
            {
                Rigidbody bodyA = _bodyList[i];
                AABB bodyA_AABB = bodyA.GetFlatAABB();
                for (int j = i + 1; j < _bodyList.Count; j++)
                {
                    Rigidbody bodyB = _bodyList[j];
                    if (bodyA.IsStatic && bodyB.IsStatic)
                        continue;

                    AABB bodyB_AABB = bodyB.GetFlatAABB();
                    if (Collisions.Collide(bodyA_AABB, bodyB_AABB) == false)
                        continue;
                    contactPairs.Add((i, j));
                }
            }
        }

        public void NarrowPhase()
        {
            //collision resolution step
            for (int i = 0; i < contactPairs.Count; i++)
            {
                Rigidbody bodyA = _bodyList[contactPairs[i].Item1];
                Rigidbody bodyB = _bodyList[contactPairs[i].Item2];
                if (Collisions.Collide(bodyA, bodyB, out Vector2 normal, out float depth))
                {
                    SeparateBodies(bodyA, bodyB, normal * depth);
                    Collisions.GetContactPoints(bodyA, bodyB, out Vector2 contact1, out Vector2 contact2, out int count);
                    CollisionManifold contact = (new CollisionManifold(bodyA, bodyB, normal, depth, contact1, contact2, count));
                    ResolveCollision(in contact);
                }
            }
        }
        public void SeparateBodies(Rigidbody bodyA, Rigidbody bodyB, Vector2 mtv)
        {
            if (bodyA.IsStatic)
                bodyB.Move(mtv);
            else if (bodyB.IsStatic)
                bodyA.Move(-mtv);
            else
            {
                bodyA.Move(-mtv / 2f);
                bodyB.Move(mtv / 2f);
            }
        }

        public void ResolveCollision(in CollisionManifold collisionManifold)
        {
            Rigidbody bodyA = collisionManifold.BodyA;
            Rigidbody bodyB = collisionManifold.BodyB;
            Vector2 normal = collisionManifold.Normal;
            float depth = collisionManifold.Depth;
            Vector2 relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;

            if (MathHelper.Dot(relativeVelocity, normal) > 0f)
            {
                return;
            }
            float e = MathF.Min(bodyA.Restituion, bodyB.Restituion);

            float j = -(1f + e) * MathHelper.Dot(relativeVelocity, normal);
            j /= bodyA.InvMass + bodyB.InvMass;

            Vector2 impulse = j * normal;
            bodyA.LinearVelocity -= impulse * bodyA.InvMass;
            bodyB.LinearVelocity += impulse * bodyB.InvMass;
        }

        public void ResolveCollisionWithRotationAndFriction(in CollisionManifold collisionManifold)
        {
            // string log = string.Empty;
            Rigidbody bodyA = collisionManifold.BodyA;
            Rigidbody bodyB = collisionManifold.BodyB;
            Vector2 normal = collisionManifold.Normal;
            Vector2 contact1 = collisionManifold.Contact1;
            Vector2 contact2 = collisionManifold.Contact2;
            // log += "\nis static A: " + bodyA.IsStatic + " b : " + bodyB.IsStatic + "\n";
            // log += "contact count : " + collisionManifold.ContactCount + "\n";
            // log += "depth : " + collisionManifold.Depth + "\n";
            // log += "normal : " + collisionManifold.Normal + "\n";

            float contactCount = collisionManifold.ContactCount;
            float depth = collisionManifold.Depth;
            float e = MathF.Min(bodyA.Restituion, bodyB.Restituion);
            float sf = (bodyA.StaticFriction + bodyB.StaticFriction) * 0.5f;
            float df = (bodyA.DynamicFriction + bodyB.DynamicFriction) * 0.5f;
            // log += "e : " + e + "\n";
            //reset the list values
            contacts[0] = contact1;
            contacts[1] = contact2;
            for (int i = 0; i < contactCount; i++)
            {
                impulses[i] = Vector2.Zero;
                raList[i] = Vector2.Zero;
                jList[i] = 0f;
                rbList[i] = Vector2.Zero;
            }
            for (int i = 0; i < contactCount; i++)
            {
                // log += $"processing contact : {i} at {contacts[i]}\n";
                Vector2 ra = contacts[i] - bodyA.Position;
                Vector2 rb = contacts[i] - bodyB.Position;
                // log += $"ra : {ra} rb : {rb}\n";
                raList[i] = ra;
                rbList[i] = rb;

                Vector2 raPerp = new Vector2(-ra.Y, ra.X);
                Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

                Vector2 angularVelocityA = raPerp * bodyA.AngularVelocity;
                Vector2 angularVelocityB = rbPerp * bodyB.AngularVelocity;
                // log += $"angularVel A : {angularVelocityA} angularVel B : {angularVelocityB}\n";
                Vector2 relativeVelocity = (bodyB.LinearVelocity + angularVelocityB) - (bodyA.LinearVelocity + angularVelocityA);
                // log += $"relativeVelocity : " + relativeVelocity + "\n";

                float contactVelocityMag = MathHelper.Dot(relativeVelocity, normal);
                // log += "contact velocity mag : " + contactVelocityMag + "\n";
                if (contactVelocityMag > 0f)
                {
                    return;
                }

                float raPerpDotN = MathHelper.Dot(raPerp, normal);
                float rbPerpDotN = MathHelper.Dot(rbPerp, normal);

                float denom = bodyA.InvMass + bodyB.InvMass
                        + (raPerpDotN * raPerpDotN) * bodyA.InvMomentOfInertia
                        + (rbPerpDotN * rbPerpDotN) * bodyB.InvMomentOfInertia;

                float j = -(1f + e) * contactVelocityMag;
                j /= denom;
                j /= (float)contactCount;
                jList[i] = j;
                Vector2 impulse = j * normal;
                impulses[i] = impulse;
                // log += "impulse : " + impulse + "\n";
            }

            for (int i = 0; i < contactCount; i++)
            {
                // log += $"processing contact again : {i} at {contacts[i]}\n";
                Vector2 impulse = impulses[i];
                Vector2 ra = raList[i];
                Vector2 rb = rbList[i];
                // log += $"bodyA.invmass: {bodyA.InvMass} bodyB.invmass {bodyB.InvMass}\n";
                // log += $"bodyA.Momentofinertia : {bodyA.MomentOfInertia} bodyB.Momentofinertia : {bodyB.MomentOfInertia}\n";
                // log += $"bodyA.invMomentofinertia : {bodyA.InvMomentOfInertia} bodyB.invMomentofinertia : {bodyB.InvMomentOfInertia}\n";
                bodyA.LinearVelocity += -impulse * bodyA.InvMass;
                bodyA.AngularVelocity += -MathHelper.Cross(ra, impulse) * bodyA.InvMomentOfInertia;
                bodyB.LinearVelocity += impulse * bodyB.InvMass;
                bodyB.AngularVelocity += MathHelper.Cross(rb, impulse) * bodyB.InvMomentOfInertia;
            }
            // System.IO.File.AppendAllText("flatlogs.txt", log);

            for (int i = 0; i < contactCount; i++)
            {
                frictionImpulses[i] = Vector2.Zero;
                raList[i] = Vector2.Zero;
                rbList[i] = Vector2.Zero;
            }
            for (int i = 0; i < contactCount; i++)
            {
                // log += $"processing contact : {i} at {contacts[i]}\n";
                Vector2 ra = contacts[i] - bodyA.Position;
                Vector2 rb = contacts[i] - bodyB.Position;
                // log += $"ra : {ra} rb : {rb}\n";
                raList[i] = ra;
                rbList[i] = rb;

                Vector2 raPerp = new Vector2(-ra.Y, ra.X);
                Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

                Vector2 angularVelocityA = raPerp * bodyA.AngularVelocity;
                Vector2 angularVelocityB = rbPerp * bodyB.AngularVelocity;
                // log += $"angularVel A : {angularVelocityA} angularVel B : {angularVelocityB}\n";
                Vector2 relativeVelocity = (bodyB.LinearVelocity + angularVelocityB) - (bodyA.LinearVelocity + angularVelocityA);
                // log += $"relativeVelocity : " + relativeVelocity + "\n";

                Vector2 tangent = relativeVelocity - MathHelper.Dot(normal, relativeVelocity) * normal;
                // log += "contact velocity mag : " + contactVelocityMag + "\n";
                if (MathHelper.NearlyEqual(tangent, Vector2.Zero))
                    return;
                else
                    tangent = MathHelper.Normalize(tangent);

                float raPerpDotN = MathHelper.Dot(raPerp, tangent);
                float rbPerpDotN = MathHelper.Dot(rbPerp, tangent);

                float denom = bodyA.InvMass + bodyB.InvMass
                        + (raPerpDotN * raPerpDotN) * bodyA.InvMomentOfInertia
                        + (rbPerpDotN * rbPerpDotN) * bodyB.InvMomentOfInertia;

                float jt = -(1f + e) * MathHelper.Dot(relativeVelocity, tangent);
                jt /= denom;
                jt /= (float)contactCount;
                float j = jList[i];
                Vector2 frictionImpulse;
                if (MathF.Abs(jt) <= j * sf)
                    frictionImpulse = jt * tangent;
                else
                    frictionImpulse = -j * tangent * df;
                frictionImpulses[i] = frictionImpulse;
                // log += "impulse : " + impulse + "\n";
            }

            for (int i = 0; i < contactCount; i++)
            {
                // log += $"processing contact again : {i} at {contacts[i]}\n";
                Vector2 frictionImpulse = frictionImpulses[i];
                Vector2 ra = raList[i];
                Vector2 rb = rbList[i];
                // log += $"bodyA.invmass: {bodyA.InvMass} bodyB.invmass {bodyB.InvMass}\n";
                // log += $"bodyA.Momentofinertia : {bodyA.MomentOfInertia} bodyB.Momentofinertia : {bodyB.MomentOfInertia}\n";
                // log += $"bodyA.invMomentofinertia : {bodyA.InvMomentOfInertia} bodyB.invMomentofinertia : {bodyB.InvMomentOfInertia}\n";
                bodyA.LinearVelocity += -frictionImpulse * bodyA.InvMass;
                bodyA.AngularVelocity += -MathHelper.Cross(ra, frictionImpulse) * bodyA.InvMomentOfInertia;
                bodyB.LinearVelocity += frictionImpulse * bodyB.InvMass;
                bodyB.AngularVelocity += MathHelper.Cross(rb, frictionImpulse) * bodyB.InvMomentOfInertia;
            }
            // System.IO.File.AppendAllText("flatlogs.txt", log);
        }
        public void ResolveCollisionWithRotation(in CollisionManifold collisionManifold)
        {
            // string log = string.Empty;
            Rigidbody bodyA = collisionManifold.BodyA;
            Rigidbody bodyB = collisionManifold.BodyB;
            Vector2 normal = collisionManifold.Normal;
            Vector2 contact1 = collisionManifold.Contact1;
            Vector2 contact2 = collisionManifold.Contact2;
            // log += "\nis static A: " + bodyA.IsStatic + " b : " + bodyB.IsStatic + "\n";
            // log += "contact count : " + collisionManifold.ContactCount + "\n";
            // log += "depth : " + collisionManifold.Depth + "\n";
            // log += "normal : " + collisionManifold.Normal + "\n";

            float contactCount = collisionManifold.ContactCount;
            float depth = collisionManifold.Depth;
            float e = MathF.Min(bodyA.Restituion, bodyB.Restituion);
            // log += "e : " + e + "\n";
            //reset the list values
            contacts[0] = contact1;
            contacts[1] = contact2;
            for (int i = 0; i < contactCount; i++)
            {
                impulses[i] = Vector2.Zero;
                raList[i] = Vector2.Zero;
                rbList[i] = Vector2.Zero;
            }
            for (int i = 0; i < contactCount; i++)
            {
                // log += $"processing contact : {i} at {contacts[i]}\n";
                Vector2 ra = contacts[i] - bodyA.Position;
                Vector2 rb = contacts[i] - bodyB.Position;
                // log += $"ra : {ra} rb : {rb}\n";
                raList[i] = ra;
                rbList[i] = rb;

                Vector2 raPerp = new Vector2(-ra.Y, ra.X);
                Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

                Vector2 angularVelocityA = raPerp * bodyA.AngularVelocity;
                Vector2 angularVelocityB = rbPerp * bodyB.AngularVelocity;
                // log += $"angularVel A : {angularVelocityA} angularVel B : {angularVelocityB}\n";
                Vector2 relativeVelocity = (bodyB.LinearVelocity + angularVelocityB) - (bodyA.LinearVelocity + angularVelocityA);
                // log += $"relativeVelocity : " + relativeVelocity + "\n";

                float contactVelocityMag = MathHelper.Dot(relativeVelocity, normal);
                // log += "contact velocity mag : " + contactVelocityMag + "\n";
                if (contactVelocityMag > 0f)
                {
                    return;
                }

                float raPerpDotN = MathHelper.Dot(raPerp, normal);
                float rbPerpDotN = MathHelper.Dot(rbPerp, normal);

                float denom = bodyA.InvMass + bodyB.InvMass
                        + (raPerpDotN * raPerpDotN) * bodyA.InvMomentOfInertia
                        + (rbPerpDotN * rbPerpDotN) * bodyB.InvMomentOfInertia;

                float j = -(1f + e) * contactVelocityMag;
                j /= denom;
                j /= (float)contactCount;

                Vector2 impulse = j * normal;
                impulses[i] = impulse;
                // log += "impulse : " + impulse + "\n";
            }

            for (int i = 0; i < contactCount; i++)
            {
                // log += $"processing contact again : {i} at {contacts[i]}\n";
                Vector2 impulse = impulses[i];
                Vector2 ra = raList[i];
                Vector2 rb = rbList[i];
                // log += $"bodyA.invmass: {bodyA.InvMass} bodyB.invmass {bodyB.InvMass}\n";
                // log += $"bodyA.Momentofinertia : {bodyA.MomentOfInertia} bodyB.Momentofinertia : {bodyB.MomentOfInertia}\n";
                // log += $"bodyA.invMomentofinertia : {bodyA.InvMomentOfInertia} bodyB.invMomentofinertia : {bodyB.InvMomentOfInertia}\n";
                bodyA.LinearVelocity += -impulse * bodyA.InvMass;
                bodyA.AngularVelocity += -MathHelper.Cross(ra, impulse) * bodyA.InvMomentOfInertia;
                bodyB.LinearVelocity += impulse * bodyB.InvMass;
                bodyB.AngularVelocity += MathHelper.Cross(rb, impulse) * bodyB.InvMomentOfInertia;
            }
            // System.IO.File.AppendAllText("flatlogs.txt", log);
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using WickedEngine;
using WickedEngineTests.Mocks;

namespace WickedEngineTests
{
    [TestFixture]
    public class Logic_GameObject
    {
        private ISceneGraph CreateSceneGraphScaffolding()
        {
            return new Mocks.SceneGraphBlank();
        }

        [Test]
        public void AbsolutePosition_RespectsParentRotation()
        {
            GameObject obj = new GameObject()
            {
                Rotation = MathHelper.Pi
            };
            GameObject child = new GameObject()
            {
                Position = new Vector2(32, 32)
            };
            obj.AddChild(child);
            Vector2 expected = new Vector2(-32f, -32f);
            float precision  = .00001f;
            Assert.That(child.AbsolutePosition, Is.Not.EqualTo(child.Position));
            Assert.That(MathFunctions.FloatEquivalent(child.AbsolutePosition.X, expected.X, precision));
            Assert.That(MathFunctions.FloatEquivalent(child.AbsolutePosition.Y, expected.Y, precision));
        }

        [Test]
        public void SetCollisionSize()
        {
            GameObject obj = new GameObject();
            obj.Position = new Vector2(32, 64);
            obj.SetCollisionSize(32, 32);

            Rectangle expected = new Rectangle(32, 64, 32, 32);
            Assert.That(obj.CollisionBounds == expected);
        }

        [Test]
        public void CollisionBounds_SetToAbsolutePosition()
        {
            GameObject obj = new GameObject();
            obj.Position = new Vector2(32f, 64f);
            GameObject child = new GameObject();
            child.Position = new Vector2(32f, 32f);
            obj.AddChild(child);
            Vector2 expected = new Vector2(64f, 96f);

            //verify child absolute position
            Assert.That(child.AbsolutePosition, Is.EqualTo(expected));

            //Collision bounds should be based off of AbsolutePosition
            Assert.That(child.CollisionBounds.X, Is.EqualTo(child.AbsolutePosition.X));
            Assert.That(child.CollisionBounds.Y, Is.EqualTo(child.AbsolutePosition.Y));

            //Collision bounds should not be based off of Relateive Position
            Assert.That(child.CollisionBounds.X, Is.Not.EqualTo(child.Position.X));
            Assert.That(child.CollisionBounds.Y, Is.Not.EqualTo(child.Position.Y));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestCase(-0.1F)]
        [TestCase(-1F)]
        [TestCase(-10000F)]
        public void Speed_NoNegativeValue(float speed)
        {
            GameObject obj = new GameObject();
            obj.Speed = speed;
        }

        [Test]
        [Ignore]
        [TestCase(128F, 90F)]
        [TestCase(-32F, 16F)]
        [TestCase(16F, 0F)]
        public void Velocity_IsNormalized(float xVelocity, float yVelocity)
        {
            Vector2 velocity = new Vector2(xVelocity, yVelocity);
            Vector2 normalized = velocity;
            normalized.Normalize();

            GameObject obj = new GameObject() { Velocity = velocity };

            Assert.That(obj.Velocity, Is.EqualTo(normalized));
        }

        [Test]
        public void Velocity_IsNotZeroNormalized()
        {
            GameObject obj = new GameObject() { Velocity = Vector2.Zero };
            Assert.That(obj.Velocity, Is.EqualTo(Vector2.Zero));

            Vector2 normalizedZero = Vector2.Zero;
            normalizedZero.Normalize();

            Assert.That(obj.Velocity, Is.Not.EqualTo(normalizedZero));
        }

        [Test]
        public void CenterPoint()
        {
            GameObject obj = new GameObject() { Position = new Vector2(32f, 32f) };
            obj.SetDrawSize(128, 256);

            //Center point is half of draw size
            Assert.That(obj.CenterPoint, Is.EqualTo(new Vector2(64f, 128f)));
        }

        [Test]
        public void CenterPoint_NotAffectedByPosition()
        {
            GameObject obj = new GameObject()
            {
                Position = new Vector2(500f, 500f)
            };

            obj.SetDrawSize(64, 64);

            //Center point is not affected by position
            Assert.That(obj.CenterPoint, Is.EqualTo(new Vector2(32f, 32f)));
        }

        [Test]
        public void Update_Moves()
        {
            GameObject obj = new GameObject() { Velocity = Vector2.UnitX };
            Vector2 initialPosition = Vector2.Zero;
            obj.Position = initialPosition;

            Vector2 expectedPosition = new Vector2(2, 0);
            obj.Update();
            Assert.That(obj.AbsolutePosition, Is.EqualTo(expectedPosition));
            Assert.That(obj.Position, Is.EqualTo(expectedPosition));
        }

        [Test]
        public void Intersects()
        {
            GameObject obj1 = new GameObject() { NoClip = false };
            obj1.SetCollisionSize(32, 32);
            GameObject obj2 = new GameObject() { NoClip = false };
            obj2.SetCollisionSize(32, 32);

            Assert.That(obj1.Intersects(obj2));
            Assert.That(obj2.Intersects(obj1));
        }

        [Test]
        public void Intersects_False()
        {
            GameObject obj1 = new GameObject() { NoClip = false };
            obj1.SetCollisionSize(32, 32);
            obj1.Position = new Vector2(128f, 128f);
            GameObject obj2 = new GameObject() { NoClip = false };
            obj2.SetCollisionSize(32, 32);
            obj2.Position = new Vector2(32f, 32f);

            Assert.That(!obj1.Intersects(obj2));
            Assert.That(!obj2.Intersects(obj1));
        }

        [Test]
        public void Intersects_CallOnCollision()
        {
            GameObjectCounter obj1 = new GameObjectCounter() { NoClip = false };
            obj1.SetCollisionSize(32, 32);
            GameObject obj2 = new GameObject() { NoClip = false };
            obj2.SetCollisionSize(32, 32);

            obj1.OnObjectCollision(obj2);

            Assert.That(obj1.Stats.CollisionCount, Is.EqualTo(1));
        }

        [Test]
        public void SetDrawSize()
        {
            GameObject obj = new GameObject();
            obj.Position = new Vector2(500f, 500f);
            obj.SetDrawSize(16, 32);

            Rectangle size = obj.DrawSize;
            Assert.That(size.Width, Is.EqualTo(16));
            Assert.That(size.Height, Is.EqualTo(32));
            Assert.That(size.X, Is.EqualTo(0));
            Assert.That(size.Y, Is.EqualTo(0));
        }

        [Test]
        public void DrawBounds()
        {
            GameObject obj = new GameObject();
            obj.SetDrawSize(16, 32);
            obj.Position = new Vector2(128, 256);

            BoundingBox box = obj.DrawBounds();
            Vector3 expectedMin = new Vector3(obj.AbsolutePosition, 0);
            Vector3 expectedMax = new Vector3(obj.AbsolutePosition, 0);
            expectedMax.X += obj.DrawSize.Width;
            expectedMax.Y += obj.DrawSize.Height;
            Assert.That(box.Min, Is.EqualTo(expectedMin));
            Assert.That(box.Max, Is.EqualTo(expectedMax));

            //Rectangle bounds = new Rectangle( obj.DrawBounds.Min;
            //Assert.That(bounds.Width, Is.EqualTo(16));
            //Assert.That(bounds.Height, Is.EqualTo(32));
            //Assert.That(bounds.X, Is.EqualTo(128));
            //Assert.That(bounds.Y, Is.EqualTo(256));
        }

        [Test]
        public void SpawnAtMe()
        {
            ISceneGraph graph = CreateSceneGraphScaffolding();
            Vector2 objPosition = new Vector2(32, 64);
            GameObjectCounter obj = new GameObjectCounter();
            obj.SceneGraph = graph;
            obj.Position = objPosition;

            GameObjectCounter spawn = new GameObjectCounter();
            obj.SpawnAtMe(spawn);

            Assert.That(graph.RootGraph.Contains(spawn));
        }

        [Test]
        public void MoveUp()
        {
            GameObject obj = new GameObject();
            obj.MoveUp();
            Assert.That(obj.Velocity, Is.EqualTo(-Vector2.UnitY));
        }

        [Test]
        public void MoveDown()
        {
            GameObject obj = new GameObject();
            obj.MoveDown();
            Assert.That(obj.Velocity, Is.EqualTo(Vector2.UnitY));
        }

        [Test]
        public void MoveLeft()
        {
            GameObject obj = new GameObject();
            obj.MoveLeft();
            Assert.That(obj.Velocity, Is.EqualTo(-Vector2.UnitX));
        }

        [Test]
        public void MoveRight()
        {
            GameObject obj = new GameObject();
            obj.MoveRight();
            Assert.That(obj.Velocity, Is.EqualTo(Vector2.UnitX));
        }

        [Test]
        public void MoveUpLeft()
        {
            GameObject obj = new GameObject();
            obj.MoveUp();
            obj.MoveLeft();

            Vector2 expected = -Vector2.UnitY + -Vector2.UnitX;
            //expected.Normalize();
            Assert.That(obj.Velocity, Is.EqualTo(expected));
        }

        [Test]
        public void MoveUpRight()
        {
            GameObject obj = new GameObject();
            obj.MoveUp();
            obj.MoveRight();

            Vector2 expected = -Vector2.UnitY + Vector2.UnitX;
            //expected.Normalize();
            Assert.That(obj.Velocity, Is.EqualTo(expected));
        }

        [Test]
        public void MoveDownLeft()
        {
            GameObject obj = new GameObject();
            obj.MoveDown();
            obj.MoveLeft();

            Vector2 expected = Vector2.UnitY + -Vector2.UnitX;
            //expected.Normalize();
            Assert.That(obj.Velocity, Is.EqualTo(expected));
        }

        [Test]
        public void MoveDownRight()
        {
            GameObject obj = new GameObject();
            obj.MoveDown();
            obj.MoveRight();

            Vector2 expected = Vector2.UnitY + Vector2.UnitX;
            //expected.Normalize();
            Assert.That(obj.Velocity, Is.EqualTo(expected));
        }

        [Test]
        public virtual void StopMovement()
        {
            GameObject obj = new GameObject();
            obj.Velocity = new Vector2(123, 456);
            obj.StopMovement();
            Assert.That(obj.Velocity, Is.EqualTo(Vector2.Zero));
        }
    }
}
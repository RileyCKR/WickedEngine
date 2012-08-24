using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using WickedEngine;
using WickedEngineTests.Mocks;

namespace WickedEngineTests
{
    [TestFixture]
    class Logic_SceneGraph
    {
        private GameObjectStatistics GetObjectTotals(SceneGraph graph)
        {
            GameObjectStatistics stats = new GameObjectStatistics();

            foreach (IGameObjectCounter obj in graph.RootGraph)
            {
                stats.CollisionCount += obj.Stats.CollisionCount;
                stats.CollisionTestCount += obj.Stats.CollisionTestCount;
                stats.DrawCount += obj.Stats.DrawCount;
                stats.UpdateCount += obj.Stats.UpdateCount;
            }

            return stats;
        }

        [Test]
        public void Update_Empty()
        {
            SceneGraph graph = BuildSceneGraph();

            graph.Update();
        }

        [Test]
        public void Update_InjectObject()
        {
            SceneGraph graph = BuildSceneGraph();
            graph.Add(new GameObjectInjector() { SceneGraph = graph });

            graph.Update();
            graph.Draw(null);

            Assert.That(graph.RootGraph.Count, Is.EqualTo(3));
            Assert.That(GetObjectTotals(graph).UpdateCount, Is.EqualTo(2));
            Assert.That(GetObjectTotals(graph).CollisionTestCount, Is.EqualTo(0));
            Assert.That(GetObjectTotals(graph).DrawCount, Is.EqualTo(3));
        }

        [Test]
        public void Update_BeforeLockToMap()
        {
            SceneGraph graph = BuildSceneGraph();
            GameObject obj = new GameObject();
            obj.NoClip = false;
            obj.Position = new Vector2(32, 0);
            obj.Speed = 100;
            obj.Velocity = -Vector2.UnitX;

            graph.Add(obj);

            //If update is called before lock, the object will move off the map and
            //then be placed back on the map after one update
            graph.Update();

            Assert.That(obj.AbsolutePosition, Is.EqualTo(Vector2.Zero));
            Assert.That(obj.Position, Is.EqualTo(Vector2.Zero));
        }

        [Test]
        public void LockToMap_BeforeCollisionCheck()
        {
            SceneGraph graph = BuildSceneGraph();
            GameObjectCounter obj = new GameObjectCounter();
            obj.NoClip = false;
            obj.Position = new Vector2(0, 0);
            obj.SetCollisionSize(16, 16);
            graph.Add(obj);

            GameObject offScreen = new GameObject();
            offScreen.NoClip = false;
            offScreen.Position = new Vector2(-32, -32);
            offScreen.SetCollisionSize(16, 16);
            graph.Add(offScreen);

            //LockToMap should be called before collision checking
            graph.Update();

            Assert.That(obj.Stats.CollisionCount, Is.EqualTo(1));
        }

        [Test]
        public void CalculateTransforms()
        {
            SceneGraph graph = BuildSceneGraph();
            Vector2 parentPosition = new Vector2(32, 64);
            GameObject obj = new GameObject();
            obj.Position = parentPosition;

            Vector2 childPosition = new Vector2(96, 128);
            GameObject childObj = new GameObject();
            childObj.Position = childPosition;

            obj.AddChild(childObj);
            graph.Add(obj);
            graph.Update();

            Assert.That(childObj.Position, Is.EqualTo(childPosition));
            Assert.That(childObj.AbsolutePosition, Is.EqualTo(parentPosition + childPosition));
        }

        [Test]
        public void CalculateTransforms_BeforeCollisionCheck()
        {
            SceneGraph graph = BuildSceneGraph();
            Vector2 parentPosition = new Vector2(32, 64);
            GameObject obj = new GameObject();
            obj.Position = parentPosition;
            obj.NoClip = false;
            obj.SetCollisionSize(16, 16);

            Vector2 childPosition = new Vector2(96, 128);
            GameObject childObj = new GameObject();
            childObj.Position = childPosition;
            childObj.NoClip = false;
            childObj.SetCollisionSize(16, 16);

            obj.AddChild(childObj);

            GameObjectCounter collisionObj = new GameObjectCounter();
            collisionObj.Position = parentPosition + childPosition;
            collisionObj.NoClip = false;
            collisionObj.SetCollisionSize(16, 16);

            graph.Add(obj);
            graph.Add(collisionObj);
            graph.Update();

            Assert.That(collisionObj.Stats.CollisionCount, Is.EqualTo(1));
        }

        [Test]
        public void Draw_Empty()
        {
            SceneGraph graph = BuildSceneGraph();

            graph.Update();
            graph.Draw(null);
        }

        [Test]
        public void Draw_SkipOffScreen()
        {
            SceneGraph graph = BuildSceneGraph();
            GameObject obj = new GameObjectCounter();
            obj.Position = new Vector2(-33f, -33f);
            graph.Add(obj);

            graph.Update();
            graph.Draw(null);

            Assert.That(GetObjectTotals(graph).DrawCount, Is.EqualTo(0));
            Assert.That(graph.NodesCulled, Is.EqualTo(1));
        }

        [Test]
        public void Draw_InjectObject()
        {
            SceneGraph graph = BuildSceneGraph();
            graph.Add(new GameObjectInjector() { SceneGraph = graph });

            graph.Update();
            graph.Draw(null);

            Assert.That(graph.RootGraph.Count, Is.EqualTo(3));
            Assert.That(GetObjectTotals(graph).DrawCount, Is.EqualTo(3));
        }

        [Test]
        public void Update_CollisionFired()
        {
            SceneGraph graph = BuildSceneGraph();

            GameObjectCounter obj1 = new GameObjectCounter() { NoClip = false };
            obj1.SetCollisionSize(32, 32);
            graph.Add(obj1);

            GameObjectCounter obj2 = new GameObjectCounter() { NoClip = false };
            obj2.SetCollisionSize(32, 32);
            graph.Add(obj2);

            graph.Update();

            Assert.That(GetObjectTotals(graph).CollisionTestCount, Is.EqualTo(2));
            Assert.That(GetObjectTotals(graph).CollisionCount, Is.EqualTo(2));
        }

        [Test]
        public void IgnoreCollision_NoClip()
        {
            SceneGraph graph = BuildSceneGraph();

            GameObjectCounter obj1 = new GameObjectCounter() { NoClip = true };
            obj1.SetCollisionSize(32, 32);
            graph.Add(obj1);

            GameObjectCounter obj2 = new GameObjectCounter() { NoClip = false };
            obj2.SetCollisionSize(32, 32);
            graph.Add(obj2);

            graph.Update();

            Assert.That(GetObjectTotals(graph).CollisionTestCount, Is.EqualTo(0));
            Assert.That(GetObjectTotals(graph).CollisionCount, Is.EqualTo(0));
        }

        [Test]
        public void Collision_InjectObject()
        {
            SceneGraph graph = BuildSceneGraph();

            GameObjectCollisionSpawn obj1 = new GameObjectCollisionSpawn(graph) { NoClip = false, SceneGraph = graph };
            obj1.SetCollisionSize(32, 32);
            graph.Add(obj1);

            GameObjectCollisionSpawn obj2 = new GameObjectCollisionSpawn(graph) { NoClip = false, SceneGraph = graph };
            obj2.SetCollisionSize(32, 32);
            graph.Add(obj2);

            graph.Update();
            graph.Draw(null);

            Assert.That(graph.RootGraph.Count, Is.EqualTo(4));
            Assert.That(GetObjectTotals(graph).CollisionTestCount, Is.EqualTo(2));
            Assert.That(GetObjectTotals(graph).CollisionCount, Is.EqualTo(2));
            Assert.That(GetObjectTotals(graph).UpdateCount, Is.EqualTo(2));
            Assert.That(GetObjectTotals(graph).DrawCount, Is.EqualTo(4));
        }

        [Test]
        public void LockToMap_OnlyFirstLevelNodes()
        {
            SceneGraph graph = BuildSceneGraph();
            GameObject obj = new GameObject();
            obj.NoClip = false;
            obj.Position = new Vector2(32, 32);
            obj.SetCollisionSize(16, 16);
            graph.Add(obj);

            Vector2 offScreenPosition = new Vector2(-300, -300);
            GameObject offScreen = new GameObject();
            offScreen.NoClip = false;
            offScreen.Position = offScreenPosition;
            offScreen.SetCollisionSize(16, 16);

            obj.AddChild(offScreen);

            graph.Update();

            Assert.That(offScreen.AbsolutePosition, Is.EqualTo(offScreenPosition + obj.Position));
            Assert.That(offScreen.Position, Is.EqualTo(offScreenPosition));
        }

        private SceneGraph BuildSceneGraph()
        {
            Rectangle viewport = new Rectangle(0, 0, 800, 600);
            Camera2D camera = new Camera2D(viewport);
            TestMap map = new TestMap();
            return new SceneGraph(camera, map);
        }
    }
}
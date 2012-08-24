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
    public class Integration_SceneGraph
    {
        [Test]
        public void SpawnAtMe_Position()
        {
            SceneGraph graph = BuildSceneGraph();
            Vector2 objPosition = new Vector2(32, 64);
            GameObjectCounter obj = new GameObjectCounter();
            obj.Position = objPosition;

            GameObject childObj = new GameObject();
            childObj.Position = new Vector2(16, 16);
            childObj.SceneGraph = graph;
            obj.AddChild(childObj);

            graph.Add(obj);
            graph.Update();

            GameObjectCounter spawn = new GameObjectCounter();
            childObj.SpawnAtMe(spawn);

            graph.Update();

            Assert.That(spawn.Position, Is.EqualTo(childObj.AbsolutePosition));
            Assert.That(spawn.Position, Is.EqualTo(new Vector2(48f, 80f)));
        }

        private SceneGraph BuildSceneGraph()
        {
            Rectangle viewport = new Rectangle(0, 0, 800, 600);
            Camera camera = new Camera(viewport);
            TestMap map = new TestMap();
            return new SceneGraph(camera, map);
        }
    }
}
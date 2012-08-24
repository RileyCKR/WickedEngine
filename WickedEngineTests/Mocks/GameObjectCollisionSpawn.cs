using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;

namespace WickedEngineTests.Mocks
{
    internal class GameObjectCollisionSpawn : GameObjectCounter
    {
        public GameObjectCollisionSpawn(ISceneGraph sceneGraph)
            : base()
        {
        }

        public override void GameObject_ObjectCollision(GameObject sender, GameObjectCollisionEventArgs e)
        {
            GameObjectCounter obj = new GameObjectCounter();
            obj.NoClip = true;
            obj.Position = this.Position;
            SceneGraph.Add(obj);

            base.GameObject_ObjectCollision(sender, e);
        }
    }
}
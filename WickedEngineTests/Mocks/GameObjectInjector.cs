using System;
using System.Collections.Generic;
using WickedEngine;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngineTests.Mocks
{
    internal class GameObjectInjector : GameObjectCounter, IGameObjectCounter
    {
        public GameObjectInjector()
            : base()
        {
        }

        public override void Update()
        {
            SceneGraph.Add(new GameObjectCounter());

            base.Update();
        }

        public override void Draw(SpriteBatch batch)
        {
            SceneGraph.Add(new GameObjectCounter());

            base.Draw(batch);
        }

        public override bool Intersects(GameObject otherObject)
        {
            return base.Intersects(otherObject);
        }

        public override void GameObject_ObjectCollision(GameObject sender, GameObjectCollisionEventArgs e)
        {
            base.GameObject_ObjectCollision(sender, e);

            SceneGraph.Add(new GameObjectCounter());
        }
    }
}

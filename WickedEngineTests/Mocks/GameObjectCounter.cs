using System;
using System.Collections.Generic;
using WickedEngine;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngineTests.Mocks
{
    internal class GameObjectStatistics
    {
        public int UpdateCount { get; set; }
        public int DrawCount { get; set; }
        public int CollisionTestCount { get; set; }
        public int CollisionCount { get; set; }
    }

    internal class GameObjectCounter: GameObject, IGameObjectCounter
    {
        public GameObjectStatistics Stats { get; private set; }

        public GameObjectCounter()
            : base()
        {
            Stats = new GameObjectStatistics();
            Stats.UpdateCount = 0;
            Stats.DrawCount = 0;
            Stats.CollisionTestCount = 0;
            Stats.CollisionCount = 0;
            this.SetDrawSize(32, 32);

            this.ObjectCollision += new GameObjectCollisionEventHandler(GameObject_ObjectCollision);
        }

        public override void Update()
        {
            ++Stats.UpdateCount;

            base.Update();
        }

        public override void Draw(SpriteBatch batch)
        {
            ++Stats.DrawCount;

            base.Draw(batch);
        }

        public override bool Intersects(GameObject otherObject)
        {
            ++Stats.CollisionTestCount;
            return base.Intersects(otherObject);
        }

        public virtual void GameObject_ObjectCollision(GameObject sender, GameObjectCollisionEventArgs e)
        {
            ++Stats.CollisionCount;
        }
    }
}

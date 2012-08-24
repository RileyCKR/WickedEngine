using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCowboy.Objects;

namespace SpaceCowboy.Objects.Weapons
{
    public class Laser : Actor
    {
        GameObject Source;

        public Laser(GameObject source)
        {
            this.Texture = SpaceCowboy.GameScreens.TestLevel.LaserFireTexture;
            this.Source = source;
        }

        public void BeginFire()
        {
        }

        public void EndFire()
        {
        }

        public override void Update()
        {
            base.Update();
            if (Source != null)
            {
                this.Position = Source.AbsolutePosition;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            //base.Draw(batch);
            if (Source != null && Target != null)
            {
                Vector2 source = Source.AbsolutePosition;
                Vector2 destination = Target.AbsolutePosition;

                float angle = (float)Math.Atan2(destination.Y - source.Y, destination.X - source.X);
                float length = Vector2.Distance(source, destination);

                batch.Draw(Texture, this.Position, null, Color.Yellow,
                           angle, Vector2.Zero, new Vector2(length, 2.0f),
                           SpriteEffects.None, 0);
            }
        }
    }
}
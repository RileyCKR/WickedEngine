using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCowboy
{
    public class Asteroid : Sprite, IProcedurallyGenerated
    {
        public void Generate(Random rng, int mass)
        {
            int velocity = rng.Next(0, 360);
            int rotation = rng.Next(0, 360);
            int speed = rng.Next(0, 100);
            int xPos = rng.Next(-10240, 10240);
            int yPos = rng.Next(-10240, 10240);
            float velocityRad = MathHelper.ToRadians(velocity);
            float rotationRad = MathHelper.ToRadians(rotation);

            Vector2 newVelocity = this.Velocity;
            newVelocity.X = (float)Math.Sin(velocityRad);
            newVelocity.Y = -(float)Math.Cos(velocityRad);
            this.Velocity = newVelocity;
            this.Rotation = rotation;
            this.Speed = (float)speed / 100F;
            this.Position = new Vector2(xPos, yPos);

            int percent = rng.Next(0, 100);
            if (percent < 50)
            {
                this.Texture = GameScreens.TestLevel.Asteriod64;
                this.SetDrawSize(64, 64);
                this.SetCollisionSize(64, 64);
            }
            else
            {
                this.Texture = GameScreens.TestLevel.Asteriod32;
                this.SetDrawSize(32, 32);
                this.SetCollisionSize(32, 32);
            }
        }
    }
}

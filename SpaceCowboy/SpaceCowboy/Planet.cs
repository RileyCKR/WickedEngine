using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCowboy
{
    class Planet : Sprite, IProcedurallyGenerated
    {
        public void Generate(Random rng, int mass)
        {
            int percent = rng.Next(0, 100);

            if (percent < 50)
            {
                this.Texture = GameScreens.TestLevel.PlanetBlueTexture;
            }
            else
            {
                this.Texture = GameScreens.TestLevel.PlanetPurpleTexture;
            }

            this.SetDrawSize(128, 128);
            this.SetCollisionSize(128, 128);

            int xPos = rng.Next(-10240, 10240);
            int yPos = rng.Next(-10240, 10240);
            this.Position = new Vector2(xPos, yPos);
        }
    }
}

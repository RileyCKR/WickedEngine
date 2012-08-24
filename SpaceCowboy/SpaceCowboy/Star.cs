using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WickedEngine;

namespace SpaceCowboy
{
    public class Star : Sprite, IProcedurallyGenerated
    {
        public void Generate(Random rng, int mass)
        {
            this.Texture = GameScreens.TestLevel.StarTexture;
            this.SetDrawSize(256, 256);
            this.SetCollisionSize(256, 256);
        }
    }
}
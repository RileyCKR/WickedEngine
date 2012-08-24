using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCowboy
{
    public class SpaceCreature : SpaceShip
    {
        public SpaceCreature() : base()
        {
            this.Texture = GameScreens.TestLevel.CreatureTexture;
            this.SetDrawSize(32, 32);
            this.SetCollisionSize(32, 32);
            this.EngineThrust = .1f;
        }
    }
}
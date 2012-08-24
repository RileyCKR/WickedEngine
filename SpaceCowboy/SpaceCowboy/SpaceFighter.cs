using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCowboy.Objects.Weapons;

namespace SpaceCowboy
{
    public class SpaceFighter : SpaceShip
    {
        public SpaceFighter() : base()
        {
            this.Texture = GameScreens.TestLevel.ShipTexture;
            this.SetDrawSize(64, 64);
            this.SetCollisionSize(64, 64);
            this.EngineThrust = .1f;

            Weapons.Add(new LaserTurret() { Position = Vector2.Zero });
            Weapons.Add(new LaserTurret() { Position = new Vector2(-32f, 0f) });
            Weapons.Add(new LaserTurret() { Position = new Vector2(32f, 0f) });

            this.AddChild(Weapons[0]);
            this.AddChild(Weapons[1]);
            this.AddChild(Weapons[2]);
        }
    }
}

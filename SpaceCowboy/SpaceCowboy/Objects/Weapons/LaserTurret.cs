using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;
using Microsoft.Xna.Framework;
using SpaceCowboy.Objects;

namespace SpaceCowboy.Objects.Weapons
{
    public class LaserTurret : Weapon
    {
        Laser laserShot;

        public LaserTurret()
        {
            this.Texture = GameScreens.TestLevel.LaserTurretTexture;
            this.SetDrawSize(16, 16);
            this.SetCollisionSize(16, 16);
            this.AI = new TargetLockAI();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void BeginFire()
        {
            if (this.Target != null)
            {
                if (laserShot == null)
                {
                    laserShot = new Laser(this);
                    GameScreens.TestLevel.SceneGraph.Add(laserShot);
                }
                laserShot.Target = this.Target;
                laserShot.Visible = true;             
            }
        }

        public override void EndFire()
        {
            if (laserShot != null)
            {
                laserShot.Visible = false;
            }
        }
    }
}
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
    public class SpaceShip : Actor
    {
        public float EngineThrust { get; set; }

        public SpaceShip()
        {
            this.Weapons = new List<Weapon>();
        }

        public override void Update()
        {
            base.Update();
            Rotate();
            ApplyFriction();

            foreach (Weapon weapon in this.Children)
            {
                //Point weapons at the star for target practice
                weapon.Target = this.Target;
            }
        }

        public virtual void Rotate()
        {
            if (this.Velocity != Vector2.Zero)
            {
                this.Rotation = (float)Math.Atan2(this.Velocity.X, -this.Velocity.Y);
            }
        }

        public virtual void ApplyFriction()
        {
            Vector2 friction = this.Velocity / 100;
            this.Velocity -= friction;
        }

        public virtual void ApplyThrust(Vector2 thrustVector)
        {
            if (thrustVector != Vector2.Zero)
            {
                thrustVector.Normalize();
                this.Velocity += thrustVector * EngineThrust;
            }
        }

        public List<Weapon> Weapons { get; set; }
    }
}
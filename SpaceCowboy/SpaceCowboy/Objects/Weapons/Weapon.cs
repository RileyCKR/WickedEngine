using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCowboy.Objects.Weapons
{
    public class Weapon : Actor
    {
        public int SocketNumber { get; set; }

        public virtual void BeginFire()
        {
        }

        public virtual void EndFire()
        {
        }
    }
}
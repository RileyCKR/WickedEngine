using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TiledLib;
using WickedEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using WickedEngine.Input;
using WickedEngine.Components;

namespace WickedDemo
{
    public class Explosion : Actor
    {
        public Explosion(Texture2D texture, Dictionary<AnimationKey, Animation> animation)
            : base(texture, animation)
        {
            this.NoClip = true;
            this.CurrentAnimation = AnimationKey.Down;
            this.IsAnimating = true;
            this.AnimationCompleted += Explosion_AnimationCompleted;
        }

        protected void Explosion_AnimationCompleted(object sender, EventArgs e)
        {
            //TODO: Add support for enabled property to avoid updating sprites that are disabled
            Sprite explosion = sender as Sprite;
            explosion.Visible = false;
            explosion.IsAnimating = false;
            explosion.AnimationCompleted -= Explosion_AnimationCompleted;
        }
    }
}
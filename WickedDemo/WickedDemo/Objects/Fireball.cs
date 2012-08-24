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
    public class Fireball : Actor
    {
        public Fireball(SceneGraph sceneGraph, Texture2D texture, Dictionary<AnimationKey, Animation> animation)
            : base(texture, animation)
        {
            this.SceneGraph = sceneGraph;
            this.NoClip = false;
            this.IsAnimating = true;
            this.Speed = 4f;
            this.ObjectCollision += new GameObjectCollisionEventHandler(Fireball_ObjectCollision);
        }

        public override void Update()
        {
            this.Tint = Color.White;

            base.Update();
        }

        void Fireball_ObjectCollision(GameObject sender, GameObjectCollisionEventArgs e)
        {
            this.Visible = false;
            this.NoClip = true;
            this.SpawnAtMe(WickedDemo.GameScreens.TestLevel.GetExplosion());
        }
    }
}
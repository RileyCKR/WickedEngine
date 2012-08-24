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
    public class Character : Actor
    {
        public Character(ISceneGraph sceneGraph, Texture2D texture, Dictionary<AnimationKey, Animation> animation)
            : base(texture, animation)
        {
            this.SceneGraph = sceneGraph;
            this.NoClip = false;
            this.ObjectCollision += new GameObjectCollisionEventHandler(Character_ObjectCollision);
        }

        public override void Update()
        {
            this.Tint = Color.White;

            base.Update();
        }

        private void Character_ObjectCollision(GameObject sender, GameObjectCollisionEventArgs e)
        {
            this.Tint = Color.Red;
        }
    }
}
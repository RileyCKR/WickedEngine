using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngine
{
    public class Actor : Sprite
    {
        #region Properties

        //TODO: Need to add AIs Graph for IdleID, WanderAI, CombatAI, etc
        public IObjectAI AI { get; set; }
        public GameObject Target { get; set; }

        #endregion

        #region Constructors

        public Actor()
            : base()
        {
        }

        public Actor(Texture2D texture) : base (texture)
        {
        }

        public Actor(Texture2D texture, Dictionary<AnimationKey, Animation> animation)
            : base (texture, animation)
        {
        }

        #endregion

        public override void Update()
        {
            if (AI != null)
            {
                AI.Update(this);
            }

            base.Update();
        }
    }
}

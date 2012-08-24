using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WickedEngine;
using WickedEngine.Input;

namespace WickedDemo
{
    public class PlayerAI : IObjectAI
    {
        public void Update(Actor caller)
        {
            caller.StopMovement();

            if (InputHelper.KeyDown(Keys.W))
            {
                caller.MoveUp();
            }
            else if (InputHelper.KeyDown(Keys.S))
            {
                caller.MoveDown();
            }

            if (InputHelper.KeyDown(Keys.A))
            {
                caller.MoveLeft();
            }
            else if (InputHelper.KeyDown(Keys.D))
            {
                caller.MoveRight();
            }
        }
    }
}

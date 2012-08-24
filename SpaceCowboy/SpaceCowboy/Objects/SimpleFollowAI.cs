using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WickedEngine;

namespace SpaceCowboy.GameScreens
{
    public class SimpleFollowAI : IObjectAI
    {
        public void Update(Actor caller)
        {
            SpaceShip ship = caller as SpaceShip;
            GameObject target = caller.Target;
            if (target != null)
            {
                Vector2 thrust;
                Vector2 callerPos = caller.AbsolutePosition;
                Vector2 targetPos = target.AbsolutePosition;
                Vector2.Subtract(ref callerPos, ref targetPos, out thrust);

                ship.ApplyThrust(-thrust);
                //if (target.AbsolutePosition.X > caller.AbsolutePosition.X)
                //{
                    
                //}
                //else
                //{
                //    caller.MoveLeft();
                //}

                //if (target.AbsolutePosition.Y < caller.AbsolutePosition.Y)
                //{
                //    caller.MoveUp();
                //}
                //else
                //{
                //    caller.MoveDown();
                //}
            }
        }
    }
}
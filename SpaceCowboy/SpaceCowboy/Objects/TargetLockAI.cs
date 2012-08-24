using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WickedEngine;

namespace SpaceCowboy.Objects
{
    public class TargetLockAI : IObjectAI
    {
        public void Update(Actor caller)
        {
            if (caller.Target != null)
            {
                Vector2 callerPos = caller.AbsolutePosition;
                Vector2 targetPos = caller.Target.AbsolutePosition;

                caller.Rotation = MathFunctions.AngleBetweenPositions(callerPos, targetPos);
            }
        }
    }
}
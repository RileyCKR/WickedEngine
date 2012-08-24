using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;

namespace WickedDemo
{
    public class IdleNpcAI : IObjectAI
    {
        private Random rand = new Random();
        private int CycleCount;
        private int PauseCount;
        private int direction;

        public void Update(Actor caller)
        {
            if (CycleCount < 120)
            {
                ++CycleCount;
                caller.StopMovement();
                switch (direction)
                {
                    case 0:
                        caller.MoveDown();
                        break;
                    case 1:
                        caller.MoveLeft();
                        break;
                    case 2:
                        caller.MoveRight();
                        break;
                    case 3:
                        caller.MoveUp();
                        break;
                }
            }
            else if (PauseCount < 60)
            {
                ++PauseCount;
                caller.StopMovement();
            }
            else
            {
                CycleCount = 0;
                PauseCount = 0;
                direction = rand.Next(0, 3);               
            }
        }
    }
}
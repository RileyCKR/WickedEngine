using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WickedEngine;
using WickedEngine.Input;
using SpaceCowboy.Objects.Weapons;

namespace SpaceCowboy.Objects
{
    class PlayerController : IObjectAI
    {
        const float RotationSpeed = .06f;
        const float EngineThrust = 0.1f;

        public void Update(Actor caller)
        {
            if (InputHelper.KeyDown(Keys.R))
            {
                caller.Position = new Vector2(500, 500);
            }

            SetTarget(caller);
            FireLaser(caller);
            CalculateThrust(caller);
        }

        public void SetTarget(Actor caller)
        {
            if (InputHelper.LeftMouseDown())
            {
                Vector2 screenPos = InputHelper.MousePositionAsVector;
                Vector2 cameraPos = GameScreens.TestLevel.SceneGraph.Camera.Position;
                Vector2 worldPos = screenPos + cameraPos;

                
                List<GameObject> targets = GameScreens.TestLevel.SceneGraph.GetObjectsAtWorldPosition(new Point((int)worldPos.X, (int)worldPos.Y));
                if (targets.Count != 0)
                {
                    caller.Target = targets[0];
                }
                else
                {
                    caller.Target = null;
                }               
            }
        }

        public void FireLaser(Actor caller)
        {
            //TODO: This cast shouldn't be done twice per frame
            SpaceShip ship = caller as SpaceShip;
            if (InputHelper.KeyPressed(Keys.Space))
            {
                foreach (Weapon weapon in ship.Weapons)
                {
                    weapon.BeginFire();
                }
            }

            if (InputHelper.KeyReleased(Keys.Space))
            {
                foreach (Weapon weapon in ship.Weapons)
                {
                    weapon.EndFire();
                }
            }
        }

        public void CalculateThrust(Actor caller)
        {
            SpaceShip ship = caller as SpaceShip;
            if (InputHelper.KeyDown(Keys.W))
            {
                ship.ApplyThrust(-Vector2.UnitY);
            }
            else if (InputHelper.KeyDown(Keys.S))
            {
                ship.ApplyThrust(Vector2.UnitY);
            }

            if (InputHelper.KeyDown(Keys.A))
            {
                ship.ApplyThrust(-Vector2.UnitX);
            }
            else if (InputHelper.KeyDown(Keys.D))
            {
                ship.ApplyThrust(Vector2.UnitX);
            }
        }

        //public void KeepOnMap(GameObject caller)
        //{
        //    Vector2 mapPosition = caller.Position;
        //    if (mapPosition.X < 0f)
        //    {
        //        mapPosition.X = 1024f;
        //    }
        //    else if (mapPosition.X > 1024f)
        //    {
        //        mapPosition.X = 0f;
        //    }

        //    if (mapPosition.Y < 0f)
        //    {
        //        mapPosition.Y = 768f;
        //    }
        //    else if (mapPosition.Y > 768f)
        //    {
        //        mapPosition.Y = 0f;
        //    }
        //    caller.Position = mapPosition;
        //}
    }
}
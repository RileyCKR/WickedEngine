using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngine
{
    public class Camera2D : ICamera
    {
        private float _zoom; // Camera Zoom

        // Sets and gets zoom
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float ZoomIncrement { get; set; }

        public float MaxZoom { get; set; }

        public float MinZoom { get; set; }

        public float Rotation { get; set; }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        public Vector2 Position { get; set; }

        public CameraMode CameraMode { get; set; }

        public GameObject FollowTarget { get; set; }

        private Rectangle _ViewportRectangle;
        public Rectangle ViewportRectangle
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    _ViewportRectangle.Width,
                    _ViewportRectangle.Height);
            }
            private set
            {
                _ViewportRectangle = value;
            }
        }

        public BoundingBox ViewportBounds
        {
            get
            {
                //Get center of camera view
                Vector2 centerOfView = new Vector2(ViewportRectangle.Width / 2, ViewportRectangle.Height / 2);
                centerOfView += Position;

                //Calculate zoomed FOV rectangle
                Rectangle ZoomedView = ViewportRectangle;
                ZoomedView.Width = (int)((float)ZoomedView.Width * (1f / Zoom));
                ZoomedView.Height = (int)((float)ZoomedView.Height * (1f / Zoom));

                //Generate Min and max vectors based off of FOV rectangle and Center of Camera view
                Vector3 Min = new Vector3(centerOfView.X - ZoomedView.Width / 2, centerOfView.Y - ZoomedView.Height / 2, 0f);
                Vector3 Max = new Vector3(centerOfView.X + ZoomedView.Width / 2, centerOfView.Y + ZoomedView.Height / 2, 0f);

                BoundingBox box = new BoundingBox(Min, Max);
                return box;
            }
        }

        public Camera2D(Rectangle viewportRect)
        {
            Zoom = 1.0f;
            Rotation = 0.0f;
            Position = Vector2.Zero;
            ViewportRectangle = viewportRect;
        }

        public virtual void Update(IMap activeMap)
        {
            if (CameraMode == CameraMode.Free)
            {
                LockToMap(activeMap);
            }
            else if (CameraMode == CameraMode.Follow)
            {
                if (FollowTarget != null)
                {
                    LockToObject(FollowTarget, activeMap);
                }
            }
        }

        public virtual void LockToMap(IMap activeMap)
        {
            if (activeMap != null)
            {
                Vector2 position = new Vector2();
                position.X = MathHelper.Clamp(
                    Position.X,
                    0,
                    activeMap.WidthInPixels * Zoom - ViewportRectangle.Width);

                position.Y = MathHelper.Clamp(
                    Position.Y, 0,
                    activeMap.HeightInPixels * Zoom -
                    ViewportRectangle.Height);

                Position = position;
            }
        }

        public virtual void LockToObject(GameObject obj, IMap activeMap)
        {
            Position = obj.AbsolutePosition - new Vector2(ViewportRectangle.Width / 2, ViewportRectangle.Height / 2);

            //position.X = (obj.AbsolutePosition.X + obj.DrawSize.Width / 2) * Zoom
            //                - (ViewportRectangle.Width / 2);
            //position.Y = (obj.AbsolutePosition.Y + obj.DrawSize.Height / 2) * Zoom
            //                - (ViewportRectangle.Height / 2);
            //Position = position;
            //LockToMap(activeMap);
        }

        public void ZoomIn(IMap activeMap)
        {
            Zoom += ZoomIncrement;

            if (Zoom > MaxZoom)
            {
                Zoom = MaxZoom;
            }
        }

        public void ZoomOut(IMap activeMap)
        {
            Zoom -= ZoomIncrement;

            if (Zoom < MinZoom)
            {
                Zoom = MinZoom;
            }
        }

        public void SnapToPosition(Vector2 newPosition, IMap activeMap)
        {
            //Vector2 position = new Vector2();

            //position.X = newPosition.X - ViewportRectangle.Width / 2;
            //position.Y = newPosition.Y - ViewportRectangle.Height / 2;

            //Position = position;

            //LockToMap(activeMap);
        }

        public Matrix CreateTransformation()
        {
            //Origin translation is used seperately from position translation,
            //this is so the camera can be set to 0,0 and be centered on that position
            Vector2 Origin = new Vector2(ViewportRectangle.Width / 2, ViewportRectangle.Height / 2);
            //return Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
            //   Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
            //   Matrix.CreateRotationZ(Rotation) *
            //   Matrix.CreateScale(Zoom, Zoom, 1) *
            //   Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) * 
                Matrix.CreateScale(Zoom, Zoom, 1) * 
                Matrix.CreateTranslation(new Vector3(Origin, 0f));
        }

        public void ToggleCameraMode()
        {
            if (CameraMode == CameraMode.Follow)
                CameraMode = CameraMode.Free;
            else if (CameraMode == CameraMode.Free)
                CameraMode = CameraMode.Follow;
        }
    }
}

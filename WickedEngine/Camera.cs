using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WickedEngine.Input;

namespace WickedEngine
{
    public enum CameraMode { Free, Follow }

    public class Camera : ICamera
    {
        #region Field Region

        private Rectangle _ViewportRectangle;
        private float _Zoom;
        private float _ZoomIncrement = .25f;
        private float _MaxZoom = 2.5f;
        private float _MinZoom = .5f;

        #endregion

        #region Property Region

        public float Zoom
        {
            get { return _Zoom; }
        }

        public float ZoomIncrement
        {
            get { return _ZoomIncrement; }
            set { _ZoomIncrement = value; }
        }

        public float MaxZoom
        {
            get { return _MaxZoom; }
            set { _MaxZoom = value; }
        }

        public float MinZoom
        {
            get { return _MinZoom; }
            set { _MinZoom = value; }
        }

        public Vector2 Position { get; set; }

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

        public CameraMode CameraMode { get; set; }

        public GameObject FollowTarget { get; set; }

        #endregion

        #region Constructor Region

        public Camera(Rectangle viewportRect)
        {
            _Zoom = 1f;
            _ViewportRectangle = viewportRect;
            this.CameraMode = CameraMode.Follow;
        }

        #endregion

        #region Method Region

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
                    activeMap.WidthInPixels * _Zoom - _ViewportRectangle.Width);

                position.Y = MathHelper.Clamp(
                    Position.Y, 0,
                    activeMap.HeightInPixels * _Zoom -
                    _ViewportRectangle.Height);

                Position = position;
            }
        }

        public virtual void LockToObject(GameObject obj, IMap activeMap)
        {
            Vector2 position = new Vector2();
            
            position.X = (obj.Position.X + obj.DrawSize.Width / 2 ) * _Zoom
                            - (_ViewportRectangle.Width / 2);
            position.Y = (obj.Position.Y + obj.DrawSize.Height / 2) * _Zoom
                            - (_ViewportRectangle.Height / 2);
            Position = position;
            LockToMap(activeMap);
        }

        public void ZoomIn(IMap activeMap)
        {
            _Zoom += _ZoomIncrement;

            if (_Zoom > _MaxZoom)
            {
                _Zoom = _MaxZoom;
            }

            RepositionCameraAfterZoom(activeMap);
        }

        public void ZoomOut(IMap activeMap)
        {
            _Zoom -= _ZoomIncrement;

            if (_Zoom < _MinZoom)
            {
                _Zoom = _MinZoom;
            }

            RepositionCameraAfterZoom(activeMap);
        }

        protected void RepositionCameraAfterZoom(IMap activeMap)
        {
            Vector2 newPosition = Position * _Zoom;
            SnapToPosition(newPosition, activeMap);
        }

        public void SnapToPosition(Vector2 newPosition, IMap activeMap)
        {
            Vector2 position = new Vector2();

            position.X = newPosition.X - _ViewportRectangle.Width / 2;
            position.Y = newPosition.Y - _ViewportRectangle.Height / 2;

            Position = position;

            LockToMap(activeMap);
        }

        public Matrix CreateTransformation()
        {
            return Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(new Vector3(-Position, 0f));
        }

        public void ToggleCameraMode()
        {
            if (CameraMode == CameraMode.Follow)
                CameraMode = CameraMode.Free;
            else if (CameraMode == CameraMode.Free)
                CameraMode = CameraMode.Follow;
        }

        #endregion
    }
}
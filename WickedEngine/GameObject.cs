using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngine
{
    public class GameObjectCollisionEventArgs : EventArgs
    {
        public GameObject CollisionObject { get; private set; }

        public GameObjectCollisionEventArgs(GameObject collisionObject)
        {
            this.CollisionObject = collisionObject;
        }
    }

    public delegate void GameObjectCollisionEventHandler(GameObject sender, GameObjectCollisionEventArgs e);

    public class GameObject
    {
        #region Events

        public event GameObjectCollisionEventHandler ObjectCollision;

        #endregion

        #region Fields

        private float _Speed;
        private Vector2 _Position;
        private Vector2 _Velocity;
        private float _Rotation;
        private ReadOnlyCollection<GameObject> _Children;

        #endregion

        #region Properties

        public Vector2 Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        //public Vector2 AbsolutePosition
        //{
        //    get { return _AbsolutePosition; }
        //    set { _AbsolutePosition = value; }
        //}

        public Vector2 AbsolutePosition
        {
            get
            {
                if (Parent != null)
                {
                    Matrix matrix =
                    Matrix.CreateTranslation(new Vector3(this.Position, 0.0f)) *
                    Matrix.CreateRotationZ(Parent.Rotation) *
                    Matrix.CreateTranslation(new Vector3(Parent.AbsolutePosition, 0.0f));
                    Vector3 newPosition = matrix.Translation;
                    return new Vector2(newPosition.X, newPosition.Y);
                }
                else
                {
                    return Position;
                }
            }
        }

        public float Speed
        {
            get { return _Speed; }
            set
            {
                if (value < 0f)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Cannot set speed to a negative value");
                }
                _Speed = value;
            }
        }

        public Vector2 Velocity
        {
            get { return _Velocity; }
            set
            {
                _Velocity = value;
                if (_Velocity != Vector2.Zero)
                {
                    //_Velocity.Normalize();
                }
            }
        }

        public float Rotation
        {
            get { return _Rotation; }
            set 
            {
                _Rotation = value;
                _Rotation = MathHelper.WrapAngle(_Rotation);
            }

        }

        public Vector2 CenterPoint
        {
            get { return new Vector2(DrawSize.Width / 2, DrawSize.Height / 2); }
        }

        public Rectangle CollisionSize { get; protected set; }

        public Rectangle CollisionBounds
        {
            get
            {
                Rectangle bounds = CollisionSize;
                bounds.X = (int)AbsolutePosition.X;// +(int)CenterPoint.X;
                bounds.Y = (int)AbsolutePosition.Y;// +(int)CenterPoint.Y;
                return bounds;
            }
        }

        public Rectangle DrawSize { get; protected set; }

        public BoundingBox DrawBounds()
        {
            Vector3 Min = new Vector3(AbsolutePosition, 0f);
            Vector3 Max = new Vector3(AbsolutePosition, 0f);
            Max.X += DrawSize.Width;
            Max.Y += DrawSize.Height;

            BoundingBox box = new BoundingBox(Min, Max);
            return box;
        }  

        public bool NoClip { get; set; }

        public bool Visible { get; set; }

        public ISceneGraph SceneGraph { get; set; }

        public GameObject Parent { get; set; }

        public ReadOnlyCollection<GameObject> Children
        {
            get
            {
                if (_Children == null)
                {
                    _Children = ChildrenList.AsReadOnly();
                }
                return _Children;
            }
        }

        private List<GameObject> ChildrenList { get; set; }

        #endregion

        #region Constructors

        public GameObject()
        {
            this.NoClip = true;
            this.Visible = true;
            this.Speed = 2f;
            this.ChildrenList = new List<GameObject>();
        }

        #endregion

        #region Collection Methods

        public void AddChild(GameObject obj)
        {
            obj.Parent = this;
            ChildrenList.Add(obj);
        }

        #endregion

        #region Methods

        public void SetDrawSize(int width, int height)
        {
            DrawSize = new Rectangle(0, 0, width, height);
        }

        public void SetCollisionSize(int width, int height)
        {
            CollisionSize = new Rectangle(0, 0, width, height);
        }

        public virtual bool Intersects(GameObject otherObject)
        {
            if (this.CollisionBounds.Intersects(otherObject.CollisionBounds))
            {
                return true;
            }
            else return false;
        }

        public virtual void Update()
        {
            if (Velocity != Vector2.Zero)
            {
                Vector2 newPosition = Position;

                newPosition.X += Velocity.X * Speed;
                newPosition.Y += Velocity.Y * Speed;
                Position = newPosition;
            }
        }

        public virtual void Draw(SpriteBatch batch)
        {
        }

        public virtual void MoveUp()
        {
            Velocity += Constants.VelocityUp;
        }

        public virtual void MoveDown()
        {
            Velocity += Constants.VelocityDown;
        }

        public virtual void MoveLeft()
        {
            Velocity += Constants.VelocityLeft;
        }

        public virtual void MoveRight()
        {
            Velocity += Constants.VelocityRight;
        }

        public virtual void StopMovement()
        {
            Velocity = Vector2.Zero;
        }

        public virtual void OnObjectCollision(GameObject collisionObject)
        {
            if (ObjectCollision != null)
            {
                GameObjectCollisionEventArgs args = new GameObjectCollisionEventArgs(collisionObject);
                ObjectCollision(this, args);
            }
        }

        //TODO: Move to a child class?
        public virtual void SpawnAtMe(GameObject objectToSpawn)
        {
            if (SceneGraph != null)
            {
                objectToSpawn.Position = this.AbsolutePosition;
                SceneGraph.Add(objectToSpawn);
            }
        }

        #endregion
    }
}
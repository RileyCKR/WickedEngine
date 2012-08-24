using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngine
{
    public enum AnimationKey { Down, Left, Right, Up }

    public class Sprite : GameObject
    {
        #region Events

        public event EventHandler AnimationCompleted;

        #endregion

        #region Fields

        Dictionary<AnimationKey, Animation> _Animations;

        #endregion

        #region Properties

        public Texture2D Texture { get; protected set; }

        public Color Tint { get; set; }

        public AnimationKey CurrentAnimation { get; set; }

        public bool IsAnimating { get; set; }

        #endregion

        #region Constructors

        public Sprite()
            : base()
        {
            this.Tint = Color.White;
        }

        public Sprite(Texture2D texture)
            : this()
        {
            this.Texture = texture;
        }

        public Sprite(Texture2D texture, Dictionary<AnimationKey, Animation> animation)
            : this (texture)
        {
            this.Texture = texture;
            this._Animations = new Dictionary<AnimationKey, Animation>();

            foreach (AnimationKey key in animation.Keys)
            {
                _Animations.Add(key, (Animation)animation[key].Clone());
            }

            DrawSize = _Animations[CurrentAnimation].CurrentFrameRect;
        }

        #endregion

        #region Methods

        public override void Update()
        {
            if (IsAnimating && _Animations != null)
            {
                _Animations[CurrentAnimation].Update();
                if (_Animations[CurrentAnimation].Looped)
                {
                    OnAnimationCompleted();
                }
                DrawSize = _Animations[CurrentAnimation].CurrentFrameRect;
            }

            base.Update();
        }

        public override void Draw(SpriteBatch batch)
        {
            if (Visible && Texture != null)
            {
                batch.Draw(
                    Texture,
                    AbsolutePosition,
                    DrawSize,
                    Tint,
                    Rotation,
                    CenterPoint,
                    1.0f,
                    SpriteEffects.None,
                    0f);
            }
        }

        //TODO: Consider changing animation keys on calling set velocity instead of here
        //TODO: Animation key setting should be handled at the level of the derived object?
        public override void MoveUp()
        {
            IsAnimating = true;
            CurrentAnimation = AnimationKey.Up;

            base.MoveUp();
        }

        public override void MoveDown()
        {
            IsAnimating = true;
            CurrentAnimation = AnimationKey.Down;

            base.MoveDown();
        }

        public override void MoveLeft()
        {
            IsAnimating = true;
            CurrentAnimation = AnimationKey.Left;

            base.MoveLeft();
        }

        public override void MoveRight()
        {
            IsAnimating = true;
            CurrentAnimation = AnimationKey.Right;

            base.MoveRight();
        }

        public override void StopMovement()
        {
            IsAnimating = false;

            base.StopMovement();
        }

        protected virtual void OnAnimationCompleted()
        {
            if (AnimationCompleted != null)
            {
                AnimationCompleted(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
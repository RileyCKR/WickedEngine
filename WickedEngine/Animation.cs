using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace WickedEngine
{
    public class Animation : ICloneable
    {
        #region Fields

        private Rectangle[] _Frames;
        private int _FramesPerSecond;
        private int _FrameLength;
        private int _FrameCounter;
        private int _CurrentFrame;
        private int _FrameWidth;
        private int _FrameHeight;

        #endregion

        #region Properties

        public int FramesPerSecond
        {
            get { return _FramesPerSecond; }
            set
            {
                if (value < 1 || value > 60)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Animation frames per second must be between 1 and 60");
                }
                else
                {
                    _FramesPerSecond = value;
                }
                _FrameLength = 60 / _FramesPerSecond;
            }
        }

        public Rectangle CurrentFrameRect
        {
            get { return _Frames[_CurrentFrame]; }
        }
       
        public int CurrentFrame
        {
            get { return _CurrentFrame; }
        }

        public int FrameWidth
        {
            get { return _FrameWidth; }
        }

        public int FrameHeight
        {
            get { return _FrameHeight; }
        }

        public bool Looped { get; private set; }

        #endregion

        #region Constructors

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            this._Frames = new Rectangle[frameCount];
            this._FrameWidth = frameWidth;
            this._FrameHeight = frameHeight;
            FramesPerSecond = 5;

            for (int i = 0; i < frameCount; i++)
            {
                _Frames[i] = new Rectangle(xOffset + (frameWidth * i), yOffset, frameWidth, frameHeight);
            }
            
            Reset();
        }

        private Animation(Animation animation)
        {
            this._Frames = animation._Frames;
        }

        #endregion

        #region Methods

        public void Update()
        {
            ++_FrameCounter;

            if (_FrameCounter >= _FrameLength)
            {
                _FrameCounter = 0;
                IterateAnimation();
            }
        }

        public void Reset()
        {
            _CurrentFrame = 0;
            _FrameCounter = 0;
            Looped = false;
        }

        protected void IterateAnimation()
        {
            ++_CurrentFrame;

            if (_CurrentFrame >= _Frames.Length)
            {
                _CurrentFrame = 0;
                Looped = true;
            }
        }

        #endregion

        #region IClonable

        //TODO: Evaluate necesity of the cloning
        public object Clone()
        {
            Animation animationClone = new Animation(this);
            animationClone._CurrentFrame = this._CurrentFrame;
            animationClone.FramesPerSecond = this._FramesPerSecond;
            animationClone._FrameWidth = this._FrameWidth;
            animationClone._FrameHeight = this._FrameHeight;
            animationClone._FramesPerSecond = this._FramesPerSecond;
            animationClone.Looped = this.Looped;

            return animationClone;
        }

        #endregion
    }
}
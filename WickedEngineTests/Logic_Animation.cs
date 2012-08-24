using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using WickedEngine;

namespace WickedEngineTests
{
    [TestFixture]
    public class Logic_Animation
    {
        [Test]
        public void Default5FPS()
        {
            Animation anim = new Animation(2, 32, 32, 0, 0);
            Assert.That(anim.FramesPerSecond == 5);
        }

        [Test]
        public void Animate()
        {
            Animation anim = new Animation(2, 32, 32, 0, 0);
            anim.FramesPerSecond = 60;
            
            Rectangle frame1 = anim.CurrentFrameRect;
            anim.Update();
            Rectangle frame2 = anim.CurrentFrameRect;

            Assert.That(frame1, Is.Not.EqualTo(frame2));
        }

        [Test]
        public void Loop()
        {
            Animation anim = new Animation(2, 32, 32, 0, 0);
            anim.FramesPerSecond = 60;

            Rectangle frame1 = anim.CurrentFrameRect;
            anim.Update();
            Rectangle frame2 = anim.CurrentFrameRect;
            anim.Update();
            Rectangle frame3 = anim.CurrentFrameRect;
            anim.Update();
            Rectangle frame4 = anim.CurrentFrameRect;

            Assert.That(frame1, Is.EqualTo(frame3));
            Assert.That(frame2, Is.EqualTo(frame4));
        }

        [Test]
        public void Reset()
        {
            Animation anim = new Animation(2, 32, 32, 0, 0);
            anim.FramesPerSecond = 60;
            Rectangle frame1 = anim.CurrentFrameRect;
            anim.Update();
            anim.Reset();
            Rectangle resetFrame = anim.CurrentFrameRect;

            Assert.That(frame1, Is.EqualTo(resetFrame));
        }

        [Test]
        public void Clone()
        {
            Animation anim = new Animation(2, 32, 32, 0, 0);
            anim.FramesPerSecond = 60;

            anim.Update();
            anim.Update();
            anim.Update();

            Animation clone = (Animation)anim.Clone();

            Assert.That(
                (anim.CurrentFrame == clone.CurrentFrame) &&
                (anim.CurrentFrameRect == clone.CurrentFrameRect) &&
                (anim.FrameHeight == clone.FrameHeight) &&
                (anim.FramesPerSecond == clone.FramesPerSecond) &&
                (anim.FrameWidth == clone.FrameWidth) &&
                (anim.Looped == clone.Looped), 
                "Clone did not produce a copy of the Animation.");

            Assert.AreNotSame(anim, clone, "Clone produced a reference copy of the Animation.");
        }

        [Test]
        public void FPSCalculation()
        {
            Animation anim = new Animation(2, 32, 32, 0, 0);
            //30 FPS = New frame every other update
            anim.FramesPerSecond = 30;

            int frame = anim.CurrentFrame;
            anim.Update();
            Assert.That(anim.CurrentFrame, Is.EqualTo(frame));
            anim.Update();
            Assert.That(anim.CurrentFrame, Is.Not.EqualTo(frame));          
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(61)]
        public void FPS_ValidValues(int fps)
        {
            Animation anim = new Animation(2, 32, 32, 0, 0);
            anim.FramesPerSecond = fps;
        }
    }
}
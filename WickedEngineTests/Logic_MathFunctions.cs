using System;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using WickedEngine;

namespace WickedEngineTests
{
    [TestFixture]
    public class Logic_MathFunctions
    {
        [Test]
        public void FloatEquivalence_DefaultPrecision()
        {
            float val1 = 5f;
            float trueVal = 5.000009f;
            float falseVal = 5.00001f;
            Assert.That(MathFunctions.FloatEquivalent(val1, trueVal));
            Assert.That(!MathFunctions.FloatEquivalent(val1, falseVal));       
        }

        [Test]
        public void FloatEquivalence()
        {
            float val1 = 5f;
            float trueVal = 5.009f;
            float falseVal = 5.01f;
            float precision = 0.01f;

            Assert.That(MathFunctions.FloatEquivalent(val1, trueVal, precision));
            Assert.That(!MathFunctions.FloatEquivalent(val1, falseVal, precision));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Float_NegativePrecision()
        {
            float val1 = 5f;
            float val2 = 5f;
            float precision = -0.1f;

            MathFunctions.FloatEquivalent(val1, val2, precision);
        }

        [Test]
        public void AngleBetweenPositions()
        {
            Vector2 pos1 = Vector2.Zero;
            float angle;

            angle = MathFunctions.AngleBetweenPositions(pos1, -Vector2.UnitY);
            Assert.That(angle, Is.EqualTo(0.0f));

            angle = MathFunctions.AngleBetweenPositions(pos1, Vector2.UnitX);
            Assert.That(angle, Is.EqualTo(MathHelper.PiOver2));

            angle = MathFunctions.AngleBetweenPositions(pos1, Vector2.UnitY);
            Assert.That(angle, Is.EqualTo(MathHelper.Pi));

            //NOTE: returns a negative angle for values in this quadrant
            angle = MathFunctions.AngleBetweenPositions(pos1, -Vector2.UnitX);
            Assert.That(angle, Is.EqualTo(-MathHelper.PiOver2));
        }
    }
}
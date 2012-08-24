using System;
using Microsoft.Xna.Framework;

namespace WickedEngine
{
    public static class MathFunctions
    {
        /// <summary>
        /// The default precision for float equivalent functions if none is provided
        /// </summary>
        public const float DefaultPrecision = .00001f;

        /// <summary>
        /// Will return true if two floats are within 1.e-5 of each other
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool FloatEquivalent(float value1, float value2)
        {
            return FloatEquivalent(value1, value2, DefaultPrecision);
        }

        /// <summary>
        /// Will return true if two floats are within the specified precision of each other
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool FloatEquivalent(float value1, float value2, float precision)
        {
            if (precision < 0.0f)
            {
                throw new ArgumentOutOfRangeException("precision", precision, "Precision must be a positive number.");
            }
            float compare = Math.Abs(value1 - value2);
            return compare <= precision;
        }

        public static float AngleBetweenPositions(Vector2 position1, Vector2 position2)
        {
            //float angle = (float)Math.Atan2(callerPos.Y - targetPos.Y, callerPos.X - targetPos.X);
            float angle = (float)Math.Atan2(position2.Y - position1.Y, position2.X - position1.X);
            angle += MathHelper.PiOver2;
            return angle;
        }
    }
}
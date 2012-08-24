using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WickedEngine
{
    public interface ICamera
    {
        void Update(IMap activeMap);

        Rectangle ViewportRectangle { get; }

        BoundingBox ViewportBounds { get; }

        Vector2 Position { get; }

        Matrix CreateTransformation();
    }
}
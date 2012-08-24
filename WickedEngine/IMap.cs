using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngine
{
    public interface IMap
    {
        int WidthInPixels { get; }

        int WidthInTiles { get; }

        int HeightInPixels { get; }

        int HeightInTiles { get; }

        void Draw(SpriteBatch spriteBatch);

        void Draw(SpriteBatch spriteBatch, Rectangle worldArea);
    }
}
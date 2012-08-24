using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngineTests.Mocks
{
    public class TestMap : IMap
    {
        public int WidthInPixels { get { return 1024; } }

        public int WidthInTiles { get { return 1024 / 32; } }

        public int HeightInPixels { get { return 768; } }

        public int HeightInTiles { get { return 768 / 32; } }

        public void Draw(SpriteBatch spriteBatch) { }

        public void Draw(SpriteBatch spriteBatch, Rectangle worldArea) { }
    }
}

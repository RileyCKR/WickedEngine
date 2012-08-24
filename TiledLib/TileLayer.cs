using Microsoft.Xna.Framework.Graphics;
namespace TiledLib
{
    /// <summary>
    /// A map layer containing tiles.
    /// </summary>
    public class TileLayer : Layer
    {
        // The data coming in combines flags for whether the tile is flipped as well as
        // the actual index. These flags are used to first figure out if it's flipped and
        // then to remove those flags and get us the actual ID.
        private const uint FlippedHorizontallyFlag = 0x80000000;
        private const uint FlippedVerticallyFlag = 0x40000000;

        /// <summary>
        /// Gets the layout of tiles on the layer.
        /// </summary>
        public TileGrid Tiles { get; private set; }

        internal TileLayer(string name, int width, int height, float layerDepth, bool visible, float opacity, PropertyCollection properties, Map map, uint[] data, bool makeUnique)
            : base(name, width, height, layerDepth, visible, opacity, properties)
        {
            Tiles = new TileGrid(width, height);

            // data is left-to-right, top-to-bottom
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    uint index = data[y * width + x];

                    // compute the SpriteEffects to apply to this tile
                    SpriteEffects spriteEffects = SpriteEffects.None;
                    if ((index & FlippedHorizontallyFlag) != 0)
                        spriteEffects |= SpriteEffects.FlipHorizontally;
                    if ((index & FlippedVerticallyFlag) != 0)
                        spriteEffects |= SpriteEffects.FlipVertically;

                    // strip out the flip flags to get the real ID
                    int id = (int)(index & ~(FlippedVerticallyFlag | FlippedHorizontallyFlag));

                    // get the tile
                    Tile t = map.Tiles[id];

                    // if the tile is non-null...
                    if (t != null)
                    {
                        // if we want unique instances, clone it
                        if (makeUnique)
                        {
                            t = t.Clone();
                            t.SpriteEffects = spriteEffects;
                        }

                        // otherwise we may need to clone if the tile doesn't have the correct effects
                        // in this world a flipped tile is different than a non-flipped one; just because
                        // they have the same source rect doesn't mean they're equal.
                        else if (t.SpriteEffects != spriteEffects)
                        {
                            t = t.Clone();
                            t.SpriteEffects = spriteEffects;
                        }
                    }

                    // put that tile in our grid
                    Tiles[x, y] = t;
                }
            }
        }
    }
}

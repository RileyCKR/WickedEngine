namespace TiledLib
{
    /// <summary>
    /// An abstract base for a layer in a map.
    /// </summary>
    public abstract class Layer
    {
        /// <summary>
        /// Gets the name of the layer.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the width (in tiles) of the layer.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height (in tiles) of the layer.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets or sets the depth of the layer.
        /// </summary>
        /// <remarks>
        /// This value is passed to SpriteBatch when drawing tiles in this layer and therefore
        /// is only useful if SpriteBatch.Begin is called with SpriteSortMode.FrontToBack or
        /// SpriteSortMode.BackToFront.
        /// 
        /// By default, the Map will set up the layers with LayerDepth values that work for
        /// SpriteSortMode.BackToFront to enable alpha blending to work properly.
        /// </remarks>
        public float LayerDepth { get; set; }

        /// <summary>
        /// Gets or sets the whether the layer is visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the layer.
        /// </summary>
        public float Opacity { get; set; }

        /// <summary>
        /// Gets the list of properties for the layer.
        /// </summary>
        public PropertyCollection Properties { get; private set; }

        internal Layer(string name, int width, int height, float layerDepth, bool visible, float opacity, PropertyCollection properties)
        {
            this.Name = name;
            this.Width = width;
            this.Height = height;
            this.LayerDepth = layerDepth;
            this.Visible = visible;
            this.Opacity = opacity;
            this.Properties = properties;
        }
    }
}

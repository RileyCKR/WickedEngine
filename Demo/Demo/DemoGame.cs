using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledLib;
using System;

namespace Demo
{
	public class DemoGame : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		// a blank 1x1 texture for drawing the box and point
		Texture2D blank;

		// our map
		Map map;

		// some objects in the map
		MapObject box;
		MapObject point;

		// the color to draw our box object
		Color boxColor;

		// one tile from our first tile layer
		Tile tile;

		public DemoGame()
		{
			graphics = new GraphicsDeviceManager(this);

			// on the phone, we want to be fullscreen and we're locked to 30Hz
#if WINDOWS_PHONE
			graphics.IsFullScreen = true;
			TargetElapsedTime = TimeSpan.FromTicks(333333);
#endif

			Content.RootDirectory = "Content";
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			blank = new Texture2D(GraphicsDevice, 1, 1);
			blank.SetData(new[] { Color.White });

			// load the map
			map = Content.Load<Map>("Map");

			// find the "Box" and "Point" objects we made in the level
			MapObjectLayer objects = map.GetLayer("Objects") as MapObjectLayer;
			point = objects.GetObject("Point");
			box = objects.GetObject("Box");

			// attempt to read the box color from the box object properties
			try
			{
				boxColor.R = (byte)box.Properties["R"];
				boxColor.G = (byte)box.Properties["G"];
				boxColor.B = (byte)box.Properties["B"];
				boxColor.A = 255;
			}
			catch
			{
				// on failure, default to yellow
				boxColor = Color.Yellow;
			}

			// find one tile from our tile layer
			TileLayer tileLayer = map.GetLayer("Tiles") as TileLayer;
			tile = tileLayer.Tiles[0, 0];
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			// modulate one tile's alpha which will show off the difference
			// between the two values for the TmxProcessor's MakeTilesUnique 
			// property.
			tile.Color = Color.White * (float)Math.Abs(Math.Cos(gameTime.TotalGameTime.TotalSeconds));

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			// draw the map using the basic, built in drawing
			map.Draw(spriteBatch);

			// fill in our box object
			spriteBatch.Draw(blank, box.Bounds, boxColor);

			// draw a box around our point
			spriteBatch.Draw(blank, new Rectangle(point.Bounds.X - 5, point.Bounds.Y - 5, 10, 10), Color.Red);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

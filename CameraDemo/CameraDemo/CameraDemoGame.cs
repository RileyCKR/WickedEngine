using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledLib;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace CameraDemo
{
	public class CameraDemoGame : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		// position of the camera
		Vector2 camera = Vector2.Zero;

		// the speed of camera movement; used on phone to let us "flick" the camera
		Vector2 cameraVelocity = Vector2.Zero;

		// a blank 1x1 texture for drawing the box and point
		Texture2D blank;

		// our map
		Map map;

		// some objects in the map
		MapObject box;
		MapObject point;

		// the color to draw our box object
		Color boxColor;

		// change to see the effects of each drawing method
		// if set to true, we use the efficient Draw(SpriteBatch, Rectangle) method.
		// if set to false, we use the slower Draw(SpriteBatch) method.
		bool useEfficientDrawing = true;

		public CameraDemoGame()
		{
			graphics = new GraphicsDeviceManager(this);

			// on the phone, we want to be fullscreen and we're locked to 30Hz
#if WINDOWS_PHONE
			graphics.IsFullScreen = true;
			TargetElapsedTime = TimeSpan.FromTicks(333333);
#endif

			Content.RootDirectory = "Content";

			// we are using a frame rate counter to compare the performance of the drawing
			Components.Add(new FrameRateComponent(this));

			// utilize drag and flick to move the camera
			TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.Flick;
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			blank = new Texture2D(GraphicsDevice, 1, 1);
			blank.SetData(new[] { Color.White });

			// load the map. the map for this demo is 999x999 or 998,001 tiles to showcase
			// the efficient Draw(SpriteBatch, Rectangle) method.
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
		}

		protected override void Update(GameTime gameTime)
		{
			// get the state of a gamepad and the keyboard
			GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
			KeyboardState keyboardState = Keyboard.GetState();

			// exit from the back button or escape key
			if (gamePadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
				Exit();

#if WINDOWS_PHONE
			// if we have a finger on the screen, set the velocity to 0
			if (TouchPanel.GetState().Count > 0)
			{
				cameraVelocity = Vector2.Zero;
			}

			// update our camera with the velocity
			MoveCamera(cameraVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

			// apply some friction to the camera velocity
			cameraVelocity *= 1f - (.95f * (float)gameTime.ElapsedGameTime.TotalSeconds);

			while (TouchPanel.IsGestureAvailable)
			{
				GestureSample gesture = TouchPanel.ReadGesture();

				// just move the camera if we have a drag
				if (gesture.GestureType == GestureType.FreeDrag)
				{
					MoveCamera(-gesture.Delta);
				}

				// set our velocity if we see a flick
				else if (gesture.GestureType == GestureType.Flick)
				{
					cameraVelocity = -gesture.Delta;
				}
			}
#else
			// update the camera
			UpdateCamera(gamePadState, keyboardState);
#endif

			base.Update(gameTime);
		}

		private void UpdateCamera(GamePadState gamePadState, KeyboardState keyboardState)
		{
			Vector2 cameraMovement = Vector2.Zero;

			// if a gamepad is connected, use the left thumbstick to move the camera
			if (gamePadState.IsConnected)
			{
				cameraMovement.X = gamePadState.ThumbSticks.Left.X;
				cameraMovement.Y = -gamePadState.ThumbSticks.Left.Y;
			}
			else
			{
				// otherwise we use the arrow keys
				if (keyboardState.IsKeyDown(Keys.Left))
					cameraMovement.X = -1;
				else if (keyboardState.IsKeyDown(Keys.Right))
					cameraMovement.X = 1;
				if (keyboardState.IsKeyDown(Keys.Up))
					cameraMovement.Y = -1;
				else if (keyboardState.IsKeyDown(Keys.Down))
					cameraMovement.Y = 1;

				// to match the thumbstick behavior, we need to normalize non-zero vectors in case the user
				// is pressing a diagonal direction.
				if (cameraMovement != Vector2.Zero)
					cameraMovement.Normalize();
			}

			// scale our movement to move 25 pixels per second
			cameraMovement *= 25f;

			// move the camera
			MoveCamera(cameraMovement);
		}

		private void MoveCamera(Vector2 cameraMovement)
		{
			camera += cameraMovement;

			// clamp the camera so it never leaves the visible area of the map. we
			Vector2 cameraMax = new Vector2(
				map.Width * map.TileWidth - GraphicsDevice.Viewport.Width,
				map.Height * map.TileHeight - GraphicsDevice.Viewport.Height);
			camera = Vector2.Clamp(camera, Vector2.Zero, cameraMax);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// create a matrix for the camera to offset everything we draw, the map and our objects. since the
			// camera coordinates are where the camera is, we offset everything by the negative of that to simulate
			// a camera moving. we also cast to integers to avoid filtering artifacts
			Matrix cameraMatrix = Matrix.CreateTranslation(-(int)camera.X, -(int)camera.Y, 0);

			// we use 0 and null to indicate that we just want the default values
			spriteBatch.Begin(0, null, null, null, null, null, cameraMatrix);

			if (useEfficientDrawing)
			{
				// if using the efficient drawing, we use an overload of Draw that takes in the area to
				// draw in order to compensate for large maps that only need to draw what's on screen.
				// our visible area is computed using the camera and the viewport size.
				Rectangle visibleArea = new Rectangle(
					(int)camera.X,
					(int)camera.Y,
					GraphicsDevice.Viewport.Width,
					GraphicsDevice.Viewport.Height);
				map.Draw(spriteBatch, visibleArea);
			}
			else
			{
				// if not using the efficient drawing, just call Draw to draw the whole map. this is substantially
				// slower for such a large map.
				map.Draw(spriteBatch);
			}

			// fill in our box object
			spriteBatch.Draw(blank, box.Bounds, boxColor);

			// draw a box around our point
			spriteBatch.Draw(blank, new Rectangle(point.Bounds.X - 5, point.Bounds.Y - 5, 10, 10), Color.Red);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

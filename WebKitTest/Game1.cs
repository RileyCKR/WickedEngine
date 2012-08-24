using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Threading;
using System.Diagnostics;
using WickedEngine.Input;
using WickedEngine.GUI;

namespace WebKitTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game, IGameObject
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch SpriteBatch { get; set; }

        private WindowManager windowManager;
        private Point mousePoint;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true; 

            Components.Add(new InputHelper(this));

            windowManager = new WindowManager(this);
            
            Components.Add(windowManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            Vector2 position = new Vector2(50f, 50f);
            Window window = new Window(this, position, 200, 200);
            windowManager.AddWindow(window);
            Window window2 = new Window(this, new Vector2(300,100), 300, 300);
            windowManager.AddWindow(window2);

            mousePoint = new Point(0, 0);       
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Shut down Awesomium before exiting.
            if (windowManager != null)
            {
                windowManager.Dispose();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Window window = windowManager.GetActiveWindow();

            if (window != null)
            {
                if (InputHelper.KeyPressed(Keys.Up))
                {
                    window.ScreenPosition -= Vector2.UnitY * 5;
                }
                if (InputHelper.KeyPressed(Keys.Down))
                {
                    window.ScreenPosition += Vector2.UnitY * 5;
                }
                if (InputHelper.KeyPressed(Keys.Left))
                {
                    window.ScreenPosition -= Vector2.UnitX * 5;
                }
                if (InputHelper.KeyPressed(Keys.Right))
                {
                    window.ScreenPosition += Vector2.UnitX * 5;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}

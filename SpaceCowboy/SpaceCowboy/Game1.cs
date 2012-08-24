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
using SpaceCowboy.GameScreens;
using WickedEngine;
using WickedEngine.Components;
using WickedEngine.Input;

namespace SpaceCowboy
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Member Variables

        GraphicsDeviceManager graphics;

        private GameStateManager _StateManager;

        private int _ScreenWidth = 1024;
        private int _ScreenHeight = 768;
        private Rectangle _ScreenRectangle;
        private SpriteBatch _SpriteBatch;

        #endregion

        #region Properties

        public GameStateManager StateManager
        {
            get { return _StateManager; }
            set { _StateManager = value; }
        }

        public Rectangle ScreenRectangle
        {
            get { return _ScreenRectangle; }
            set { _ScreenRectangle = value; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return _SpriteBatch; }
            set { _SpriteBatch = value; }
        }

        #endregion

        public TestLevel TestLevel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = _ScreenWidth;
            graphics.PreferredBackBufferHeight = _ScreenHeight;

            //Uncomment the next two lines to turn off VSync for performance testing
            //graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;

            Content.RootDirectory = "Content";

            _ScreenRectangle = new Rectangle(0, 0, _ScreenWidth, _ScreenHeight);

            Components.Add(new InputHelper(this));
            StateManager = new GameStateManager(this);
            Components.Add(StateManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Create a new SpriteBatch, which can be used to draw textures.
            _SpriteBatch = new SpriteBatch(GraphicsDevice);

            TestLevel = new TestLevel(this, StateManager, SpriteBatch, ScreenRectangle);
            StateManager.ChangeState(TestLevel);
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}

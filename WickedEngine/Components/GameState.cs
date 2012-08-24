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

namespace WickedEngine.Components
{
    //TODO: This class is not intuitive, needs work
    public abstract class GameState : DrawableGameComponent
    {
        #region Fields

        private Game _GameRef;
        private List<GameComponent> _ChildComponents;
        private GameStateManager _StateManager;
        private GameState _Tag;

        #endregion

        #region Properties

        public ICollection<GameComponent> ChildComponents { get { return _ChildComponents; } }

        public GameState Tag { get { return _Tag; } }

        protected GameStateManager StateManager
        {
            get { return _StateManager; }
            set { _StateManager = value; }
        }

        protected Game GameRef
        {
            get { return _GameRef; }
            set { _GameRef = value; }
        }

        public SpriteBatch SpriteBatch { get; protected set; }

        public Rectangle ScreenRectangle { get; set; }

        #endregion


        #region Constructor Region

        protected GameState(Game game, GameStateManager manager, SpriteBatch spriteBatch, Rectangle screenRectangle)
            : base(game)
        {
            _StateManager = manager;
            _ChildComponents = new List<GameComponent>();
            _Tag = this;
            _GameRef = game;
            this.SpriteBatch = spriteBatch;
            this.ScreenRectangle = screenRectangle;
        }
        
        #endregion

        #region XNA Drawable Game Component Methods

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in _ChildComponents)
            {
                if (component.Enabled)
                    component.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent drawComponent;
            foreach (GameComponent component in _ChildComponents)
            {
                drawComponent = component as DrawableGameComponent;
                if (drawComponent != null && drawComponent.Visible)
                {
                    drawComponent.Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }

        #endregion

        #region GameState Method Region

        internal protected virtual void StateChange(object sender, EventArgs e)
        {
            if (_StateManager.CurrentState == Tag)
                Show();
            else
                Hide();
        }

        protected virtual void Show()
        {
            Visible = true;
            Enabled = true;

            DrawableGameComponent drawComponent;
            foreach (GameComponent component in _ChildComponents)
            {
                component.Enabled = true;

                drawComponent = component as DrawableGameComponent;
                if (drawComponent != null)
                {
                    drawComponent.Visible = true;
                }
            }
        }

        protected virtual void Hide()
        {
            Visible = false;
            Enabled = false;

            DrawableGameComponent drawComponent;
            foreach (GameComponent component in _ChildComponents)
            {
                component.Enabled = false;

                drawComponent = component as DrawableGameComponent;
                if (drawComponent != null)
                {
                    drawComponent.Visible = false;
                }
            }
        }

        #endregion
    }
}
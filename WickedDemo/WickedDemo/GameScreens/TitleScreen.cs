using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WickedEngine;
using WickedEngine.Components;

namespace WickedDemo.GameScreens
{
    public class TitleScreen : GameState
    {
        Texture2D backgroundImage;

        #region Constructor region

        public TitleScreen(Game game, GameStateManager manager, SpriteBatch spriteBatch, Rectangle screenRectangle)
            : base(game, manager, spriteBatch, screenRectangle)
        {
        }

        #endregion

        #region XNA Method region

        protected override void LoadContent()
        {
            ContentManager Content = GameRef.Content;
            backgroundImage = Content.Load<Texture2D>(@"Backgrounds\titlescreen");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            base.Draw(gameTime);
            SpriteBatch.Draw(
                backgroundImage, ScreenRectangle,
                Color.White);
            SpriteBatch.End();
        }

        #endregion
    }
}

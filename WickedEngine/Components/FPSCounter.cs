using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngine.Components
{
    //TODO: Should FPSCounter really be a gamestate?
    public class FPSCounter : GameState
    {
        public Vector2 Position { get; set; }
        public Vector2 Position2 { get; set; }
        public Color ColorNormal { get; set; }
        public Color ColorSlow { get; set; }
        public Color ColorShadow { get; set; }
        public SpriteFont Font { get; set; }

        private int frameRate = 0;
        private int frameCounter = 0;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private TimeSpan updateInterval = TimeSpan.FromMilliseconds(100);

        public FPSCounter(Game game, GameStateManager manager, SpriteBatch spriteBatch, Rectangle screenRectangle, Vector2 position, Color colorNormal, Color colorSlow, Color colorShadow)
            : base(game, manager, spriteBatch, screenRectangle)
        {
            Position = position;
            Position2 = new Vector2(Position.X + 1, Position.Y + 1);
            ColorNormal = colorNormal;
            ColorSlow = colorSlow;
            ColorShadow = colorShadow;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SetFPS(gameTime.ElapsedGameTime);

            string text = "FPS: " + frameRate;

            SpriteBatch.Begin();

            SpriteBatch.DrawString(Font, text, Position2, ColorShadow);

            if (gameTime.IsRunningSlowly)
            {           
                SpriteBatch.DrawString(Font, text, Position, ColorSlow);
            }
            else
            {
                SpriteBatch.DrawString(Font, text, Position, ColorNormal);
            }
            
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetFPS(TimeSpan elapsedGameTime)
        {
            elapsedTime += elapsedGameTime;

            if (elapsedTime > updateInterval)
            {
                frameRate = frameCounter * (1000 / elapsedTime.Milliseconds);
                elapsedTime = TimeSpan.Zero;
                frameCounter = 0;
            }

            frameCounter++;
        }
    }
}

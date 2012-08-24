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

using Awesomium.Core;
using AwesomiumSharpXna;

namespace WickedEngine.GUI
{
    public class Window : DrawableGameComponent
    {
        private WebView webView;
        private IGameObject _GameRef;

        public string Title { get; set; }
        public bool IsFixed { get; set; }
        public bool ShowBorders { get; set; }
        public bool FinishedLoading { get; private set; }
        public Texture2D AwesomeTexture { get; set; }
        public Vector2 ScreenPosition { get; set; }
        public Rectangle WindowSize { get; set; }
        public Rectangle ScreenRectangle
        {
            get
            {
                return new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, WindowSize.Width, WindowSize.Height);
            }
        }

         public Window(Game game, Vector2 position, int width, int height)
            : base(game)
        {
            _GameRef = (IGameObject)game;
            ScreenPosition = position;
            WindowSize = new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            webView = WebCore.CreateWebView(WindowSize.Width, WindowSize.Height);
            webView.LoadCompleted += OnFinishLoading;

            WebCore.BaseDirectory = Environment.CurrentDirectory;
            
            string path = @"\test.htm";
            if (!webView.LoadFile(path))
            {
                throw new ApplicationException("Error loading html file.");
            }

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void OnFinishLoading(object sender, EventArgs e)
        {
            FinishedLoading = true;

            webView.CreateObject("Application");
            webView.SetObjectCallback("Application", "Test", myCallBack);
        }

        private void myCallBack(object sender, JSCallbackEventArgs e)
        {
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            _GameRef.SpriteBatch.Begin();

            if (FinishedLoading)
            {
                if (webView.IsDirty)
                {
                    RenderBuffer buff = webView.Render();
                    AwesomeTexture = new Texture2D(GraphicsDevice, buff.Width, buff.Height);
                    buff.RenderTexture2D(AwesomeTexture);
                }

                _GameRef.SpriteBatch.Draw(AwesomeTexture, ScreenPosition, WindowSize, Color.White);
            }

            _GameRef.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void Focus()
        {
            webView.Focus();
        }

        public void InjectKeyboardEvent(Keys key)
        {
            //TODO: add support for key down and keyup state as well as non-character keys
            char keyChar = (char)key;
            WebKeyboardEvent keyDown = new WebKeyboardEvent();
            keyDown.Type = WebKeyType.Char;

            keyDown.Text = new ushort[] { keyChar, 0, 0, 0 };

            webView.InjectKeyboardEvent(keyDown);
        }

        public void InjectMouseMove(int X, int Y)
        {
            webView.InjectMouseMove(X - (int)ScreenPosition.X, Y - (int)ScreenPosition.Y);
        }

        public void InjectMouseDown(bool leftButton)
        {
            if (leftButton)
            {
                webView.InjectMouseDown(MouseButton.Left);
            }
            else
            {
                webView.InjectMouseDown(MouseButton.Right);
            }
        }

        public void InjectMouseUp(bool leftButton)
        {
            if (leftButton)
            {
                webView.InjectMouseUp(MouseButton.Left);
            }
            else
            {
                webView.InjectMouseUp(MouseButton.Right);
            }
        }
    }
}

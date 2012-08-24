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

using System.Collections.ObjectModel;
using WickedEngine.Input;
using Awesomium.Core;

namespace WickedEngine.GUI
{
    //TODO: Convert to Singleton
    public class WindowManager : DrawableGameComponent, IDisposable
    {
        #region Fields

        private List<Window> _Windows;

        #endregion

        #region Properties

        public IEnumerable<Window> Windows
        {
            get { return _Windows; }
        }

        #endregion

        #region Constructors

        public WindowManager(Game game)
            : base(game)
        {
            _Windows = new List<Window>();
        }

        #endregion

        #region XNA Methods

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            if (!WebCore.IsRunning)
            {
                // We demonstrate an easy way to hide the scrollbars by providing
                // custom CSS. Read more about how to style the scrollbars here:
                // http://www.webkit.org/blog/363/styling-scrollbars/.
                // Just consider that this setting is global. If you want to apply
                // a similar effect for single pages, you can use ExecuteJavascript
                // and pass: document.documentElement.style.overflow = 'hidden';
                // (Unfortunately WsebKit's scrollbar does not have a DOM equivalent yet)
                WebCore.Initialize(new WebCoreConfig() { CustomCSS = "::-webkit-scrollbar { visibility: hidden; }" });
            }

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Window window = this.GetWindowAtCoords(InputHelper.MousePosition);

            if (window != null && window.FinishedLoading)
            {
                window.Focus();

                Keys[] keys = InputHelper.KeyboardState.GetPressedKeys();

                foreach (Keys thisKey in keys)
                {
                    if (InputHelper.LastKeyboardState.IsKeyUp(thisKey))
                    {
                        window.InjectKeyboardEvent(thisKey);
                    }
                }

                window.InjectMouseMove(InputHelper.MousePosition.X, InputHelper.MousePosition.Y);

                if (InputHelper.MouseState.LeftButton == ButtonState.Pressed && InputHelper.LastMouseState.LeftButton != ButtonState.Pressed)
                {
                    window.InjectMouseDown(true);
                }
                else if (InputHelper.MouseState.LeftButton == ButtonState.Released && InputHelper.LastMouseState.LeftButton != ButtonState.Released)
                {
                    window.InjectMouseUp(true);
                }
            }

            foreach (Window component in _Windows)
            {
                if (component.Enabled)
                    component.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Window component in _Windows)
            {
                if (component.Visible)
                    component.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        #endregion

        #region Methods

        public void AddWindow(Window window)
        {
            _Windows.Add(window);
            window.Initialize();
        }

        public Window GetWindowAtCoords(Point point)
        {
            //TODO: Correct layering problem so top window gets preference
            foreach (Window component in _Windows)
            {
                if (component.ScreenRectangle.Contains(point))
                {
                    return component;
                }
            }

            return null;
        }

        public Window GetActiveWindow()
        {
            //TODO: Update this implementation to not be identical to GetWindowAtCoords
            foreach (Window component in _Windows)
            {
                if (component.ScreenRectangle.Contains(InputHelper.MousePosition))
                {
                    return component;
                }
            }

            return null;
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            // Shut down Awesomium before exiting.
            WebCore.Shutdown();

            base.Dispose(disposing);
        }

        #endregion
    }
}
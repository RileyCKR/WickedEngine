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

namespace WickedEngine.Input
{
    public class InputHelper : GameComponent
    {
        #region Fields

        private static KeyboardState _KeyboardState;
        private static KeyboardState _LastKeyboardState;
        private static MouseState _MouseState;
        private static MouseState _LastMouseState;
        private static Point _MousePosition = new Point(0, 0);

        #endregion

        #region Properties

        public static KeyboardState KeyboardState { get { return _KeyboardState; } }
        
        public static KeyboardState LastKeyboardState { get { return _LastKeyboardState; } }
        
        public static MouseState MouseState { get { return _MouseState; } }
        
        public static MouseState LastMouseState { get { return _LastMouseState; } }

        public static Point MousePosition
        {
            get { return _MousePosition; }
        }

        public static Vector2 MousePositionAsVector { get; private set; }

        #endregion

        #region Constructors

        public InputHelper(Game game)
            : base(game)
        {
            _KeyboardState = Keyboard.GetState();
            _LastKeyboardState = _KeyboardState;
        }

        #endregion

        #region GameComponent Methods

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _LastKeyboardState = _KeyboardState;
            _KeyboardState = Keyboard.GetState();

            _LastMouseState = _MouseState;
            _MouseState = Mouse.GetState();

            _MousePosition.X = _MouseState.X;
            _MousePosition.Y = _MouseState.Y;

            MousePositionAsVector = new Vector2(_MouseState.X, _MouseState.Y);

            base.Update(gameTime);
        }

        #endregion

        #region Methods

        public static bool KeyReleased(Keys key)
        {
            return _KeyboardState.IsKeyUp(key) &&
                _LastKeyboardState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return _KeyboardState.IsKeyDown(key) &&
                _LastKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return _KeyboardState.IsKeyDown(key);
        }

        public static bool LeftMouseDown()
        {
            return MouseState.LeftButton == ButtonState.Pressed && 
                LastMouseState.LeftButton == ButtonState.Released;
        }

        public static bool LeftMouseUp()
        {
            return MouseState.LeftButton == ButtonState.Released && 
                LastMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool RightMouseDown()
        {
            return MouseState.RightButton == ButtonState.Pressed &&
                LastMouseState.RightButton == ButtonState.Released;
        }

        public static bool RightMouseUp()
        {
            return MouseState.RightButton == ButtonState.Released &&
                LastMouseState.RightButton == ButtonState.Pressed;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeteorFreeze.Gui
{
    /// <summary>
    /// An enumeration of supported mouse buttons, because XNA doesn't have this.
    /// </summary>
    public enum MouseButton
    {
        LeftButton,
        MiddleButton,
        RightButton
    }

    /// <summary>
    /// The delegate callback for button click events.
    /// </summary>
    /// <param name="button">The button control that was clicked</param>
    /// <param name="mouseButton">The mouse button that was clicked.</param>
    public delegate void ButtonClickHandler(Button button, MouseButton mouseButton);

    /// <summary>
    /// The implementation for button controls.
    /// </summary>
    public class Button : GameControl
    {
        private MouseState oldMouseState;
        private Texture2D texUp;
        private Texture2D texHover;
        private Texture2D texDown;
        private Texture2D texCurrent;
        private string text;
        private SpriteFont font;
        private Color color;
        /// <summary>
        /// The event that gets raised whenever this button control is clicked.
        /// </summary>
        public event ButtonClickHandler Click;

        /// <summary>
        /// Creates a new button control.
        /// </summary>
        /// <param name="game">The game this button belongs to.</param>
        /// <param name="groupName">The name of the button's group.</param>
        /// <param name="x">The X coordinate of the button.</param>
        /// <param name="y">The X coordinate of the button.</param>
        /// <param name="width">The width of the button.</param>
        /// <param name="height">The height of the button.</param>
        public Button(Game game, GuiManager manager, string text, float x, float y, float width, float height)
            : base(game, manager)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(width, height);

            font = game.Content.Load<SpriteFont>("SpriteFont1");
            this.text = text;

            texUp = Game.Content.Load<Texture2D>("Gui/Button-up");
            texHover = Game.Content.Load<Texture2D>("Gui/Button-hover");
            texDown = Game.Content.Load<Texture2D>("Gui/Button-down");
          
        }

        /// <summary>
        /// Updates the button.
        /// </summary>
        /// <param name="gameTime">Frame time information.</param>
        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();
            Point mouseLoc = new Point(newMouseState.X, newMouseState.Y);

            texCurrent = texUp;
            color = Color.White;
            // only check for click event if the mouse is over us
            if (this.Bounds.Contains(mouseLoc))
            {
                // check if we need the hover or down textures
                if (newMouseState.LeftButton == ButtonState.Pressed ||
                     newMouseState.MiddleButton == ButtonState.Pressed ||
                     newMouseState.RightButton == ButtonState.Pressed)
                {
                    color = Color.Red;
                    texCurrent = texDown;
                }
                else
                {
                    texCurrent = texHover;
                }

                // check for left click
                if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (Click != null)
                    {
                        Click(this, MouseButton.LeftButton);
                    }
                }

                // check for middle click
                if (newMouseState.MiddleButton == ButtonState.Released && oldMouseState.MiddleButton == ButtonState.Pressed)
                {
                    if (Click != null)
                    {
                        Click(this, MouseButton.MiddleButton);
                    }
                }

                // check for right click
                if (newMouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed)
                {
                    if (Click != null)
                    {
                        Click(this, MouseButton.RightButton);
                    }
                }
            }

            oldMouseState = newMouseState;
        }

        /// <summary>
        /// Draws the button.
        /// </summary>
        /// <param name="gameTime">Frame time information.</param>
        /// <param name="spriteBatch">The sprite batch to use to draw.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (texCurrent != null)
            {
                spriteBatch.Draw(texCurrent, Bounds, color);
                spriteBatch.DrawString(font, text, new Vector2(bounds.X + bounds.Width / 3, bounds.Y + bounds.Height / 4), Color.Black);
            }
        }
    }
}
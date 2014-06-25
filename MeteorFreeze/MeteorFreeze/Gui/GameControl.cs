using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeteorFreeze.Gui
{
    /// <summary>
    /// The base class for game controls.
    /// </summary>
    public abstract class GameControl
    {
        private static int controlCount = 0;

        protected Game game;
        protected Vector2 position;
        protected Vector2 size;
        protected Color color;
        protected Rectangle bounds;
        protected string name;
        protected GuiManager manager;

        /// <summary>
        /// Gets the game this control belongs to.
        /// </summary>
        public Game Game
        {
            get
            {
                return game;
            }
        }

        /// <summary>
        /// Gets or sets the position of this control.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                CalculateBounds();
            }
        }

        /// <summary>
        /// Gets or sets the size of this control.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                CalculateBounds();
            }
        }

        /// <summary>
        /// Gets or sets the color of this control.
        /// </summary>
        public virtual Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        /// <summary>
        /// Gets the bounds of this control.
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }
        }

        /// <summary>
        /// Gets or sets the name of this control.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                manager.RemoveControl(name);
                name = value;
                manager[name] = this;
            }
        }

        /// <summary>
        /// Creates a new game control.
        /// </summary>
        /// <param name="game">The game this control belongs to.</param>
        public GameControl(Game game, GuiManager manager)
        {
            this.game = game;
            position = new Vector2();
            size = new Vector2();
            color = Color.White;
            CalculateBounds();

            // provide default names
            this.manager = manager;
            name = "control" + ++controlCount;
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        /// <param name="gameTime">Frame time values.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the control.
        /// </summary>
        /// <param name="gameTime">Frame time values.</param>
        /// <param name="spriteBatch">The sprite batch to use when drawing.</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Calculates the bounds of the control.
        /// </summary>
        protected virtual void CalculateBounds()
        {
            bounds = new Rectangle
            (
                (int)Math.Round(position.X),
                (int)Math.Round(position.Y),
                (int)Math.Round(size.X),
                (int)Math.Round(size.Y)
            );
        }
    }
}

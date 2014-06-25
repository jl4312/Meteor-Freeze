using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeteorFreeze.Gui
{
    /// <summary>
    /// The implementation for label controls.
    /// </summary>
    public class Label : GameControl
    {
        private SpriteFont spriteFont;
        private string text;

        /// <summary>
        /// Gets or sets the text of this label.
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Creates a new label.
        /// </summary>
        /// <param name="game">The game this label belongs to.</param>
        /// <param name="font">The font to use with the label.</param>
        /// <param name="text">The label's text.</param>
        public Label(Game game, GuiManager manager, SpriteFont font, string text = "")
            : base(game, manager)
        {
            spriteFont = font;
            this.text = text ?? "";
        }

        /// <summary>
        /// Updates the label.
        /// </summary>
        /// <param name="gameTime">Frame time information.</param>
        public override void Update(GameTime gameTime)
        {
            // do ~nothing~
        }

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="gameTime">Frame time information.</param>
        /// <param name="spriteBatch">The sprite batch to use to draw.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, text, Position, Color);
        }
    }
}
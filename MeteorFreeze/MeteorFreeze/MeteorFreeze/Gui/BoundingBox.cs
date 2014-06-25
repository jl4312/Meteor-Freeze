using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MeteorFreeze.Gui;

namespace MeteorFreeze.Gui
{
    class BoundingBox : GameControl
    {
        private Texture2D image;
        private Picture characterIcon;


        /// <summary>
        /// Gets or sets the texture image in use by this picture box.
        /// </summary>
        public Texture2D Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }

        public Picture CharacterIcon
        {
            get { return characterIcon; }
            set { characterIcon = value; }
        }

        public Rectangle MidPoint
        {

            get { return new Rectangle(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2, Bounds.Width, Bounds.Height); }
        }
        /// <summary>
        /// Creates a new picture box.
        /// </summary>
        /// <param name="game">The game this control belongs to.</param>
        /// <param name="image">The image.</param>
        public BoundingBox(Game game, GuiManager manager)
            : base(game, manager)
        {
            this.image = game.Content.Load<Texture2D>("Graphics/GUI/BoundingBox");
            characterIcon = null;
        }

        /// <summary>
        /// Updates the picture box.
        /// </summary>
        /// <param name="gameTime">Frame time information.</param>
        public override void Update(GameTime gameTime)
        {
            // do ~nothing~
        }

        /// <summary>
        /// Draws the picture box.
        /// </summary>
        /// <param name="gameTime">Frame time information.</param>
        /// <param name="spriteBatch">The sprite batch to draw with.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle bounds = Bounds;
            spriteBatch.Draw(image, bounds, Color);
        }


    }
}

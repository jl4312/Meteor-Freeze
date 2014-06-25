using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeteorFreeze.Gui
{
    /// <summary>
    /// The implementation for picture box controls.
    /// </summary>
    public class Picture : GameControl
    {
        private Texture2D image;
        private float rotation;

        private Boolean movable;
        private Boolean visible;
        Vector2 permanentPostion;


        /// <summary>
        /// Gets or sets the rotation angle (in radians) of the picture box.
        /// </summary>
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

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

        public Vector2 PermanentPosition
        {
            get { return permanentPostion; }
            set { permanentPostion = value; }
        }
        public Boolean Movable
        {
            get { return movable; }
            set { movable = value; }
        }
        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }


        /// <summary>
        /// Creates a new picture box.
        /// </summary>
        /// <param name="game">The game this control belongs to.</param>
        /// <param name="image">The image.</param>
        public Picture(Game game, GuiManager manager, Texture2D image)
            : base(game, manager)
        {
            this.image = image;
            rotation = 0.0f;
            movable = false;
            visible = true;


        }

        /// <summary>
        /// Updates the picture box.
        /// </summary>
        /// <param name="gameTime">Frame time information.</param>
        public override void Update(GameTime gameTime)
        {
            //checks to see if the object is moveable 
            if (movable)
            {
                MouseState ms = Mouse.GetState();
                Point mouseLoc = new Point(ms.X, ms.Y);


                if (this.Bounds.Contains(mouseLoc))
                {
                    //check sot see if the mouse presses on the picture box
                    if (ms.LeftButton == ButtonState.Pressed)
                    {
                        //sets the location of the picture box to the mouse
                        Vector2 offset = new Vector2(ms.X - this.Size.X / 2, ms.Y - this.Size.Y / 2);
                        this.Position = offset;

                    }
                    else
                    {
                        //if the person is not holding on the box it resets
                        this.Position = permanentPostion;
                    }
                }
                else
                    this.Position = permanentPostion;//if the person is not holding on the box it resets


            }


            // do ~nothing~
        }

        /// <summary>
        /// resets the picture boxes position
        /// </summary>
        public void resetPosition()
        {
            this.Position = permanentPostion;

        }
        /// <summary>
        /// Draws the picture box.
        /// </summary>
        /// <param name="gameTime">Frame time information.</param>
        /// <param name="spriteBatch">The sprite batch to draw with.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle bounds = Bounds;
            if (visible)
            {
                spriteBatch.Draw(image, bounds, null, Color, rotation, new Vector2(bounds.Width / 2f, bounds.Height / 2f), SpriteEffects.None, 0);
            }


        }
    }
}
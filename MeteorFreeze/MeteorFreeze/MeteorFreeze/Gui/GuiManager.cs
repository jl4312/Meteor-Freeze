using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeteorFreeze.Gui
{
    public class GuiManager
    {
        private Dictionary<string, GameControl> controls;
        private Game game;

        public GameControl this[ string name ]
        {
            get
            {
                if (controls.ContainsKey(name))
                {
                    return controls[ name ];

                }
                return null;
            }
            set
            {
                controls[ name ] = value;
            }
        }

        public GuiManager(Game game)
        {
            controls = new Dictionary<string, GameControl>();
            this.game = game;

        }

        /// <summary>
        /// Adds an already created control to the manager.
        /// </summary>
        /// <param name="control">The control.</param>
        public void AddControl(GameControl control)
        {
            controls.Add(control.Name, control);
        }

        /// <summary>
        /// Adds a button to this GUI manager.
        /// </summary>
        /// <param name="name">The name of the button control.</param>
        /// <param name="group">The button group.</param>
        /// <param name="x">The X coordinate of the button.</param>
        /// <param name="y">The Y coordinate of the button.</param>
        /// <param name="width">The width of the button.</param>
        /// <param name="height">The height of the button.</param>
        public void AddButton(string name, string text, float x, float y, float width, float height)
        {
            Button button = new Button(game, this, text, x, y, width, height);
            button.Name = name;
            controls[name] = button;
        }

        public void AddPicture(string name, Texture2D image, Vector2 loc, Boolean movable)
        {
            Picture picture = new Picture(game, this, image);
            picture.PermanentPosition = loc;
            picture.Name = name;
            picture.Movable = true;
            controls[name] = picture;
        }

        public void AddPicture(string name, Texture2D image)
        {
            Picture picture = new Picture(game, this, image);
            picture.Name = name;
            controls[name] = picture;
        }

        public void AddBoundingBox(string name)
        {
            BoundingBox boundingBox = new BoundingBox(game, this);
            boundingBox.Name = name;
            controls[name] = boundingBox;
        }


        /// <summary>
        /// Adds a label to the GUI manager.
        /// </summary>
        /// <param name="name">The name of the label.</param>
        /// <param name="font">The font for the label.</param>
        /// <param name="text">The label's text.</param>
        public void AddLabel(string name, SpriteFont font, string text)
        {
            Label label = new Label(game, this, font, text);
            label.Name = name;
            controls[name] = label;
        }

        /// <summary>
        /// Removes a control.
        /// </summary>
        /// <param name="name">The name of the control to remove.</param>
        public void RemoveControl(string name)
        {
            if (controls.ContainsKey(name))
            {
                controls.Remove(name);
            }
        }

        /// <summary>
        /// Updates all GUI controls in the manager.
        /// </summary>
        /// <param name="gameTime">Frame time values.</param>
        public void Update(GameTime gameTime)
        {
            foreach (GameControl control in controls.Values)
            {
                control.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws all GUI controls in the manager.
        /// </summary>
        /// <param name="gameTime">Frame time values.</param>
        /// <param name="spriteBatch">The sprite batch to use to draw.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameControl control in controls.Values)
            {
                control.Draw(gameTime, spriteBatch);
            }
        }



    }
}

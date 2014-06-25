using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;

namespace MeteorFreeze.GameObjects
{
    class Ammo : GameObject
    {
        private Color color;

        private Rectangle top;
        private Rectangle bottom;

        private int fallingSpeed;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public int FallingSpeed
        {
            get { return fallingSpeed; }
        }

        public Ammo(Rectangle position, ContentManager contentRef, Color color)
            : base(position, contentRef)
        {
            this.texture = contentRef.Load<Texture2D>("IceChunk");
            this.color = color;
            active = true;
            fallingSpeed = 1;
            top = new Rectangle(position.X, position.Y, position.Width, 1);
            bottom = new Rectangle(position.X, position.Y + position.Height, position.Width, 1);
        }

        public override void Update(GameTime gameTime, List<GameObject> objectList)
        {
            fallingSpeed = 6;
            top = new Rectangle(position.X, position.Y - 5, position.Width, 1);
            //bottom = new Rectangle(position.X, position.Y + position.Height + 25, position.Width, 1);

            if (active)
            {

                foreach (GameObject obj in objectList)
                {
                    if (obj is Ammo)
                    {
                        if (bottom.Intersects(obj.Position))
                        {
                            fallingSpeed = 0;
                        }
                    }
                    else if (obj is Guage)
                    {
                        bottom = new Rectangle(position.X, position.Y + position.Height + 25, position.Width, 1);
                        if (!bottom.Intersects(obj.Position))
                        {
                            fallingSpeed = 0;

                        }
                        if (bottom.Y <= obj.Position.Y + obj.Position.Height / 6 * 5)
                        {
                            bottom = new Rectangle(position.X, position.Y + position.Height, position.Width, 1);
 
                        }
                        else
                            bottom = new Rectangle(position.X, position.Y + position.Height + 25, position.Width, 1);
                            
   
                    }

                }
            }
            position.Y += fallingSpeed;
            

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active)
                spriteBatch.Draw(texture, position, color);
        }

    }
}

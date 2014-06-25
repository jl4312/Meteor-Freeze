using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MeteorFreeze
{
    public class GameObject
    {
        protected Rectangle position;
        protected Texture2D texture;
        protected Boolean active;
        protected ContentManager contentRef;
        protected Color color = Color.White;
        protected bool passableFromBelow;//checks to see if it is passible

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Boolean Active
        {
            get { return active; }
            set { active = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public GameObject(int x, int y, int width, int height, Texture2D texture)
        {
            position = new Rectangle(x, y, width, height);
            this.texture = texture;
            active = true;


        }

        public GameObject(Rectangle position, ContentManager contentRef)
        {
            this.position = position;
            this.contentRef = contentRef;
            active = true;

        }

        public GameObject(int x, int y, int width, int height)
        {
            position = new Rectangle(x, y, width, height);
        }

        public virtual void Update(GameTime gameTime, List<GameObject> objectList)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null && active)
            {
                spriteBatch.Draw(texture, position, color);
            }
        }

        public Vector2 getMidPoint()
        {
            return new Vector2(position.X + position.Width / 2, position.Y + position.Height / 2);
        }

        public Rectangle getMidPointR()
        {
            return new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, this.position.Width, this.position.Height);
        }

        public bool PassableFromBelow
        {
            get { return passableFromBelow; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MeteorFreeze
{
    class Platform : GameObject
    {
      

        public Platform(Rectangle pos, ContentManager contentRef) 
            : base(pos, contentRef)
        {
            position = pos;
            texture = contentRef.Load<Texture2D>("BoundingBox");
 
        }

        public virtual void Update(GameTime gameTime, List<GameObject> objectList)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }
       
    }
}

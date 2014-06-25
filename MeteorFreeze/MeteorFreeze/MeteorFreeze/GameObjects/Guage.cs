using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MeteorFreeze.GameObjects
{
    class Guage : GameObject
    {
        
        private Queue<Ammo> stockpile;

        public Queue<Ammo> Stockpile
        {
            get { return stockpile; }
        }

        public Guage(Rectangle position, ContentManager contentRef)
            : base(position, contentRef)
        {
            this.texture = contentRef.Load<Texture2D>("Guage");
            stockpile = new Queue<Ammo>();
        }

        public Ammo getAmmo()
        {
            if (stockpile.Count > 0)
            {
                Ammo tmp = stockpile.Dequeue();
                tmp.Active = false;
                return tmp;

            }
            else
                return null;
        }

        public bool guageLoaded()
        {
            if (stockpile.Count > 0)
                return true;
            return false;
        }

        public void addAmmo(Meteor meteor)
        {
            if (stockpile.Count < 11)
            {
                Rectangle startingPosition = new Rectangle((int)(this.getMidPoint().X - this.Position.Width / 4), (int)(this.getMidPoint().Y - this.Position.Height / 4), (int)this.Position.Width / 4, (int)this.Position.Width / 4);
                Ammo tmp = new Ammo(startingPosition, contentRef, meteor.Color);
                stockpile.Enqueue(tmp);
            }
        }

       


        public void reset()
        {
            stockpile.Clear();
        }




    }
}

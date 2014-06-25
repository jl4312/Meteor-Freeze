using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MeteorFreeze.Graphics;

namespace MeteorFreeze.GameObjects
{
    class Meteor : GameObject
    {
        private Texture2D activeTexture;
        private Texture2D frozenTexture;
        private Vector2 velocity;
        private Vector2 currentVelocity;
        private int totalHP;
        private int currentHP;

        private Boolean frozen;

        private Rectangle top;
        private Rectangle bottom;

        private FireEmitter fireEmitter;
        private Texture2D fireTexture;

        public FireEmitter FireEmitter
        {
            get { return fireEmitter; }
        }

        public Boolean Frozen
        {
            get { return frozen; }
            set { frozen = value; }
        }

        public float VelocityX
        {
            get { return currentVelocity.X; }
            set { currentVelocity.X = value; }
        }

        public float VelocityY
        {
            get { return currentVelocity.Y; }
            set { currentVelocity.Y = value; }
        }

        public int CurrentHP
        {
            get { return currentHP; }
        }



        public Random rand = new Random();
  /*      public Random randX;
        public Random randY;
*/
        public Meteor(Rectangle position, ContentManager contentRef)
            : base(position, contentRef)
        {
            
            velocity = new Vector2((float)rand.Next(-5, 5), rand.Next(1, 3));
            //velocity = new Vector2(0, 1);
                
            currentVelocity = velocity;

            totalHP = rand.Next(1, 7);
            currentHP = totalHP;


            position.Width = 35 + totalHP * 5;
            position.Height = 35 + totalHP * 5;

            active = true;
            frozen = false;

            frozenTexture = contentRef.Load<Texture2D>("Block3");
            texture = contentRef.Load<Texture2D>("Meteor");
            fireTexture = contentRef.Load<Texture2D>("vortex");

            fireEmitter = new FireEmitter(fireTexture, position, .6f);
        }

        public Meteor(Rectangle position, ContentManager contentRef, int hp)
            : base(position, contentRef)
        {
           
            if (hp > 0)
            {
                
                velocity = new Vector2((float)rand.Next(-5, 5), (float)(rand.Next(1, 2)));
                currentVelocity = velocity;

                totalHP = hp;
                currentHP = hp;

                position.Width = 35 + totalHP * 5;
                position.Height = 35 + totalHP * 5;


                frozen = false;
            }
            else
            {
                totalHP = hp;
                currentHP = hp;
                velocity = new Vector2(0, 0);
                currentVelocity.Y = 0;
                frozen = true;
            }
            active = true;

            frozenTexture = contentRef.Load<Texture2D>("Block3");
            texture = contentRef.Load<Texture2D>("Meteor");

            fireTexture = contentRef.Load<Texture2D>("vortex");

            fireEmitter = new FireEmitter(fireTexture, position, .6f);
        }

        public Meteor(Rectangle position, ContentManager contentRef, int hp, Vector2 velocity)
            : base(position, contentRef)
        {
            if (hp > 0)
            {
                
                this.velocity = velocity;
                currentVelocity = velocity;

                totalHP = hp;
                currentHP = hp;

                position.Width = 35 + totalHP * 5;
                position.Height = 35 + totalHP * 5;


                frozen = false;
            }
            else
            {
                totalHP = hp;
                currentHP = hp;
                velocity = new Vector2(0, 0);
                currentVelocity.Y = 0;
                frozen = true;
            }
            active = true;

            frozenTexture = contentRef.Load<Texture2D>("Block3");
            texture = contentRef.Load<Texture2D>("Meteor");

            fireTexture = contentRef.Load<Texture2D>("vortex");

            fireEmitter = new FireEmitter(fireTexture, position, .6f);
        }

        public override void Update(GameTime gameTime, List<GameObject> objectList)
        {

            if (active)
            {
                top = new Rectangle(position.X, position.Y - 5, position.Width, 1);
                bottom = new Rectangle(position.X, position.Y + position.Height + 1, position.Width, 1);
                currentVelocity = velocity;

                
                foreach (GameObject obj in objectList)
                {
                    if (obj == this)
                        continue;
                    if (!obj.Active)
                        continue;
                    if (obj is Platform)
                    {
                        if (obj.Position.X >= position.X)
                        {
                            
                                position.X = obj.Position.X + obj.Position.Width - position.Width;
                        }
                        else if (obj.Position.X + obj.Position.Width <= position.Width + position.X)
                        {

                            position.X = obj.Position.X;
                        }
                        else if (frozen)
                        {
                            if (bottom.Intersects(new Rectangle(obj.Position.X, obj.Position.Y + obj.Position.Height - 15, obj.Position.Width, 10)))
                            {
                                currentVelocity.Y = 0;
                            }

                        }
                    }
                    if (velocity.Y > 0 && position.Intersects(obj.Position) && obj != this)
                    {
                        if (obj is FreezeShot && !frozen && obj.Active)
                        {
                            FreezeShot shot = obj as FreezeShot;
                            if (shot.CurrentState == State.fire)
                            {
                                shot.CurrentState = State.explosion;
                                currentHP-= 1;
                            }


                        }
                        else if (obj is Meteor)
                        {
                            Meteor tmp = obj as Meteor;

                            if (frozen != tmp.Frozen)
                            {
                                tmp.Active = false;
                                active = false;
                            }
                            if ((frozen && tmp.frozen))
                            {
                                tmp.currentVelocity.Y = 0;
                                currentVelocity.Y = 0;
                            }
                            /*
                            if (!frozen && !tmp.Frozen)
                            {
                                if (currentVelocity == tmp.currentVelocity)
                                {
                                    currentVelocity *= -1;
                                }
                            }

                            /*
                        else
                        {
                            if (VelocityX == 0 && tmp.VelocityX == 0)
                            {
                                float avgVelocityY = ((velocity.Y + tmp.VelocityY) / 4) * 3;

                                VelocityY = avgVelocityY;
                                tmp.VelocityY = avgVelocityY;
                            }
                        }*/
                        }
                    }

                }

                if (currentHP <= 0)
                {

                    position.Width = 30;
                    position.Height = 30;

                    if (position.X % 30 != 0)
                    {
                        if (currentVelocity.X > 0)
                        {
                            position.X = (int)(position.X + 30 / 30) * 30;
                        }
                        else
                            position.X = (int)(position.X / 30) * 30;
                        Console.WriteLine(position.X);

                    }
                    else
                    {
                        frozen = true;
                        currentVelocity.X = 0;
                        currentVelocity.Y = (int)1.5 * currentVelocity.Y;
                        position.Width = 30;
                        position.Height = 30;
                        texture = frozenTexture;
                    }
                    frozen = true;
                        currentVelocity.X = 0;
                        currentVelocity.Y = (int)1.5 * currentVelocity.Y;
                        position.Width = 30;
                        position.Height = 30;
                        texture = frozenTexture;

                }
                else
                {
                    BreakApart(objectList);
                    position.Width = 35 + currentHP *5;
                    position.Height = 35 + currentHP *5;

                }

                Physics(); 

                
            }

            position.X += (int)currentVelocity.X / 10;
            position.Y += (int)currentVelocity.Y / 10;

            fireEmitter.Update(gameTime, position);

            base.Update(gameTime, objectList);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!frozen)
                fireEmitter.Draw(spriteBatch);
            if (active)
            {
                
                spriteBatch.Draw(texture, position, color);
            }
           
        }

        public bool CheckCollision(GameObject obj)
        {
            return this.Position.Intersects(obj.Position);
        }

        public void Physics()
        {
            //Moves the meteor
            if (!frozen)
            {
                
                if (currentVelocity.Y <= 0)
                {
                    currentVelocity.Y += 1f;
                }
                position.X += (int)currentVelocity.X;
                position.Y += (int)currentVelocity.Y;

            }
            else
            {
                if (currentVelocity.Y < 0)
                {
                    currentVelocity.Y += 1f;
                }
                position.Y += (int)currentVelocity.Y;
            }
        }

        public void BreakApart(List<GameObject> objectList)
        {
            List<Meteor> mList = new List<Meteor>();
            for (int i = 0; i < objectList.Count; i++)
            {
                if (velocity.Y > 0 && position.Intersects(objectList[i].Position))
                {
                    if (objectList[i] is FreezeShot && !frozen && objectList[i].Active)
                    {
                        int division = rand.Next(2, 5);
                        if (currentHP > 3 && currentHP % division == 0)
                        {
                            switch (division)
                            {
                                case 2:
                                    mList.Add(new Meteor(position, contentRef, (int)(currentHP / division), new Vector2(3 , 1)));
                                    mList.Add(new Meteor(position, contentRef, (int)(currentHP / division), new Vector2(-3, 1)));
                                    break;
                                case 3:
                                    mList.Add(new Meteor(position, contentRef, (int)(currentHP / division), new Vector2(3, 1)));
                                    mList.Add(new Meteor(position, contentRef, (int)(currentHP / division), new Vector2(-3, 1)));
                                    mList.Add(new Meteor(position, contentRef, (int)(currentHP / division), new Vector2(0, 1)));
                                    break;
                                case 4:
                                    mList.Add(new Meteor(position, contentRef, (int)(currentHP / division), new Vector2(3, 1)));
                                    mList.Add(new Meteor(position, contentRef, (int)(currentHP / division), new Vector2(-3, 2)));
                                    mList.Add(new Meteor(position, contentRef, (int)(currentHP / division), new Vector2(0, 1)));
                                    mList.Add(new Meteor(position, contentRef, (int)(currentHP / division), new Vector2(3, 1)));
                                    break;

                            }
                            
                            this.Active = false;
                        }
                    }
                }

            }
            objectList.AddRange(mList);
            
        }

        public void Combine(List<GameObject> objectList)
        {
            if (!frozen)
            {
                for (int i = 0; i < objectList.Count; i++ )
                {
                    if (objectList[i] is Meteor)
                    {
                        Meteor met = objectList[i] as Meteor;

                        if (position.Intersects(met.position) && !met.Frozen)
                        {
                            objectList.Add(new Meteor(position, contentRef, met.currentHP + this.currentHP));
                            active = false;
                            met.Active = false;
                        }
                    }

                }
            }

        }
    }
}

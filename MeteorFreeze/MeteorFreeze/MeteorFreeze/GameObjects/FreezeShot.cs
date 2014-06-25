using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MeteorFreeze.Graphics;
namespace MeteorFreeze.GameObjects
{
    enum State
    {
        charge, fire, explosion
    }
    class FreezeShot : GameObject
    {
        private const int SPEED = 30;
        private Vector2 velocity;
        private int counter;
        private State currentState;
        private Dictionary<State, Texture2D> imageDictionary;

        private Vector2 pos;

        private FrostEmitter frostEmitter;
        private Texture2D fireTexture;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public State CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public FrostEmitter FrostEmitter
        {
            get { return frostEmitter; }
        }

        public FreezeShot(Rectangle position, ContentManager contentRef, float angle, Ammo ammo)
            : base(position, contentRef)
        {
            imageDictionary = new Dictionary<State, Texture2D>();

            imageDictionary.Add(State.charge, contentRef.Load<Texture2D>("IceChunk"));
            imageDictionary.Add(State.fire, contentRef.Load<Texture2D>("Shot"));
            imageDictionary.Add(State.explosion, contentRef.Load<Texture2D>("PowerUps"));

            pos = new Vector2(position.X, position.Y);

            velocity.X = (float)(Math.Cos(angle) * SPEED);
            velocity.Y = (float)(Math.Sin(angle) * SPEED);

            Console.WriteLine(angle);
            color = ammo.Color;
            counter = 0;
            currentState = State.charge;
            texture = imageDictionary[currentState];
            active = true;

            fireTexture = contentRef.Load<Texture2D>("vortex");
            
            frostEmitter = new FrostEmitter(fireTexture, position, .1f);  

        }

        public override void Update(GameTime gameTime, List<GameObject> objectList)
        {


            switch (currentState)
            {
                case State.charge:
                    counter++;
                    if (counter > 5)
                    {

                        currentState = State.fire;
                    }
                    break;
                case State.fire:
                    foreach (GameObject obj in objectList)
                    {
                        if (!obj.Active)
                            continue;
                        else if (obj is FreezeShot || obj is Platform || obj is Character.Character || obj is Ammo || obj is Guage)
                        {
                            continue;
                        }
                        else if (this.Position.Intersects(obj.Position))
                        {
                            currentState = State.explosion;
                            velocity.X = 0;
                            velocity.Y = 0;
                            counter = 0;
                        }
                    }
                    position.Width = 15;
                    position.Height = 15;
                    pos.X += (int)velocity.X;
                    pos.Y += (int)velocity.Y;
                    break;
                case State.explosion:
                    counter++;
                    velocity = new Vector2(0, 0);

                    position.X -= counter;
                    position.Y -= counter;
                    position.Width += counter;
                    position.Height += counter;
                    if (counter >= 10)
                    {
                        active = false;
                    }
                    break;

            }

            position.X = (int)pos.X;
            position.Y = (int)pos.Y;

            texture = imageDictionary[currentState];

            frostEmitter.Update(gameTime, position);
            base.Update(gameTime, objectList);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                frostEmitter.Draw(spriteBatch);
                spriteBatch.Draw(texture, position, color);
            }
           
        }



    }
}

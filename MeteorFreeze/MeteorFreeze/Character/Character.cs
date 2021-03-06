﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MeteorFreeze.GameObjects;
using MeteorFreeze.Graphics;
using MeteorFreeze;

namespace MeteorFreeze.Character
{
    public enum characterState
    {
        //finite state representing the player's state
        walking,
        running,
        standing,
        jumping,  
        inair,
        falling
        
    }
    class Character : GameObject
    {
        protected Vector2 velocity;//holds the players velocity 
        protected Vector2 acceleration;//holds the players acceleration

        protected bool isJumping;//holds whether or not the character is jumping
        protected bool inAir;//holds whether or not the character is airborn


        private KeyboardState kPrevState;//holds the prevoius key pressed
        private KeyboardState kState;//hold the current key pressed

        protected characterState currentState;//holds the character's current state
        protected characterState previousState;//holds the character's previous state

        protected Dictionary<string, Texture2D> imageDictionary;//image dictionary of all the texture2d for the character

        protected Rectangle bottom;//rectangle representing the bottom part of the character'
        protected Rectangle top;//rectangle representing the top part of the character
        protected Rectangle left;//rectangle representing the left side of the character
        protected Rectangle right;//rectangle representing the right side of the character

        protected Dictionary<string, Animation> animationList;//dictionary of the animation for specific states
        protected Animation currentAnimation;//holds the current animation for the character

        protected FreezeShot shot;//freezeshot that the character shoots out
        protected int point;//holds the character's points
        protected int lives;//holds how many lives the character has

        public int Point
        {
            get { return point; }
            set { point = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public float VelocityX
        {
            get { return velocity.X; }
            set { velocity.X = value; }
        }

        public Character(Rectangle pos, ContentManager contentRef) 
            : base(pos, contentRef)
        {
            imageDictionary = new Dictionary<string, Texture2D>();
            animationList = new Dictionary<string, Animation>();

            #region creates 4 rectangles representing the sides of the character
            top = new Rectangle(position.X + position.Width - 4, position.Y, position.Width - position.Width / 4, 5);
            bottom = new Rectangle(position.X + position.Width - 4, position.Y + position.Height , position.Width - position.Width / 4, 5);
            left = new Rectangle(position.X - 10, position.Y + position.Height / 12, 5, position.Height - position.Height / 6);
            right = new Rectangle(position.X + position.Width + 10, position.Y + position.Height / 12, 5, position.Height - position.Height / 6);
            #endregion

            point = 0;
            lives = 3;

        }

        /// <summary>
        /// checks if the object collides with the gameObject
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CheckCollision(GameObject obj)
        {
            return this.Position.Intersects(obj.Position);
        }

        /// <summary>
        /// jump method t
        /// </summary>
        /// <param name="gameTime"></param>
        public void Jump(GameTime gameTime)
        {
            if (!isJumping)
            {
                isJumping = true;
                inAir = true;
                acceleration.X = 0;
                velocity.Y = 8f;
                if (velocity.Y > 10f)
                {
                    velocity.Y = 10;
                }
            }
        }

        public override void Update(GameTime gameTime, List<GameObject> objectList)
        {
           
            position.X += (int)velocity.X;
            position.Y -= (int)velocity.Y;

            Input(gameTime);
            Physics(objectList);

            //Update the quads
            left = new Rectangle(position.X - 5, position.Y + position.Height / 8, 5, position.Height - position.Height / 4);
            right = new Rectangle(position.X + position.Width, position.Y + position.Height / 8, 5, position.Height - position.Height / 4);
            bottom = new Rectangle(position.X + position.Width / 8, position.Y + position.Height + 7, position.Width - position.Width / 4, 5);
            top = new Rectangle(position.X + position.Width / 8, position.Y, position.Width - position.Width / 4, 5);

            if (shot != null)
            {
                shot.Update(gameTime, objectList);
            }
            currentAnimation.Update(gameTime);
            base.Update(gameTime, objectList);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public void Physics(List<GameObject> objectList)
        {
            acceleration.Y = -0.28f;

            velocity.X += acceleration.X;
            velocity.Y += acceleration.Y;

            if (velocity.Y < -6.0f)
            {
                velocity.Y = -6.0f;
            }

            foreach (GameObject gObject in objectList)
            {
                if (gObject == this)
                    continue;
                if (gObject is FreezeShot)
                    continue;
                if (!gObject.Active)
                    continue;


                if (velocity.X > 0)
                {
                    if (gObject is Platform)
                    {
                        if (position.X + position.Width > gObject.Position.X + gObject.Position.Width - 10)
                        {
                            position.X = gObject.Position.X + 10;
                        }

                    }
                    else
                    {
                        if (right.Intersects(gObject.Position))
                        {
                            velocity.X = 0;
                        }
                    }

                }
                else if (velocity.X < 0)
                {
                    if (gObject is Platform)
                    {
                        if (position.X < gObject.Position.X + 10)
                            position.X = gObject.Position.X + gObject.Position.Width - position.Width - 10;

                    }
                    else
                    {
                        if (left.Intersects(gObject.Position))
                        {
                            velocity.X = 0;
                        }
                    }
                }

                if (velocity.Y < 0)
                {
                    if (inAir)
                    {
                        currentState = characterState.falling;
                    }
                    if (gObject is Platform)
                    {
                        if (position.Y + position.Height >= gObject.Position.Y + gObject.Position.Height - 15)
                        {
                            velocity.Y = 0;
                            velocity.Y = 0;
                            inAir = false;
                            isJumping = false;
                        }

                    }
                    else
                    {
                        if (bottom.Intersects(gObject.Position))
                        {
                            velocity.Y = 0;
                            inAir = false;
                            isJumping = false;

                        }
                    }
                }
                else if (velocity.Y > 0)
                {
                    if (gObject is Platform)
                    {
                        if (position.Y <= gObject.Position.Y)
                            velocity.X = 0;

                    }
                    else
                    {
                        if (top.Intersects(gObject.Position) && !gObject.PassableFromBelow)
                        {
                            velocity.Y = 0;

                        }
                    }
                }
                
                currentState = characterState.standing;

            }

            
         
 
        }

        public void Input(GameTime gameTime)
        {
            

            kPrevState = kState;
            kState = Keyboard.GetState();
            previousState = currentState;

           
            if(kState.IsKeyDown(Keys.W))
            {
                Jump(gameTime);
                currentState = characterState.jumping;

            }
            else if (kState.IsKeyDown(Keys.A))
            {
                acceleration.X -= .005f;

                if (velocity.X <= -3)
                {
                    velocity.X = -3;
                    currentState = characterState.running;
                }
                else
                {
                    velocity.X += acceleration.X;
                    currentState = characterState.walking;
                }
 
            }
            else if (kState.IsKeyDown(Keys.D))
            {
                acceleration.X += .005f;
                if (velocity.X >= 3)
                {
                    velocity.X = 3;
                    currentState = characterState.running;
                }
                else
                {
                    velocity.X += acceleration.X;
                    currentState = characterState.walking;
                }

            }
            else
            {
                //currentState = characterState.standing;
                acceleration.X = 0;
                velocity.X = 0;

            }
            
        }
    }
}

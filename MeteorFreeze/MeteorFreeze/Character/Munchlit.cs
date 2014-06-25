using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using MeteorFreeze.Graphics;

namespace MeteorFreeze.Character
{
    class Munchlit : Character
    {
        const float SCALE_FACTOR = 1.2f;

        public Munchlit(Rectangle position, ContentManager contentRef)
            : base(position, contentRef)
        {
            Animation falling = new Animation();
            Animation jumping = new Animation();
            Animation inAir = new Animation();
            Animation walking = new Animation();
            Animation running = new Animation();
            

            //Create and load textures
            Texture2D fallingTexture = contentRef.Load<Texture2D>("Munchlit Falling");
            Texture2D jumpingTexture = contentRef.Load<Texture2D>("Munchlit Jumping");
            Texture2D inAirTexture = contentRef.Load<Texture2D>("Munchlit Rising");
            Texture2D walkingTexture = contentRef.Load<Texture2D>("Munchlit Running Spreadsheet");
            Texture2D runningTexture = contentRef.Load<Texture2D>("Munchlit Running Spreadsheet");

            // Texture2D fireTexture = contentRef.Load<Texture2D>("vortex");

            //Add textures to the image Dictionary
            imageDictionary.Add("falling", fallingTexture);
            imageDictionary.Add("jumping", jumpingTexture);
            imageDictionary.Add("inAir", inAirTexture);
            imageDictionary.Add("walking", walkingTexture);
           
            imageDictionary.Add("running", runningTexture);


            //Initialize animations with spritesheets
            falling.Initialize(imageDictionary["falling"], 1, 540, 540, 0, SCALE_FACTOR, int.MaxValue);
            jumping.Initialize(imageDictionary["jumping"], 2, 259, 259, 0, SCALE_FACTOR, 50);
            inAir.Initialize(imageDictionary["inAir"], 1, 864, 864, 0, SCALE_FACTOR, int.MaxValue);
            walking.Initialize(imageDictionary["walking"], 8, 250, 250, 0, SCALE_FACTOR, 50);
            running.Initialize(imageDictionary["running"], 8, 250, 250, 0, SCALE_FACTOR, 50);


            //Add animations to the animation list
            animationList.Add("falling", falling);
            animationList.Add("jumping", jumping);
            animationList.Add("inAir", inAir);
            animationList.Add("walking", walking);
            animationList.Add("running", running);

            currentState = characterState.running;
            currentAnimation = running;

            this.position.Width = (int)(position.Width * SCALE_FACTOR);
            this.position.Height = (int)(position.Height * SCALE_FACTOR);
 
        }

        public void Initialize()
        {
            position = new Rectangle((int)(position.X), (int)(position.Y), (int)(position.Width * SCALE_FACTOR), (int)(position.Height * SCALE_FACTOR));
        }

        public override void Update(GameTime gameTime, List<GameObject> objectList)
        {

            base.Update(gameTime, objectList);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (currentState)
            {
                case characterState.running:
                    currentAnimation = animationList["running"];
                    break;
                case characterState.walking:
                    currentAnimation = animationList["walking"];
                    break;
                case characterState.jumping:
                    currentAnimation = animationList["jumping"];
                    break;
                case characterState.inair:
                    currentAnimation = animationList["inAir"];
                    break;
                case characterState.falling:
                    currentAnimation = animationList["falling"];
                    break;
            }

            currentAnimation.Play(position, spriteBatch, velocity);

            base.Draw(spriteBatch);
        }
    }
}

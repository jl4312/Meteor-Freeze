using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeteorFreeze.Graphics
{
    /// <summary>
    /// Class for handling animations
    /// </summary>
    public class Animation
    {
        private Texture2D spriteSheet;

        private int frameCount;
        private int currentFrame;

        private int frameWidth;
        private int frameHeight;

        private int frameTime;
        private int elapsedTime;

        //Base 0
        private int row;
        private float scale;

        private Rectangle source;

        public int Width
        {
            get { return spriteSheet.Width; }
        }

        public int Height
        {
            get { return spriteSheet.Height; }
        }

        /// <summary>
        /// Initializes core components of an animation
        /// </summary>
        /// <param name="image">Spritesheet used for animation</param>
        /// <param name="frames">Number of frames in the animation</param>
        /// <param name="width">Width of one frame</param>
        /// <param name="height">Height of one frame</param>
        /// <param name="row">The row on the spritesheet that contains the animation</param>
        /// <param name="time">Time between frames in milliseconds</param>
        public void Initialize(Texture2D image, int frames, int width, int height, int row, float scale, int time)
        {
            spriteSheet = image;
            frameCount = frames;
            frameWidth = width;
            frameHeight = height;
            this.row = row;
            this.scale = scale;
            frameTime = time;

            currentFrame = 0;
            elapsedTime = 0;
        }

        /// <summary>
        /// Update which frame to draw
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime > frameTime)
            {
                currentFrame++;
                if (currentFrame == frameCount)
                    currentFrame = 0;

                elapsedTime = 0;
            }
            source = new Rectangle((int)(currentFrame * frameWidth), (int)(row * frameHeight), frameWidth, frameHeight);
        }

        //Play the animation
        public void Play(Rectangle position, SpriteBatch spriteBatch, Vector2 direction)
        {
            if (direction.X >= 0)
                spriteBatch.Draw(spriteSheet, position, source, Color.White);
            else
                spriteBatch.Draw(spriteSheet, position, source, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            //spriteBatch.Draw(spriteSheet, new Rectangle(position.X, position.Y, (int)(position.Width * scale),(int)(position.Height * scale)), source, Color.White);
        }
    }
}

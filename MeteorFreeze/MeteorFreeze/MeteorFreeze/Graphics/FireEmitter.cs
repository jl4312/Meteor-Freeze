using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace MeteorFreeze.Graphics
{
    public class FireEmitter
    {

        #region Fields
        // Number of total particles
        const int TOTALPARTICLES = 1000;

        public Texture2D particleTexture;
        public Rectangle particleSourceRect;

        // List of particles
        List<Particle> particles;
        int firstDeadIndex;
        int firstAliveIndex;
        int spawnRate;

        // Random
        Random rand;

        //scale
        float scale;

        //color
        Color startColor = new Color(1.0f, 0.5f, 0.0f, .1f);
        Color endColor = new Color(1.0f, 0, 0, 0.3f);

        //rotation
        float rotationSpeed;

        //position
        float x;
        float y;

        #endregion

        #region properties
        public Color StartColor
        {
            get { return startColor; }
            set { startColor = value; }
        }

        public Color EndColor
        {
            get { return endColor; }
            set { endColor = value; }
        }

        public float RotationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        #endregion


        #region Constructor
        public FireEmitter(Texture2D tex, Rectangle charPosition, float scale)
        {
            particleTexture = tex;
            particleSourceRect = new Rectangle(0, 0, particleTexture.Width, particleTexture.Height);
            rand = new Random();
            this.scale = scale;

            spawnRate = 10;
            firstAliveIndex = 0;
            firstDeadIndex = 0;
            particles = new List<Particle>();
            for (int i = 0; i < TOTALPARTICLES; i++)
            {
                Particle p = new Particle();
                Reset(p, charPosition, this.scale, this.startColor, this.endColor);
                particles.Add(p);
            }
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime, Rectangle charPosition)
        {
            for (int i = 0; i < TOTALPARTICLES; i++)
            {
                // Check if particle is alive
                if (particles[i].Alive)
                {
                    particles[i].UpdateParticle((float)gameTime.ElapsedGameTime.TotalSeconds);

                    // Check if the particle is dead
                    if (particles[i].Age >= particles[i].Lifetime)
                    {
                        particles[i].Alive = false;
                        firstAliveIndex++;
                        firstAliveIndex %= TOTALPARTICLES;
                    }
                }
            }

            // Spawn some amount
            for (int i = 0; i < spawnRate; i++)
            {
                Reset(particles[firstDeadIndex], charPosition, this.scale, this.startColor, this.endColor);
                particles[firstDeadIndex].Alive = true;
                firstDeadIndex++;
                firstDeadIndex %= TOTALPARTICLES;
            }
        }

        public void Reset(Particle p, Rectangle charPosition)
        {
            Reset(p, charPosition, 1f, new Color(1.0f, 0.5f, 0.0f, .1f), new Color(1.0f, 0, 0, 0.3f));
        }

        public void Reset(Particle p, Rectangle charPosition, float scale, Color start, Color end)
        {
            x = (float)rand.NextDouble() * 10 + charPosition.X + charPosition.Width / 2 - 4;
            y = (float)rand.NextDouble() * 10 + charPosition.Y + charPosition.Height / 2 - 3;
            rotationSpeed = (float)rand.NextDouble() * .1f - .05f;
            float startRoation = (float)rand.NextDouble() * MathHelper.TwoPi;

            Vector2 vel = new Vector2(
                (float)rand.NextDouble() * 1.0f - 1.5f,//2.5f,
                (float)rand.NextDouble() * -2.0f);

            p.Alive = false;
            p.Position = new Vector2(x, y);
            p.Velocity = vel;
            p.Rotation = startRoation;
            p.RotationSpeed = rotationSpeed;
            p.Lifetime = 0.2f;
            p.Age = 0;
            p.StartColor = start;
            p.EndColor = end;
            p.CurrentColor = p.StartColor;
            p.Scale = scale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            foreach (Particle p in particles)
            {
                if (p.Alive)
                {
                    spriteBatch.Draw(
                        particleTexture,
                        p.Position,
                        particleSourceRect,
                        p.CurrentColor,
                        p.Rotation,
                        new Vector2(particleTexture.Width / 2, particleTexture.Height / 2),
                        p.Scale,
                        SpriteEffects.None,
                        0);
                }
            }
            // spriteBatch.End();
        }
        #endregion
    }
}

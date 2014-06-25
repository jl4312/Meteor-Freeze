using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MeteorFreeze.Graphics
{
    public class Particle
    {
        // Particle Fields
        public bool Alive;
        public Vector2 Position;
        public Vector2 Velocity;
        public float Rotation;
        public float RotationSpeed;
        public float Lifetime;
        public float Age;
        public Color CurrentColor;
        public Color StartColor;
        public Color EndColor;
        public float Scale;

        // Update the individual particle
        public void UpdateParticle(float deltaTime)
        {
            Position += Velocity;
            Rotation += RotationSpeed;
            Age += deltaTime;
            CurrentColor = Color.Lerp(StartColor, EndColor, Age / Lifetime);
        }
    }
}
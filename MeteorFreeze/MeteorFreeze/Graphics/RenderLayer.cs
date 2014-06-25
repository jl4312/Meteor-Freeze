using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MeteorFreeze.Graphics
{
    /// <summary>
    /// A layer used for rendering elements.
    /// </summary>
    public class RenderLayer
    {
        private GraphicsDevice device;
        private RenderTarget2D renderTarget;
        private BlendState blendState;
        private SpriteBatch spriteBatch;
        private Texture2D entity;
        private Rectangle destination;
        private Effect effect;
        private bool isDrawing;

        /// <summary>
        /// Gets or sets this render layer's effect.
        /// </summary>
        public Effect Effect
        {
            get
            {
                return effect;
            }
            set
            {
                effect = value;
            }
        }

        /// <summary>
        /// Gets or sets the blend state of this layer.
        /// </summary>
        public BlendState BlendState
        {
            get
            {
                return blendState;
            }
            set
            {
                blendState = value;
            }
        }

        /// <summary>
        /// Gets the layer's render target.
        /// </summary>
        public RenderTarget2D RenderTarget
        {
            get
            {
                return renderTarget;
            }
        }

        /// <summary>
        /// Gets this layer's sprite batch.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get
            {
                return spriteBatch;
            }
        }

        /// <summary>
        /// Gets whether or not the layer is currently drawing.
        /// </summary>
        public bool IsDrawing
        {
            get
            {
                return isDrawing;
            }
        }

        /// <summary>
        /// Gets or sets the destination rectangle this layer should be drawn to on-screen.
        /// </summary>
        public Rectangle Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
            }
        }

        /// <summary>
        /// Creates a new, empty render layer.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device the layer belongs to.</param>
        /// <param name="blendState">The blending state of this layer.</param>
        public RenderLayer(GraphicsDevice graphicsDevice, BlendState blendState)
        {
            effect = null;
            device = graphicsDevice;
            this.blendState = blendState;
            spriteBatch = new SpriteBatch(device);
            entity = null;

            // create the target to take up the whole screen
            PresentationParameters pp = device.PresentationParameters;
            renderTarget = new RenderTarget2D(
                device, pp.BackBufferWidth, pp.BackBufferHeight,
                false, device.DisplayMode.Format,
                pp.DepthStencilFormat, pp.MultiSampleCount, pp.RenderTargetUsage
            );
        }

        /// <summary>
        /// Creates a new render layer from a texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="blendState">The blend state.</param>
        public RenderLayer(Texture2D texture, BlendState blendState)
        {
            effect = null;
            device = texture.GraphicsDevice;
            this.blendState = blendState;
            spriteBatch = new SpriteBatch(device);
            entity = texture;

            // create the target to take up the whole screen
            PresentationParameters pp = device.PresentationParameters;
            renderTarget = new RenderTarget2D(
                device, pp.BackBufferWidth, pp.BackBufferHeight,
                false, device.DisplayMode.Format,
                pp.DepthStencilFormat, pp.MultiSampleCount, pp.RenderTargetUsage
            );
        }

        /// <summary>
        /// Disposes of this render layer.
        /// </summary>
        public void Dispose()
        {
            renderTarget.Dispose();
            spriteBatch.Dispose();
        }

        /// <summary>
        /// Gets the render target as a texture.
        /// </summary>
        /// <returns></returns>
        public Texture2D GetTexture()
        {
            return (Texture2D)renderTarget;
        }

        /// <summary>
        /// Tells the render layer that drawing has begun.
        /// </summary>
        /// <param name="clear">True to clear the layer, false to not.</param>
        /// <param name="clearColor">The clear color.</param>
        public void Begin()
        {
            // switch to this render 
            device.SetRenderTarget(renderTarget);
            device.Clear(Color.Transparent);

            // begin the sprite batch
            spriteBatch.Begin(SpriteSortMode.Deferred, blendState);
            isDrawing = true;

            // draw the entity FIRST if we have one
            if (entity != null)
            {
                spriteBatch.Draw(entity, Vector2.Zero, Color.White);
            }
        }

        /// <summary>
        /// Tells the render layer that drawing has ended.
        /// </summary>
        public void End()
        {
            // end the sprite batch
            spriteBatch.End();
            isDrawing = false;
            device.SetRenderTarget(null);

            // draw the render target now
            Texture2D renderTexture = (Texture2D)renderTarget;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (effect != null)
            {
                // draw using the effect
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    spriteBatch.Draw(renderTexture, destination, Color.White);
                }
            }
            else
            {
                // draw normally
                spriteBatch.Draw(renderTarget, destination, Color.White);
            }
            spriteBatch.End();
        }
    }
}
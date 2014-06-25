using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MeteorFreeze.Graphics
{
    public class EffectRenderer : IDisposable
    {
        private List<RenderLayer> layers;
        private List<RenderLayer> renderQueue;
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;
        private int currentLayerIndex;

        /// <summary>
        /// Gets a render layer based on its index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public RenderLayer this[int index]
        {
            get
            {
                if (index < 0 || index >= layers.Count)
                {
                    return null;
                }
                return layers[index];
            }
        }

        /// <summary>
        /// Gets the current layer.
        /// </summary>
        public RenderLayer CurrentLayer
        {
            get
            {
                return this[currentLayerIndex];
            }
        }

        /// <summary>
        /// Checks to see if the effect renderer is drawing.
        /// </summary>
        public bool IsDrawing
        {
            get
            {
                return CurrentLayer == null ? false : CurrentLayer.IsDrawing;
            }
        }

        /// <summary>
        /// Gets the current sprite batch.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get
            {
                return CurrentLayer == null ? null : CurrentLayer.SpriteBatch;
            }
        }

        /// <summary>
        /// Creates a new effect renderer.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        public EffectRenderer(GraphicsDevice device)
        {
            layers = new List<RenderLayer>();
            renderQueue = new List<RenderLayer>();
            this.device = device;
            spriteBatch = new SpriteBatch(this.device);
            currentLayerIndex = -1;
        }

        /// <summary>
        /// Disposes of all layers in the renderer.
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < layers.Count; ++i)
            {
                layers[i].Dispose();
            }
            spriteBatch.Dispose();
        }

        /// <summary>
        /// Adds a layer to the effect renderer.
        /// </summary>
        /// <param name="blendState">The layer's blend state.</param>
        public void AddLayer(BlendState blendState)
        {
            AddLayer(blendState, null);
        }

        /// <summary>
        /// Adds a layer to the effect renderer.
        /// </summary>
        /// <param name="blendState">The layer's blend state.</param>
        /// <param name="effect">The layer's effect</param>
        public void AddLayer(BlendState blendState, Effect effect)
        {
            RenderLayer layer = new RenderLayer(device, blendState);
            layer.Effect = effect;
            layers.Add(layer);
        }

        /// <summary>
        /// Adds a layer to the effect renderer.
        /// </summary>
        /// <param name="blendState">The layer's blend state.</param>
        /// <param name="effect">The layer's effect</param>
        /// <param name="entity">The layer's entity.</param>
        public void AddLayer(BlendState blendState, Effect effect, Texture2D entity)
        {
            RenderLayer layer = new RenderLayer(entity, blendState);
            layer.Effect = effect;
            layers.Add(layer);
        }

        /// <summary>
        /// Notifies a layer that it should prepare to draw.
        /// </summary>
        /// <param name="index">The index of the layer.</param>
        /// <param name="clearColor">The clear color.</param>
        public void BeginLayer(int index)
        {
            currentLayerIndex = index;
            if (CurrentLayer != null)
            {
                CurrentLayer.Begin();
            }
        }

        /// <summary>
        /// Tells the current layer to finish drawing.
        /// </summary>
        public void EndLayer()
        {
            if (CurrentLayer != null)
            {
                renderQueue.Add(CurrentLayer);
                CurrentLayer.End();
                currentLayerIndex = -1;
            }
        }

        /// <summary>
        /// Renders all of the layers in the layer queue.
        /// </summary>
        /// <param name="clearColor">The color to clear the graphics device to.</param>
        public void RenderAll(Color clearColor)
        {
            // clear graphics device
            device.SetRenderTarget(null);
            device.Clear(clearColor);

            // render each layer that was drawn to
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            for (int i = 0; i < renderQueue.Count; ++i)
            {
                RenderLayer layer = renderQueue[i];
                spriteBatch.Draw(layer.GetTexture(), layer.Destination, Color.White);
            }
            spriteBatch.End();

            // clear out our render queue
            renderQueue.Clear();
        }
    }
}
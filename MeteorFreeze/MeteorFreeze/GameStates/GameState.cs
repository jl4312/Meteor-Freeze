using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MeteorFreeze.Graphics;
using MeteorFreeze.Gui;


namespace MeteorFreeze.GameStates
{
    public abstract class GameState : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected StateManager parent;
        public List<GameObject> childComponents;
        protected EffectRenderer renderer;
        protected Game gameRef;
        protected GuiManager guiManager;

        Texture2D emptyTexture;

        public List<GameObject> Components
        {
            get { return childComponents; }
        }

        public GameState(Game game, EffectRenderer renderer, StateManager parent) :
            base(game)
        {
            childComponents = new List<GameObject>();
            this.renderer = renderer;
            gameRef = game;
            guiManager = new GuiManager(gameRef);
            this.parent = parent;
            emptyTexture = game.Content.Load<Texture2D>("EmptyTile");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!(this is Play))
            {
                for (int i = 0; i < childComponents.Count; ++i)
                {
                    // update each component and remove it if it was disabled
                    GameObject obj = childComponents[i];
                    obj.Update(gameTime, childComponents);
                   
                }
            }
            guiManager.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            guiManager.Draw(gameTime, renderer.SpriteBatch);
            base.Draw(gameTime);
        }
    }
}

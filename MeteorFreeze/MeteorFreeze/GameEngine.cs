
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MeteorFreeze.Character;
using MeteorFreeze.Graphics;
using MeteorFreeze.GameStates;

//using Newtonsoft.Json;


namespace MeteorFreeze
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameEngine : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private EffectRenderer renderer;

        private StateManager stateManager;
        private Play play;
        private MainMenu mainMenu;
        private EndState endState;

        private static Rectangle windowBounds;

    
        public Rectangle WindowBounds
        {
            get { return windowBounds; }
            set { windowBounds = value; }
        }

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 700;
            // TODO: Add your initialization logic here
            graphics.ApplyChanges();

            windowBounds = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            IsMouseVisible = true;
           
            base.Initialize();
            
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
             PresentationParameters pp = GraphicsDevice.PresentationParameters;
            renderer = new EffectRenderer( GraphicsDevice );
            renderer.AddLayer( BlendState.AlphaBlend, Content.Load<Effect>( "basic" ) );
            renderer[ 0 ].Destination = new Rectangle( 0, 0, pp.BackBufferWidth, pp.BackBufferHeight );
            renderer.AddLayer( BlendState.Additive );
            renderer[ 1 ].Destination = renderer[ 0 ].Destination;
            renderer.AddLayer(BlendState.AlphaBlend, Content.Load<Effect>("basic"));
            renderer[2].Destination = renderer[0].Destination;

            stateManager = new StateManager(this);
            Components.Add(stateManager);     
    
            play = new Play( this, renderer, stateManager );
            mainMenu = new MainMenu(this, renderer, stateManager);
            endState = new EndState(this, renderer, stateManager);

            stateManager.RegisterState( GameStateType.Playing, play );
            stateManager.RegisterState(GameStateType.MainMenu, mainMenu);
            stateManager.RegisterState(GameStateType.OptionsMenu, endState);

            stateManager.PushState( mainMenu );
            stateManager.Initialize();


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
             renderer.Dispose();
            
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
           
            
            stateManager.Update(gameTime);

            base.Update( gameTime );
        
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // game rendering
            renderer.BeginLayer(0);
            
            
            stateManager.Draw(gameTime);

            base.Draw(gameTime);
            renderer.EndLayer();

            if (stateManager.CurrentState is Play)
            {
                renderer.BeginLayer(1);

                Play playState = (Play)(stateManager.CurrentState);
            /*    playState.FireEmit.Draw(renderer.SpriteBatch);
                playState.SparkEmit.Draw(renderer.SpriteBatch);
                playState.ChickenFire.Draw(renderer.SpriteBatch);*/
                renderer.EndLayer();

                renderer.BeginLayer(2);
                playState.DrawChars();
                renderer.EndLayer();
            }

            renderer.RenderAll(Color.Silver);
        }
        
    }

     
}

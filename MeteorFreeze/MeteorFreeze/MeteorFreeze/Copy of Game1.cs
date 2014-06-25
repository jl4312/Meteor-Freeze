using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


using MeteorFreeze.Graphics;
using MeteorFreeze.GameObjects;
using MeteorFreeze.Character;
using MeteorFreeze.Gui;

namespace MeteorFreeze
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        
        static Platform platform;
        Guage guage;
        Munchlit player;
        FreezeShot shot;
        int count = 0;

        private KeyboardState kPrevState;
        private KeyboardState kState;

        MouseState ms;
        MouseState prevoiusms;

        double timer;

        private List<GameObject> childComponent;
        public Game1()
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
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 700;
            // TODO: Add your initialization logic here
            graphics.ApplyChanges();

            IsMouseVisible = true;

            timer = 0;
            base.Initialize();
            
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            childComponent = new List<GameObject>();

            platform = new Platform(new Rectangle(GraphicsDevice.Viewport.Width / 5 + 28, 10, 445, 650), this.Content);

            guage = new Guage(new Rectangle(platform.Position.X + platform.Position.Width + 2, 10, 100, 650), this.Content);
            Vector2 startingPos = platform.getMidPoint();
            player = new Munchlit(new Rectangle((int)startingPos.X, (int)startingPos.Y, 30, 30), this.Content);


            childComponent.Add(platform);
            childComponent.Add(guage);
            childComponent.Add(player);

            for (int i = platform.Position.X + 12; i <= platform.Position.X + platform.Position.Width - 24; i += 30)
            {
                Console.WriteLine(i);
                childComponent.Add(new Meteor(new Rectangle(i, platform.Position.Y + platform.Position.Height - 45, 30, 30), this.Content, 0));
            }

            


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

            
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
           
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            prevoiusms = ms;
            ms = Mouse.GetState();
            Point mouseLoc = new Point(ms.X, ms.Y);


            kPrevState = kState;
            kState = Keyboard.GetState();


            if (ms.LeftButton == ButtonState.Pressed && ms != prevoiusms)
            {
                if (guage.guageLoaded() && guage.Stockpile.Peek().FallingSpeed == 0)
                {
                    
                    float angle = 0;

                    if (player.getMidPoint().X > mouseLoc.X && player.getMidPoint().Y > mouseLoc.Y)
                    {
                        angle = (float)Math.Atan2(mouseLoc.Y - player.getMidPoint().Y, mouseLoc.X - player.getMidPoint().X);
                    }
                    else if (player.getMidPoint().X > mouseLoc.X && player.getMidPoint().Y < mouseLoc.Y)
                    {
                        angle = (float)Math.Atan2(player.getMidPoint().Y - mouseLoc.Y, mouseLoc.X - player.getMidPoint().X);
                    }
                    else if (player.getMidPoint().X < mouseLoc.X && player.getMidPoint().Y > mouseLoc.Y)
                    {
                        angle = (float)Math.Atan2(mouseLoc.Y - player.getMidPoint().Y, mouseLoc.X - player.getMidPoint().X);
                    }
                    else
                    {
                        angle = (float)Math.Atan2(mouseLoc.Y - player.getMidPoint().Y, mouseLoc.X - player.getMidPoint().X);
                    }
                    
                    shot = new FreezeShot(new Rectangle((int)player.getMidPoint().X - 2, (int)player.getMidPoint().Y - 2, 2, 2), this.Content, angle, guage.getAmmo());
                    childComponent.Add(shot);

                }
            }

            addMeteor();
            addAmmo();

            if (kPrevState.IsKeyDown(Keys.Space) && kState.IsKeyUp(Keys.Space))
            { 
                
                addAmmo();
                
                
            }




            count++;
            // TODO: Add your update logic here

            for (int i = 0; i < childComponent.Count; i++ )
            {
                if (GraphicsDevice.Viewport.Bounds.Intersects(childComponent[i].Position) || childComponent[i].Active)
                {
                    childComponent[i].Update(gameTime, childComponent);
                }
                else
                    childComponent.RemoveAt(i);
            }
           
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

           
            platform.Draw(spriteBatch);
            foreach (GameObject obj in childComponent)
            {
                obj.Draw(spriteBatch);
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected void addMeteor()
        {
            Random rand = new Random();
            int occurrance = 1000 - (int)timer;

            if (occurrance < 400)
            {
                occurrance = 400;
            }
            if (rand.Next(occurrance) == 0)
            {
                Rectangle pos = new Rectangle(platform.Position.X + rand.Next(platform.Position.Width - 25), platform.Position.Y, 25, 25);
                childComponent.Add(new Meteor(pos, this.Content));
            }
        }
        protected void addAmmo()
        {
            if (guage.Stockpile.Count <= 9)
            {
                
                Rectangle startingPosition = new Rectangle((int)(guage.getMidPoint().X - guage.Position.Width / 4), (int)guage.Position.Y, (int)guage.Position.Width / 2, (int)guage.Position.Width / 2);
                Color color;
                Random rand = new Random();

                if (rand.Next(5) == 0)
                {
                    color = Color.Azure;
                }
                else if (rand.Next(5) == 1)
                {
                    color = Color.Green;
                }
                else if (rand.Next(5) == 2)
                {
                    color = Color.Red;
                }
                else if (rand.Next(5) == 3)
                {
                    color = Color.Lavender;
                }
                else
                    color = Color.LightCoral;
                
                Ammo tmp = new Ammo(startingPosition, this.Content, color);
                guage.Stockpile.Enqueue(tmp);
                childComponent.Add(tmp);
            }
           
        }
    }
}

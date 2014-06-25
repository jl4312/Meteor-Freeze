using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using MeteorFreeze.Graphics;
using MeteorFreeze.GameObjects;
using MeteorFreeze.Character;
using MeteorFreeze.Gui;

using System.IO;

namespace MeteorFreeze.GameStates
{
    class Play : GameState
    {
        private static Platform platform;
        private Guage guage;
        private Munchlit player;
        private FreezeShot shot;
        private int count = 0;

        private KeyboardState kPrevState;
        private KeyboardState kState;

        private MouseState ms;
        private MouseState prevoiusms;

        private float timer;

        private List<GameObject> characters;
        private int level;
        private SpriteFont font;

        private int highScore;
        private IOManager ioManager;

        private Button btnExit;
        private Button btnMainMenu;

        private int amount;
       
        public Play(Game game, EffectRenderer renderer, StateManager parent)
            : base(game, renderer, parent)
        {
            timer = 0;
            characters = new List<GameObject>();
            level = 0;
        }

        public Play(Game game, EffectRenderer renderer, StateManager parent, int level)
            : base(game, renderer, parent)
        {
            timer = 0;
            characters = new List<GameObject>();
            this.level = level;
        }
        
        public override void Initialize()
        {
            childComponents = new List<GameObject>();
            ioManager = new IOManager();
            font = gameRef.Content.Load<SpriteFont>("SpriteFont1");

            platform = new Platform(new Rectangle(18, 40, 445, 650), gameRef.Content);
            guage = new Guage(new Rectangle(platform.Position.X + platform.Position.Width + 2, 40, 100, 650), gameRef.Content);
            Vector2 startingPos = platform.getMidPoint();
            player = new Munchlit(new Rectangle((int)startingPos.X, (int)startingPos.Y, 20, 20), gameRef.Content);

            childComponents.Add(platform);
            childComponents.Add(guage);
            childComponents.Add(player);

            for (int i = platform.Position.X + 12; i <= platform.Position.X + platform.Position.Width - 24; i += 30)
            {
                
                childComponents.Add(new Meteor(new Rectangle(i, platform.Position.Y + platform.Position.Height - 45, 30, 30), gameRef.Content, 0));
                childComponents.Add(new Meteor(new Rectangle(i, platform.Position.Y + platform.Position.Height - 75, 30, 30), gameRef.Content, 0));
                childComponents.Add(new Meteor(new Rectangle(i, platform.Position.Y + platform.Position.Height - 105, 30, 30), gameRef.Content, 0));
            }

            guiManager.AddLabel("HighScore", font, "Highscore: " + 0);
            guiManager["HighScore"].Position = new Vector2(guage.Position.X + guage.Position.Width + 10, 40.0f);
            guiManager["HighScore"].Size = new Vector2(200.0f, 75.0f);
            guiManager["HighScore"].Color = Color.Black;

            guiManager.AddLabel("level", font, "Level: " + level);
            guiManager["level"].Position = new Vector2(platform.Position.X + platform.Position.Width / 3 + 20, 10.0f);
            guiManager["level"].Size = new Vector2(150, 20.0f);
            guiManager["level"].Color = Color.Black;

            guiManager.AddLabel("time", font, "Timer: " + timer);
            guiManager["time"].Position = new Vector2(guage.Position.X + guage.Position.Width + 10, 70.0f);
            guiManager["time"].Size = new Vector2(200.0f, 75.0f);
            guiManager["time"].Color = Color.Black;

            guiManager.AddLabel("lives", font, "Lives: " + player.Lives);
            guiManager["lives"].Position = new Vector2(guage.Position.X + guage.Position.Width + 10, 100.0f);
            guiManager["lives"].Size = new Vector2(150.0f, 20.0f);
            guiManager["lives"].Color = Color.Black;

            guiManager.AddLabel("points", font, "Points: " + player.Point);
            guiManager["points"].Position = new Vector2(guage.Position.X + guage.Position.Width + 10, 130.0f);
            guiManager["points"].Size = new Vector2(150.0f, 20.0f);
            guiManager["points"].Color = Color.Black;

            guiManager.AddPicture("tmp", gameRef.Content.Load<Texture2D>("Freezling"));
            guiManager["tmp"].Position = new Vector2(guage.Position.X + guage.Position.Width + 30, 200.0f);
            guiManager["tmp"].Size = new Vector2(200.0f, 200.0f);

            guiManager.AddButton("pause", "PAUSE", guage.Position.X + guage.Position.Width + 10, 400, 200.0f, 75.0f);

            guiManager.AddButton("btnMainMenu", "Menu", guage.Position.X + guage.Position.Width + 10, 500, 200.0f, 75.0f);

            guiManager.AddButton("btnExit", "Exit", guage.Position.X + guage.Position.Width + 10, 600, 200.0f, 75.0f);

            btnMainMenu = guiManager["btnMainMenu"] as Button;
            btnMainMenu.Click += (control, button) =>
            {
                GameState MainMenu = this.parent[GameStateType.Playing];
                if (MainMenu != null)
                {
                    this.parent.PushState(MainMenu);

                    MainMenu MainMenuState = new MainMenu(this.gameRef, this.renderer, this.parent);
                    MainMenu.Initialize();
                    this.parent.PushState(MainMenuState);


                }
            };

            btnExit = guiManager["btnExit"] as Button;
            btnExit.Click += (control, button) =>
            {
                gameRef.Exit();
            };

            try
            {
                highScore = ioManager.LoadHighScore();
            }
            catch (Exception)
            {
                ioManager.SaveHighScore(0);
                highScore = 0;
            }

          


        }

        public override void Update(GameTime gameTime)
        {
            amount = 0;
            for (int i = 0; i < childComponents.Count; i++)
            {
                if (childComponents[i] is Meteor)
                {
                    amount += (childComponents[i] as Meteor).CurrentHP;
                }
            }
            ChangeText(guiManager["points"] as Label, "" + amount);

            addAmmo();

            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            //updates the text for time
            ChangeText(guiManager["time"] as Label, "Time: " + string.Format("{0:0.00}", time) + " sec");
            timer = time;

            prevoiusms = ms;
            ms = Mouse.GetState();
            Point mouseLoc = new Point(ms.X, ms.Y);


            kPrevState = kState;
            kState = Keyboard.GetState();


            if (ms.LeftButton == ButtonState.Pressed/* && ms != prevoiusms*/)
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

                    shot = new FreezeShot(new Rectangle((int)player.getMidPoint().X - 2, (int)player.getMidPoint().Y - 2, 15, 15), gameRef.Content, angle, guage.getAmmo());
                    childComponents.Add(shot);

                }
            }
            if (amount < 5)
            {
                addMeteor();
       

            }
            if (kPrevState.IsKeyDown(Keys.Space) && kState.IsKeyUp(Keys.Space))
            {

                addMeteor2();


            }

            count++;
            // TODO: Add your update logic here

            for (int i = 0; i < childComponents.Count; i++)
            {
                Rectangle windowBounds = new Rectangle(0, 0, 1000, 700);
                if (windowBounds.Contains(childComponents[i].Position) && childComponents[i].Active)
                    childComponents[i].Update(gameTime, childComponents);
                
                else
                    childComponents.Remove(childComponents[i]);
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            

            platform.Draw(renderer.SpriteBatch);
            foreach (GameObject obj in childComponents)
            {
                obj.Draw(renderer.SpriteBatch);
            }
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected void addMeteor()
        {
            
            Random rand = new Random();
            Random rand1 = new Random();
            
            int occurrance = 1500 - (int)timer;

            if (occurrance < 400)
            {
                occurrance = 400;
            }

            if (rand1.Next(occurrance) % 300 == 0)
            {
                
                rand1 = new Random();
                
                Console.WriteLine(rand.Next((platform.Position.Width - 12)));
                Rectangle pos = new Rectangle(platform.Position.X + 12 + rand.Next(platform.Position.Width - 12), platform.Position.Y, 25, 25);

                childComponents.Add(new Meteor(pos, gameRef.Content));
                amount += (childComponents[childComponents.Count - 1] as Meteor).CurrentHP;
            }


        }

        protected void addMeteor2()
        {
          

                Random rand1 = new Random();

    
                Rectangle pos = new Rectangle(platform.Position.X + 12 + rand1.Next(platform.Position.Width - 12), platform.Position.Y, 25, 25);

                childComponents.Add(new Meteor(pos, gameRef.Content));
         

        }

        private void ChangeText(Label label, string text)
        {
            if (label == null)
                return;

            label.Text = text;
        }


        protected void addAmmo()
        {
            if (guage.Stockpile.Count < 11)
            {

                Rectangle startingPosition = new Rectangle((int)(guage.getMidPoint().X - guage.Position.Width / 4), (int)guage.Position.Y, (int)guage.Position.Width / 2, (int)guage.Position.Width / 2);
                Color color = Color.White;
                Random rand = new Random();
/*
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
                */
                Ammo tmp = new Ammo(startingPosition, gameRef.Content, color);
                guage.Stockpile.Enqueue(tmp);
                childComponents.Add(tmp);
            }

        }

        public void DrawChars()
        {
            foreach (GameObject character in characters)
            {
                character.Draw(renderer.SpriteBatch);
            }
        }
    }
}

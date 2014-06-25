using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MeteorFreeze.Graphics;
using MeteorFreeze.Gui;
using MeteorFreeze;

namespace MeteorFreeze.GameStates
{
    class MainMenu : GameState
    {
        private Button btnPlay;
        private Button btnExit;
        private Button btnRules;
        private Button btnOptions;
        private Button btnSurvival;

        private Picture titlePic;
        private Picture characterPic;

        public MainMenu(Game game, EffectRenderer renderer, StateManager parent)
            : base(game, renderer, parent)
        {
            titlePic = new Picture(game, guiManager, gameRef.Content.Load<Texture2D>("Gui/button-up"));
            characterPic = new Picture(game, guiManager, gameRef.Content.Load<Texture2D>("Freezling"));

            guiManager.AddPicture("titlePic", gameRef.Content.Load<Texture2D>("Gui/button-up"));
            guiManager["titlePic"].Position = new Vector2(400, 40.0f);
            guiManager["titlePic"].Size = new Vector2(400, 100);

            guiManager.AddButton("btnPlay", "Play", 60, guiManager["titlePic"].Position.Y + 100, 300, 75);
            guiManager.AddButton("btnSurvival", "Survival", 60, guiManager["btnPlay"].Position.Y + 100, 300, 75);
            guiManager.AddButton("btnRules", "Rules", 60, guiManager["btnSurvival"].Position.Y + 100, 300, 75);
            guiManager.AddButton("btnOptions", "Options", 60, guiManager["btnRules"].Position.Y + 100, 300, 75);
            guiManager.AddButton("btnExit", "Exit", 60, guiManager["btnOptions"].Position.Y + 100, 300, 75);

            guiManager.AddPicture("characterPic", gameRef.Content.Load<Texture2D>("Freezling"));
            guiManager["characterPic"].Position = new Vector2(guiManager["titlePic"].Position.X + 75, guiManager["titlePic"].Position.Y + 225);
            guiManager["characterPic"].Size = new Vector2(450, 450);

            btnPlay = guiManager["btnPlay"] as Button;
            btnPlay.Click += (control, button) =>
                {
                    GameState Play = this.parent[GameStateType.Playing];
                    if (Play != null)
                    {
                        this.parent.PushState(Play);

                        Play playState = new Play(this.gameRef, this.renderer, this.parent);
                        playState.Initialize();
                        this.parent.PushState(playState);
                    
                    
                    }

                };

            btnOptions = guiManager["btnOptions"] as Button;
            btnOptions.Click += (control, button) =>
            {
                GameState Options = this.parent[GameStateType.OptionsMenu];
                if (Options != null)
                {
                    this.parent.PushState(Options);

                    EndState endState = new EndState(this.gameRef, this.renderer, this.parent);
                    endState.Initialize();
                    this.parent.PushState(endState);


                }

            };

            btnExit = guiManager["btnExit"] as Button;
            btnExit.Click += (control, button) =>
            {
                gameRef.Exit();
            };
            
        
        }

    }
}

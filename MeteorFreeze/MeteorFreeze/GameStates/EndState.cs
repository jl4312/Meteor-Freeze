using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using MeteorFreeze.Graphics;
using MeteorFreeze.GameObjects;
using MeteorFreeze.Character;
using MeteorFreeze.Gui;


namespace MeteorFreeze.GameStates
{
    public class EndState : GameState
    {

        private Munchlit munchlit;

        private float destination;
        private Random rand;
        
        public EndState(Game game, EffectRenderer renderer, StateManager parent) :
            base(game, renderer, parent)
        {
            rand = new Random();
            

            
        }

        public override void Initialize()
        {
            munchlit = new Munchlit(new Rectangle(50, 50, 30, 30), gameRef.Content);
            destination = 50;
            childComponents.Add(munchlit);
            for (int i = 0; i < 800; i += 30)
            {
                childComponents.Add(new Meteor(new Rectangle(i, 600, 30, 30), gameRef.Content, 0, new Vector2(0, 0)));


            }

            childComponents.Add(new Meteor(new Rectangle(50, 570, 30, 30), gameRef.Content, 0));
            childComponents.Add(new Meteor(new Rectangle(210, 570, 30, 30), gameRef.Content, 0));
            childComponents.Add(new Meteor(new Rectangle(250, 570, 30, 30), gameRef.Content, 0));
            childComponents.Add(new Meteor(new Rectangle(380, 570, 30, 30), gameRef.Content, 0));
            childComponents.Add(new Meteor(new Rectangle(600, 570, 30, 30), gameRef.Content, 0));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        { 

            if (munchlit.Position.X < destination)
            {
                munchlit.VelocityX = 1;
            }
            else if (munchlit.Position.X > destination)
            {
                munchlit.VelocityX =  -1;
            }
            else
            {
                destination = rand.Next(30, 740);
            }

            if(rand.Next(1000) == 0)
           childComponents.Add(new Meteor(new Rectangle(rand.Next(800), 0, 30, 30), gameRef.Content));

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

           
            foreach (GameObject obj in childComponents)
            {
                obj.Draw(renderer.SpriteBatch);
            }
            base.Draw(gameTime);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MeteorFreeze.GameStates
{
    /// <summary>
    /// An enumeration of possible game state types.
    /// </summary>
    public enum GameStateType
    {
        MainMenu,
        OptionsMenu,
        LevelSetup,
        Playing,
        Paused,
        RaceFinished,
        CharacterSelect
    }

    public class StateManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Stack<GameState> gameStates;
        private Dictionary<GameStateType, GameState> stateTypes;

        /// <summary>
        /// The current running gameState
        /// </summary>
        public GameState CurrentState
        {
            get
            {
                if (gameStates.Count > 0)
                    return gameStates.Peek();
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets the game state registered with the given state type (if one is registered).
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameState this[GameStateType type]
        {
            get
            {
                if (stateTypes.ContainsKey(type))
                {
                    return stateTypes[type];
                }
                return null;
            }
        }

        /// <summary>
        /// Creates a new game state manager.
        /// </summary>
        /// <param name="game"></param>
        public StateManager(Game game)
            : base(game)
        {
            gameStates = new Stack<GameState>();
            stateTypes = new Dictionary<GameStateType, GameState>();
        }

        public override void Initialize()
        {
            // need to initialize ALL of the registered states
            foreach (GameState state in stateTypes.Values)
            {
                state.Initialize();
            }
            base.Initialize();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (CurrentState != null)
            {
                CurrentState.Update(gameTime);
            }
            // base.Update(gameTime);
        }

        /// <summary>
        /// Gets the game state registered to the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameState GetRegisteredState(GameStateType type)
        {
            if (stateTypes.ContainsKey(type))
            {
                return stateTypes[type];
            }
            return null;
        }

        /// <summary>
        /// Draws the current game state.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (CurrentState != null)
            {
                CurrentState.Draw(gameTime);
            }
            //base.Draw( gameTime );
        }

        /// <summary>
        /// Registers a game state with a given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public bool RegisterState(GameStateType type, GameState state)
        {
            if (stateTypes.ContainsKey(type))
            {
                return false;
            }
            stateTypes.Add(type, state);
            return true;
        }

        /// <summary>
        /// Removes the top state in the gamestates stack
        /// </summary>
        public void PopState()
        {
            if (gameStates.Count > 0)
                gameStates.Pop();
        }

        /// <summary>
        /// Adds the given state to the 
        /// </summary>
        public void PushState(GameState newState)
        {
            gameStates.Push(newState);
        }

        /// <summary>
        /// Clears the stack cleanly and adds the new state to the stack
        /// </summary>
        public void ChangeState(GameState newState)
        {
            // why do this? --Rich
            while (gameStates.Count > 0)
                PopState();
            // -------------------

            PushState(newState);
            CurrentState.Initialize();
        }
    }
}

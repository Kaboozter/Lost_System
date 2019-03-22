using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace SHMUP_Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public GraphicsDeviceManager myGraphics;
        SpriteBatch mySpriteBatch;

        MainMenu myMenu;
        public States myCurState;
        Stack<States> myStateStack;

        public Game1()
        {
            myGraphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 480,
                PreferredBackBufferWidth = 800
            };

            Content.RootDirectory = "Content";

            myStateStack = new Stack<States>();

        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
            //myGraphics.PreferredBackBufferHeight = 480;
            //myGraphics.PreferredBackBufferWidth = 800;
            myMenu = new MainMenu(this, GraphicsDevice, Content);
            myCurState = myMenu;
            myStateStack.Push(myMenu);
        }

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime someGameTime)
        {
            if (myStateStack.Peek().Update(someGameTime) == false)
            {
                myStateStack.Pop();
            }

            base.Update(someGameTime);
        }

        protected override void Draw(GameTime someGameTime)
        {
            myStateStack.Peek().Draw(someGameTime, mySpriteBatch);

            base.Draw(someGameTime);
        }

        // Used to remove a state that is unused.
        public void PopStack()
        {
            myStateStack.Pop();
        }

        // Used to change a state.
        public void ChangeState(States state)
        {
            myCurState = state;
            myStateStack.Push(state);
        }
    }
}

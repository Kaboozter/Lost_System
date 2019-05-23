using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;



namespace SHMUP_Project
{
    class PauseMenu : States
    {
        #region Variables

        float myTimer = 0f;
        bool myPaused = true;
        List<Components> myButtons;
        Texture2D myScreen;
        Color myWhite = Color.White;
        Vector2 myOverPos = new Vector2(320, 190);
        Vector2 myCenterPos = new Vector2(320, 240);
        Vector2 myUnderPos = new Vector2(320, 290);
        Song mySong1;
        Song mySong2;
        Game1 myGame;

        #endregion

        public PauseMenu(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager someContent) : base(aGame, aGraphicsDevice, someContent)
        {
            #region Load

            Texture2D buttonText = someContent.Load<Texture2D>("button");
            SpriteFont buttonFont = someContent.Load<SpriteFont>("font");
            myScreen = someContent.Load<Texture2D>("paused");
            myGame = aGame;
            //mySong1 = content.Load<Song>("inGameMusic");
            //mySong2 = content.Load<Song>("menuMusic");

            #endregion

            MediaPlayer.IsRepeating = true;

            #region Creating Buttons

            Buttons resumeButton = new Buttons(buttonText, buttonFont)
            {
                AccessPos = myOverPos,
                AccessText = "Resume",
            };
            resumeButton.Click += ResumeButton_Click;

            Buttons menuButton = new Buttons(buttonText, buttonFont)
            {
                AccessPos = myCenterPos,
                AccessText = "Main Menu",
            };
            menuButton.Click += MenuButton_Click;

            Buttons quitButton = new Buttons(buttonText, buttonFont)
            {
                AccessPos = myUnderPos,
                AccessText = "Quit",
            };
            quitButton.Click += QuitButton_Click;

            myButtons = new List<Components>()
            {
                resumeButton,
                menuButton,
                quitButton,
            };

            #endregion
        }

        #region Button Clicks

        private void QuitButton_Click(object sender, EventArgs e)
        {
             myGame.Exit();
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                myGame.PopStack();
            }
            //MediaPlayer.Play(mySong2);
        }

        private void ResumeButton_Click(object sender, EventArgs e)
        {
            myPaused = false;
            MediaPlayer.Resume();
        }

        #endregion

        public override void Draw(GameTime someGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();
            aSpriteBatch.Draw(myScreen, new Rectangle(0, 0, 800, 480), myWhite);
            foreach (Buttons component in myButtons)
            {
                component.Draw(aSpriteBatch, someGameTime);
            }
            aSpriteBatch.End();
        }

        public override bool Update(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            myTimer += deltaTime;
            myGame.myGraphics.PreferredBackBufferWidth = 800;
            myGame.myGraphics.PreferredBackBufferHeight = 480;
            myGame.myGraphics.ApplyChanges();

            foreach (Buttons component in myButtons)
            {
                component.Update(gameTime);
            }

            if (keys.IsKeyDown(Keys.Escape) && myTimer > 1)
            {
                myTimer = 0f;
                myPaused = false;
                MediaPlayer.Resume();
                return false;
            }

            if (myPaused == false)
            {
                return false;
            }

            return true;
        }

    }
}


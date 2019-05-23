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
    class ShipSelect : States
    {
        #region Variables

        float myTimer = 0f;
        int myShipSelected = 0;
        bool myPaused = true;
        List<Components> myButtons;
        List<Texture2D> myShips = new List<Texture2D>();
        List<int> myShipParts;
        Texture2D myScreen;
        Color myWhite = Color.White;
        Vector2 myOverPos = new Vector2(170, 350);
        Vector2 myCenterPos = new Vector2(470, 350);
        Vector2 myUnderPos = new Vector2(320, 290);
        Texture2D myButtonText;
        SpriteFont myButtonFont;
        Song mySong1;
        Song mySong2;
        Game1 myGame;
        MainMenu myMainMenu;

        #endregion

        public ShipSelect(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager someContent) : base(aGame, aGraphicsDevice, someContent)
        {
            #region Load

            myButtonText = someContent.Load<Texture2D>("button");
            myButtonFont = someContent.Load<SpriteFont>("font");
            myGame = aGame;

            myShips.Add(myGame.Content.Load<Texture2D>("squareship"));
            myShips.Add(myGame.Content.Load<Texture2D>("squareship_Green"));
            myScreen = someContent.Load<Texture2D>("paused");
            //mySong1 = content.Load<Song>("inGameMusic");
            //mySong2 = content.Load<Song>("menuMusic");
            myShipParts = new List<int>
            {
                5,
                0
            };

            #endregion

            MediaPlayer.IsRepeating = true;

            #region Creating Buttons

            Buttons BackButton = new Buttons(myButtonText, myButtonFont)
            {
                AccessPos = myOverPos,
                AccessText = "Main Menu",
            };
            BackButton.Click += BackButton_Click;

            Buttons ConfirmButton = new Buttons(myButtonText, myButtonFont)
            {
                AccessPos = myCenterPos,
                AccessText = "Confirm selection",
            };
            ConfirmButton.Click += ConfirmButton_Click;

            myButtons = new List<Components>()
            {
                BackButton,
                ConfirmButton,
            };

            #endregion
        }

        #region Button Clicks


        private void BackButton_Click(object sender, EventArgs e)
        {
            myGame.PopStack();
            SaveGameData.Save(); MediaPlayer.Play(mySong2);
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            MainMenu.myShipTexture = myShips[myShipSelected];
            MainMenu.myShip = myShipSelected;
            myGame.PopStack();
            SaveGameData.Save();
            //MediaPlayer.Play(mySong2);
        }

        #endregion

        public override void Draw(GameTime someGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();
            aSpriteBatch.Draw(myScreen, new Rectangle(0, 0, 800, 480), myWhite);
            aSpriteBatch.Draw(myShips[myShipSelected], new Rectangle(350,200,100,100),myWhite);
            foreach (Buttons component in myButtons)
            {
                component.Draw(aSpriteBatch, someGameTime);
            }
            aSpriteBatch.DrawString(myButtonFont,myShipParts[myShipSelected] + "/5",new Vector2(390,340),Color.White);
            aSpriteBatch.End();
        }

        public override bool Update(GameTime gameTime)
        {
            KeyboardState tempKeys = Keyboard.GetState();
            float tempDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            myTimer += tempDeltaTime;
            myGame.myGraphics.PreferredBackBufferWidth = 800;
            myGame.myGraphics.PreferredBackBufferHeight = 480;
            myGame.myGraphics.ApplyChanges();

            if ((tempKeys.IsKeyDown(Keys.A) || tempKeys.IsKeyDown(Keys.Left)))
            {
                if (myShipSelected == 0)
                {
                    myShipSelected = 1;
                }
                else
                {
                    myShipSelected--;
                }

            }
            if (tempKeys.IsKeyDown(Keys.D) || tempKeys.IsKeyDown(Keys.Right))
            {
                if (myShipSelected == 1)
                {
                    myShipSelected = 0;
                }
                else
                {
                    myShipSelected++;
                }
            }

            foreach (Buttons component in myButtons)
            {
                component.Update(gameTime);
            }

            if (tempKeys.IsKeyDown(Keys.Escape) && myTimer > 1)
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


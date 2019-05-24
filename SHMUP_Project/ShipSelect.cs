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
using System.IO;

namespace SHMUP_Project
{
    class ShipSelect : States
    {
        #region Variables

        float myTimer = 0f, myPressTimer = 0;
        int myShipSelected = 0;
        int[] mySaveData = new int[5];
        bool myPaused = true;
        string myShipExplanation = " ", myLockedorNot = " ";
        List<Components> myButtons;
        List<Texture2D> myShips = new List<Texture2D>();
        List<int> myShipParts;
        Texture2D myScreen;
        Color myWhite = Color.White;
        Vector2 myOverPos = new Vector2(170, 350);
        Vector2 myCenterPos = new Vector2(470, 350);
        Vector2 myUnderPos = new Vector2(320, 290);
        Texture2D myButtonText, myArrowTexture;
        SpriteFont myButtonFont;
        Song mySong1;
        Song mySong2;
        Game1 myGame;
        MainMenu myMainMenu;

        #endregion

        public ShipSelect(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager someContent) : base(aGame, aGraphicsDevice, someContent)
        {
            mySaveData = SaveGameData.GetSaveData();

            #region Load

            myButtonText = someContent.Load<Texture2D>("button");
            myButtonFont = someContent.Load<SpriteFont>("font");
            myGame = aGame;

            myShips.Add(myGame.Content.Load<Texture2D>("squareship"));
            myShips.Add(myGame.Content.Load<Texture2D>("squareship_Green"));
            myShips.Add(myGame.Content.Load<Texture2D>("squareship_Blue"));
            myScreen = someContent.Load<Texture2D>("Hangar");
            //mySong1 = content.Load<Song>("inGameMusic");
            //mySong2 = content.Load<Song>("menuMusic");
            myShipParts = new List<int>
            {
                mySaveData[0],
                mySaveData[1],
                mySaveData[2]
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


        private void BackButton_Click(object aSender, EventArgs anE)
        {
            myGame.PopStack();
            SaveGameData.Save(); MediaPlayer.Play(mySong2);
        }

        private void ConfirmButton_Click(object aSender, EventArgs anE)
        {
            if (mySaveData[myShipSelected] == 5)
            {
                myGame.PopStack();
                SaveGameData.mySplittedSaveData[3] = myShipSelected;
                SaveGameData.Save();
                //MediaPlayer.Play(mySong2);
            }

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
            aSpriteBatch.DrawString(myButtonFont,myShipParts[myShipSelected] + "/5", new Vector2(myGame.myGraphics.PreferredBackBufferWidth * 0.5f - (myButtonFont.MeasureString("0/0").X * 0.5f), 360), Color.White);
            aSpriteBatch.DrawString(myButtonFont, "Parts:", new Vector2(myGame.myGraphics.PreferredBackBufferWidth * 0.5f - (myButtonFont.MeasureString("Parts:").X * 0.5f), 330), Color.White);
            aSpriteBatch.DrawString(myButtonFont, myLockedorNot, new Vector2(myGame.myGraphics.PreferredBackBufferWidth * 0.5f - (myButtonFont.MeasureString(myLockedorNot).X * 0.5f), 50), Color.White);
            aSpriteBatch.DrawString(myButtonFont,myShipExplanation, new Vector2(myGame.myGraphics.PreferredBackBufferWidth * 0.5f - (myButtonFont.MeasureString(myShipExplanation).X * 0.5f), 170), Color.White);
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


            if (myShipSelected == 0)
            {
                myShipExplanation = "Straight single target fire, tripple fire, powerups";
            }
            if (myShipSelected == 1)
            {
                myShipExplanation = "Curving shots, burst fire, manual control over direction";
            }
            if (myShipSelected == 2)
            {
                myShipExplanation = "Aimable shots, penetrating shots, free movement";
            }
            if(myShipParts[myShipSelected] < 5)
            {
                myLockedorNot = "Locked";
            }
            if (myShipParts[myShipSelected] >= 5)
            {
                myLockedorNot = "Unlocked";
            }

            if ((tempKeys.IsKeyDown(Keys.A) || tempKeys.IsKeyDown(Keys.Left)) && myPressTimer <= 0)
            {
                if (myShipSelected == 0)
                {
                    myShipSelected = 2;
                }
                else
                {
                    myShipSelected--;
                }
                myPressTimer = 0.25f;
            }
            if ((tempKeys.IsKeyDown(Keys.D) || tempKeys.IsKeyDown(Keys.Right)) && myPressTimer <= 0)
            {
                if (myShipSelected == 2)
                {
                    myShipSelected = 0;
                }
                else
                {
                    myShipSelected++;
                }
                myPressTimer = 0.25f;
            }
            myPressTimer -= tempDeltaTime;

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


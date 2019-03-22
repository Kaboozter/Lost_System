﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace SHMUP_Project
{
    class MainMenu : States
    {
        private List<Components> myButtons;
        Texture2D myMenu;
        Vector2 myOverPos = new Vector2(325, 190);
        Vector2 myCenterPos = new Vector2(325, 240);
        Vector2 myUnderPos = new Vector2(325, 290);
        SpriteFont myButtonFont;
        Song mySong;

        public MainMenu(Game1 aGame1, GraphicsDevice someGraphics, ContentManager someContent) : base(aGame1, someGraphics, someContent)
        {

            Texture2D buttonTexture = someContent.Load<Texture2D>("button");
            myButtonFont = someContent.Load<SpriteFont>("font");
            myMenu = someContent.Load<Texture2D>("menu");
            mySong = someContent.Load<Song>("menuMusic");

            MediaPlayer.Play(mySong);
            MediaPlayer.IsRepeating = true;

            #region Creating Button

            Buttons startHardButtons = new Buttons(buttonTexture, myButtonFont)
            {
                AccessPos = myOverPos,
                AccessText = "Normal Mode",
            };
            startHardButtons.Click += StartHardButtons_Click;

            Buttons startEzButtons = new Buttons(buttonTexture, myButtonFont)
            {
                AccessPos = myCenterPos,
                AccessText = "Baby Mode",
            };
            startEzButtons.Click += StartButtons_Click;

            Buttons quitButtons = new Buttons(buttonTexture, myButtonFont)
            {
                AccessPos = myUnderPos,
                AccessText = "Quit",
            };
            quitButtons.Click += QuitButtons_Click;

            Buttons readMeButtons = new Buttons(buttonTexture, myButtonFont)
            {
                AccessPos = new Vector2(150, 200),
                AccessText = "Read the readme"
            };
            readMeButtons.Click += ReadMeButtons_CLick;

            myButtons = new List<Components>()
            {
                startEzButtons,
                quitButtons,
                startHardButtons,
                readMeButtons,
            };

            #endregion

        }

        #region Buttons clicking

        private void StartHardButtons_Click(object sender, EventArgs e)
        {
            myGame.ChangeState(new GameState(myGame, myGraphicsDevice, myContentManager));
        }

        private void QuitButtons_Click(object sender, EventArgs e)
        {
            myGame.Exit();
        }

        private void StartButtons_Click(object sender, EventArgs e)
        {
            myGame.ChangeState(new GameState(myGame, myGraphicsDevice, myContentManager));
        }

        private void ReadMeButtons_CLick(object sender, EventArgs e)
        {
            Process.Start("https://github.com/FalasFry/Frustration.exe/blob/master/README.md");


        }
        #endregion

        public override void Draw(GameTime someGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();
            aSpriteBatch.Draw(myMenu, new Rectangle(0, 0, 800, 480), Color.White);
            //spriteBatch.DrawString(buttonFont,"Read the readme",new Vector2(150,200),Color.HotPink);
            foreach (Buttons component in myButtons)
            {
                component.Draw(aSpriteBatch, someGameTime);
            }

            aSpriteBatch.End();
        }

        public override bool Update(GameTime gameTime)
        {
            for (int i = 0; i < myButtons.Count; i++)
            {
                myButtons[i].Update(gameTime);
            }
            return true;
        }
    }
}

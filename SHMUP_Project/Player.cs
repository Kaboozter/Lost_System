using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SHMUP_Project
{
    class Player
    {
        public float mySpeed = 5;
        public float myRotation = 0;
        public float myAmmo = 20;
        public float myHp = 5;
        public float myAttackSpeed = 0.5f;
        public float myAttackTimer;
        public bool myDead = false;
        public bool myNormalDiff = false;

        public Vector2 myDir;
        public Vector2 myPosition;
        public Vector2 myOffset;
        public Texture2D myTexture;
        public Rectangle myRectangle;
        Game1 myGame1;

        public Player(Texture2D aTexture, Game1 aGame)
        {
            myGame1 = aGame;
            myTexture = aTexture;
            myOffset = ((myTexture.Bounds.Size.ToVector2() * 0.5f));
            myPosition = new Vector2(300, 1000);
            myRectangle = new Rectangle((myOffset).ToPoint(), (myTexture.Bounds.Size.ToVector2()).ToPoint());
        }

        public void Update(Point aBackgroundDir)
        {
            Movement();

            myRectangle.Location = myPosition.ToPoint();

            #region Worldwalls
            if (myPosition.X < 0)
            {
                myPosition.X += mySpeed;
            }
            if (myPosition.X + myTexture.Width > myGame1.myGraphics.PreferredBackBufferWidth)
            {
                myPosition.X -= mySpeed;
            }
            if (myPosition.Y < 0)
            {
                myPosition.Y += mySpeed;
            }
            if (myPosition.Y + myTexture.Height > myGame1.myGraphics.PreferredBackBufferHeight)
            {
                myPosition.Y -= mySpeed;
            }

            if (aBackgroundDir == new Point(1,0))
            {
                myRotation = ((float)Math.PI * 3) / 2;
            }
            if (aBackgroundDir == new Point(-1,0))
            {
                myRotation = (float)Math.PI / 2;
            }
            if (aBackgroundDir == new Point(0,1))
            {
                myRotation = 0;
            }
            if (aBackgroundDir == new Point(0,-1))
            {
                myRotation = (float)Math.PI;
            }

            if (aBackgroundDir == new Point(1,0) && myPosition.X < (myGame1.myGraphics.PreferredBackBufferWidth-myTexture.Width))
            {
                myPosition.X += mySpeed * 2;
            }
            if (aBackgroundDir == new Point(-1,0) && myPosition.X > (0))
            {
                myPosition.X -= mySpeed * 2;
            }
            if (aBackgroundDir == new Point(0,1) && myPosition.Y < (myGame1.myGraphics.PreferredBackBufferHeight - myTexture.Height))
            {
                myPosition.Y += mySpeed * 2;
            }
            if (aBackgroundDir == new Point(0,-1) && myPosition.Y > (0))
            {
                myPosition.Y -= mySpeed * 2;
            }
            #endregion
        }

        public void Movement()
        {
            KeyboardState tempKeyState = Keyboard.GetState();
            Vector2 tempDir = new Vector2();

            if (tempKeyState.IsKeyDown(Keys.A))
            {
                tempDir.X = -1;
            }
            if (tempKeyState.IsKeyDown(Keys.D))
            {
                tempDir.X = 1;
            }
            if (tempKeyState.IsKeyDown(Keys.W))
            {
                tempDir.Y = -1;
            }
            if (tempKeyState.IsKeyDown(Keys.S))
            {
                tempDir.Y = 1;
            }
            if (tempDir.X > 1f || tempDir.Y > 1f)
            {
                tempDir.Normalize();
            }

            if (tempDir == Vector2.Zero)
            {
                tempDir = new Vector2(1, 0);
            }
            if (tempKeyState.IsKeyDown(Keys.S) || tempKeyState.IsKeyDown(Keys.W) || tempKeyState.IsKeyDown(Keys.D) || tempKeyState.IsKeyDown(Keys.A))
            {
                myPosition += (tempDir * mySpeed);
            }


           // myRotation = (float)Math.Atan2(tempDir.X, tempDir.Y) * -1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(myTexture, myPosition + myOffset, null, Color.White, myRotation, myOffset, 1f, SpriteEffects.None, 0);
        }
    }
}

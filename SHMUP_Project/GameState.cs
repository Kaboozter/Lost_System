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


namespace SHMUP_Project
{
    class GameState : States
    {
        float attackTimer = 10;
        float moveTimer = 15;
        float pressTimer;
        float timeElapsed;
        float smartPercent;
        float delay = 2f;
        float remainingDelay = 1.5f;
        float enemyCount = 3;
        float enemiesPerLine = 1;
        float speed = 1;
        float myTimer, myOriginalTimer = 10;
        

        float health;
        int myBackY1, myBackY2, myBackY3, myBackWidth = 1080;
        Point myBackgroundDir = new Point(0,1);

        public float score;
        float invTimer = 1;
        bool manualSpawning = true;
        bool inv;
        int wait = 0;
        string powerupTimer = "0";
        List<Bullet> myBullets;
        List<Enemy> myEnemies;
        List<Timer> myTimers = new List<Timer>();
        List<Point> myStarsFront = new List<Point>();
        List<Point> myStarsMiddle = new List<Point>();
        List<Point> myStarsBack = new List<Point>();


        Player myPlayer;
        Enemy myEnemy;
        Texture2D myBulletTexture;
        Texture2D myBackSpace1,myBackSpace2;
        Random myRnd = new Random();
        SpriteFont myFont;
        Texture2D myPowerupsTexture;
        Texture2D myEnemyTexture;
        Texture2D myStarTexture;
        Song mySong;
        Color myColor = Color.White;

        public GameState(Game1 aGame, GraphicsDevice someGraphics, ContentManager someContent) : base(aGame, someGraphics, someContent)
        {
            myBackSpace1 = myGame.Content.Load<Texture2D>("stars");
            myBulletTexture = myGame.Content.Load<Texture2D>("ball.1");
            myEnemyTexture = myGame.Content.Load<Texture2D>("GreenDude");
            myStarTexture = myGame.Content.Load<Texture2D>("strass");
            myPlayer = new Player(myGame.Content.Load<Texture2D>("squareship"), myGame)
            {
                myHp = health,
            };
            myEnemies = new List<Enemy>
            {
                new CoolEnemy(myEnemyTexture, new Vector2(myGame.myGraphics.PreferredBackBufferWidth / 2, 0), new Vector2(0,1), 5, new Vector2(0.2f, 0.2f), 0, Color.White,1,this)

            };
            myGame.myGraphics.PreferredBackBufferHeight = 800;
            myGame.myGraphics.PreferredBackBufferWidth = 1080;
            myGame.myGraphics.ApplyChanges();
            myBackY1 = 0;
            myBackY2 = 480;
            myBackY3 = 960;
            myBullets = new List<Bullet>
            {
                new Bullet(2, new Vector2(1, -1), myBulletTexture, new Vector2(3000, 3000), 1, Color.Black, 1)
            };
            myBackSpace1 = GenerateStarMap(800, 1080, 1000, new Color(255, 255, 255, 255));
            myBackSpace2 = GenerateStarMap(800, myBackWidth, 1000, new Color(255, 255, 255, 200));
            for (int i = 0; i < 600; i++)
            {
                myStarsFront.Add(new Point(myRnd.Next(0,myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight)));
                myStarsMiddle.Add(new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight)));
                myStarsBack.Add(new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight)));

            }
        }

        public override bool Update(GameTime someGameTime)
        {
            float tempDeltaTime = (float)someGameTime.ElapsedGameTime.TotalSeconds;
            MouseState tempMouse = Mouse.GetState();
            KeyboardState tempKeyState = Keyboard.GetState();
            myPlayer.Update(myBackgroundDir);

            for (int i = 0; i < myEnemies.Count; i++)
            {
                myEnemies[i].Update(someGameTime);
            }
            if (myTimers.Count != 0)
            {
                for (int i = myTimers.Count-1; i > 0; i--)
                {
                    myTimers[i].Update(someGameTime);
                }
            }


            if (myTimer <= 0)
            {
                if (myBackgroundDir == new Point(0,1))
                {
                    myEnemies.Add(new CoolEnemy(myEnemyTexture, new Vector2(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth + 1), 0), new Vector2(0,1), 5, new Vector2(0.4f, 0.4f), 0, Color.Black, 1, this));
                }
                if (myBackgroundDir == new Point(0, -1))
                {
                    myEnemies.Add(new CoolEnemy(myEnemyTexture, new Vector2(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth + 1), myGame.myGraphics.PreferredBackBufferHeight), new Vector2(0, -1), 5, new Vector2(0.4f, 0.4f), (float)Math.PI, Color.Black, 1,this));
                }
                if (myBackgroundDir == new Point(1, 0))
                {
                    myEnemies.Add(new CoolEnemy(myEnemyTexture, new Vector2(0, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight + 1)), new Vector2(1, 0), 5, new Vector2(0.4f, 0.4f), ((float)Math.PI*3) / 2, Color.Black, 1,this));
                }
                if (myBackgroundDir == new Point(-1, 0))
                {
                    myEnemies.Add(new CoolEnemy(myEnemyTexture, new Vector2(myGame.myGraphics.PreferredBackBufferWidth, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight + 1)), new Vector2(-1, 0), 5, new Vector2(0.4f, 0.4f), ((float)Math.PI) / 2, Color.Black, 1,this));
                }
                myTimer = myOriginalTimer;
            }
            myTimer -= tempDeltaTime;

            #region BackgroundDir

            if (tempKeyState.IsKeyDown(Keys.Left))
            {
                BackDirLeft();
            }
            if (tempKeyState.IsKeyDown(Keys.Right))
            {
                BackDirRight();
            }
            if (tempKeyState.IsKeyDown(Keys.Down))
            {
                BackDirDown();
            }
            if (tempKeyState.IsKeyDown(Keys.Up))
            {
                BackDirUp();
            }

            for (int i = 0; i < myStarsFront.Count; i++)
            {
                myStarsFront[i] = new Point(myStarsFront[i].X + (myBackgroundDir.X * 10), myStarsFront[i].Y+ (myBackgroundDir.Y*10));
                myStarsMiddle[i] = new Point(myStarsMiddle[i].X + (myBackgroundDir.X * 6), myStarsMiddle[i].Y + (myBackgroundDir.Y*6));
                myStarsBack[i] = new Point(myStarsBack[i].X + (myBackgroundDir.X * 3), myStarsBack[i].Y + (myBackgroundDir.Y * 3));

                if (myBackgroundDir.Y > 0 && myBackgroundDir.X == 0)
                {
                    if (myStarsFront[i].Y > myGame.myGraphics.PreferredBackBufferHeight)
                    {
                        myStarsFront[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), 0);
                    }
                    if (myStarsMiddle[i].Y > myGame.myGraphics.PreferredBackBufferHeight)
                    {
                        myStarsMiddle[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), 0);
                    }
                    if (myStarsBack[i].Y > myGame.myGraphics.PreferredBackBufferHeight)
                    {
                        myStarsBack[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), 0);
                    }
                }
                if (myBackgroundDir.Y < 0 && myBackgroundDir.X == 0)
                {
                    if (myStarsFront[i].Y < 0)
                    {
                        myStarsFront[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myGame.myGraphics.PreferredBackBufferHeight);
                    }
                    if (myStarsMiddle[i].Y < 0)
                    {
                        myStarsMiddle[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myGame.myGraphics.PreferredBackBufferHeight);
                    }
                    if (myStarsBack[i].Y < 0)
                    {
                        myStarsBack[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myGame.myGraphics.PreferredBackBufferHeight);
                    }
                }
                if (myBackgroundDir.Y == 0 && myBackgroundDir.X > 0)
                {
                    if (myStarsFront[i].X > myGame.myGraphics.PreferredBackBufferWidth)
                    {
                        myStarsFront[i] = new Point(0, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                    }
                    if (myStarsMiddle[i].X > myGame.myGraphics.PreferredBackBufferWidth)
                    {
                        myStarsMiddle[i] = new Point(0, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                    }
                    if (myStarsBack[i].X > myGame.myGraphics.PreferredBackBufferWidth)
                    {
                        myStarsBack[i] = new Point(0, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                    }
                }
                if (myBackgroundDir.Y == 0 && myBackgroundDir.X < 0)
                {
                    if (myStarsFront[i].X < 0)
                    {
                        myStarsFront[i] = new Point(myGame.myGraphics.PreferredBackBufferWidth, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                    }
                    if (myStarsMiddle[i].X < 0)
                    {
                        myStarsMiddle[i] = new Point(myGame.myGraphics.PreferredBackBufferWidth, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                    }
                    if (myStarsBack[i].X < 0)
                    {
                        myStarsBack[i] = new Point(myGame.myGraphics.PreferredBackBufferWidth, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                    }
                }
            }

            #endregion

            for (int i = myBullets.Count-1; i >= 0; i--)
            {
                myBullets[i].Update(someGameTime);
                //if (myBullets[i].myPosition.X < 0)
                //{
                //    myBullets.RemoveAt(i);
                //}
                //if (myBullets[i].myPosition.X + myBullets[i].myTexture.Width > myGame.myGraphics.PreferredBackBufferWidth)
                //{
                //    myBullets.RemoveAt(i);
                //}
                //if (myBullets[i].myPosition.Y < 0)
                //{
                //    myBullets.RemoveAt(i);
                //}
                //if (myBullets[i].myPosition.Y + myBullets[i].myTexture.Height > myGame.myGraphics.PreferredBackBufferHeight)
                //{
                //    myBullets.RemoveAt(i);
                //}
                for (int j = 0; j < myEnemies.Count; j++)
                {
                    if (myBullets[i].myRectangle.Intersects(myEnemies[j].myRectangle))
                    {
                        myBullets.RemoveAt(i);
                        myEnemies.RemoveAt(j);
                        break;
                    }
                }
            }

            if ((tempMouse.LeftButton == ButtonState.Pressed) || tempKeyState.IsKeyDown(Keys.LeftShift))
            {
                if (myPlayer.myAttackTimer <= 0)
                {
                    Shoot(myBackgroundDir.ToVector2());
                    myPlayer.myAttackTimer = myPlayer.myAttackSpeed;
                }
            }
            if (tempMouse.RightButton == ButtonState.Pressed)
            {
                if (myPlayer.myAttackTimer <= 0)
                {
                    ShootAlt();
                    myPlayer.myAttackTimer = myPlayer.myAttackSpeed;
                }
            }
            myPlayer.myAttackTimer -= tempDeltaTime;
            return true;
        }

        public Vector2 GetDir(Vector2 someTo, Vector2 someFrom)
        {
            Vector2 dir = someTo - someFrom;
            dir.Normalize();

            return dir;
        }

        public void Shoot(Vector2 aDir)
        {
            MouseState mouse = Mouse.GetState();
            //if (player.myAmmo > 0)
            //{
            if (aDir.X == 0)
            {
                myBullets.Add(new Bullet(5f, aDir * -1, myBulletTexture, (myPlayer.myPosition + myPlayer.myOffset), 1, Color.LightBlue, 1));
            }
            else if (aDir.X != 0)
            {
                myBullets.Add(new Bullet(5f, aDir * -1, myBulletTexture, (myPlayer.myPosition + myPlayer.myOffset), 1, Color.LightBlue, 2));
            }
            //myBullets.Add(new Bullet(5f, new Vector2(0, 1), myBulletTexture, (myPlayer.myPosition + myPlayer.myOffset), 1, Color.LightBlue, 1));
            //myBullets.Add(new Bullet(5f, new Vector2(-1, 0), myBulletTexture, (myPlayer.myPosition + myPlayer.myOffset), 1, Color.LightBlue, 2));
            //myBullets.Add(new Bullet(5f, new Vector2(1, 0), myBulletTexture, (myPlayer.myPosition + myPlayer.myOffset), 1, Color.LightBlue, 2));

            //    player.myAmmo--;
            //}

        }
        public void ShootAlt()
        {
            Bullet tempBullet = new Bullet(10, new Vector2(-1, 0), myBulletTexture, (myPlayer.myPosition + myPlayer.myOffset), 1, Color.Gray, 3);
            myBullets.Add(tempBullet);
            myTimers.Add(new Timer(5));
        }

        protected Texture2D GenerateStarMap(int someWidth, int someHeight, int aNumStars, Color aColor)
        {
            var size = someWidth * someHeight;
            Color[] mapcolors = new Color[size];
            for (var i = 0; i<size; i++)
            {
                mapcolors[i] = Color.Black;
            }
            Random rand = new Random();
            for(var i=0;i<aNumStars;i++)
            {
                var n = rand.Next(size);
                int m = rand.Next(0,2);
                if (m == 0)
                {
                    mapcolors[n] = aColor;
                }
                else if (m == 1)
                {
                    mapcolors[n] = Color.White;
                }
            }
            var tex = new Texture2D(myGraphicsDevice, someWidth, someHeight, false, SurfaceFormat.Color);
            tex.SetData(mapcolors);
            return tex;
        }

        public void BackDirLeft()
        {
            myBackgroundDir = new Point(1, 0);
            myGame.myGraphics.PreferredBackBufferHeight = 800;
            myGame.myGraphics.PreferredBackBufferWidth = 1080;
            myGame.myGraphics.ApplyChanges();

        }
        public void BackDirRight()
        {
            myBackgroundDir = new Point(-1, 0);
            myGame.myGraphics.PreferredBackBufferHeight = 800;
            myGame.myGraphics.PreferredBackBufferWidth = 1080;
            myGame.myGraphics.ApplyChanges();
        }
        public void BackDirUp()
        {
            myBackgroundDir = new Point(0, 1);
            myGame.myGraphics.PreferredBackBufferHeight = 1080;
            myGame.myGraphics.PreferredBackBufferWidth = 1000;
            myGame.myGraphics.ApplyChanges();
            for (int i = 0; i < 600; i++)
            {
                myStarsFront[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                myStarsMiddle[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                myStarsBack[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
            }
        }
        public void BackDirDown()
        {
            myBackgroundDir = new Point(0, -1);
            myGame.myGraphics.PreferredBackBufferHeight = 1080;
            myGame.myGraphics.PreferredBackBufferWidth = 1000;
            myGame.myGraphics.ApplyChanges();
            for (int i = 0; i < 600; i++)
            {
                myStarsFront[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                myStarsMiddle[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                myStarsBack[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
            }
        }

        public override void Draw(GameTime someGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin(blendState : BlendState.NonPremultiplied);
            myGraphicsDevice.Clear(Color.Black);
            for (int i = 0; i < myStarsFront.Count; i++)
            {
                aSpriteBatch.Draw(myStarTexture, new Rectangle(myStarsFront[i], new Point(5, 5)), Color.White);
                aSpriteBatch.Draw(myStarTexture, new Rectangle(myStarsMiddle[i], new Point(5, 5)), Color.LightGray);
                aSpriteBatch.Draw(myStarTexture, new Rectangle(myStarsBack[i], new Point(5, 5)), Color.Gray);

            }
            for (int i = 0; i < myBullets.Count; i++)
            {
                myBullets[i].DrawBullet(aSpriteBatch);
            }
            for (int i = 0; i < myEnemies.Count; i++)
            {
                myEnemies[i].Draw(aSpriteBatch);
            }
            myPlayer.Draw(aSpriteBatch);
            aSpriteBatch.End();
        }
    }
}

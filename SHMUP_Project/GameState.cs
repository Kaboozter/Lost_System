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
using System.IO;


namespace SHMUP_Project
{
    class GameState : States
    {

        float myTimer, myOriginalTimer = 1,myBossTimer, myOriginalBossTimer = 2, myPressTimer, myBurstTimer, myBurstSpeed = 0.3f, myRammingEnemySpeed = 5, myShootingEnemySpeed = 5;

        int myBackWidth = 1080, myScore = 0, myScoreTracker = 0, myScoreTracker2 = 0, myPlayertype, myNumgreenPieces = 0, myNumBluePieces = 0;
        public Point myBackgroundDir = new Point(0,1);

        public static Point myDir;

        public List<Bullet> myBullets;
        List<Enemy> myEnemies;
        Dictionary<int,Timer> myTimers = new Dictionary<int, Timer>();
        List<Point> myStarsFront = new List<Point>();
        List<Point> myStarsMiddle = new List<Point>();
        List<Point> myStarsBack = new List<Point>();
        int[] mySaveData = new int[5];

        Texture2D[] myPlayerTexture;
        Player myPlayer;
        Enemy myEnemy;
        Texture2D myBulletTexture;
        Texture2D myBackSpace1,myBackSpace2;
        Random myRnd = new Random();
        SpriteFont myFont;
        Texture2D myPowerupsTexture;
        Texture2D myEnemyTexture, myBossTexture;
        Texture2D myStarTexture, myBubbleTexture;
        Song mySong;
        Color myColor = Color.White;

        public GameState(Game1 aGame, GraphicsDevice someGraphics, ContentManager someContent) : base(aGame, someGraphics, someContent)
        {
            mySaveData = SaveGameData.GetSaveData();
            myPlayertype = mySaveData[3];
            myNumBluePieces = mySaveData[2];
            myNumgreenPieces = mySaveData[1];
            myBackSpace1 = myGame.Content.Load<Texture2D>("stars");
            myBulletTexture = myGame.Content.Load<Texture2D>("BulletBlue");
            myEnemyTexture = myGame.Content.Load<Texture2D>("GreenDude");
            myStarTexture = myGame.Content.Load<Texture2D>("strass");
            myBossTexture = myGame.Content.Load<Texture2D>("BigBlueBoss");
            myBubbleTexture = myGame.Content.Load<Texture2D>("bubble");
            myFont = myGame.Content.Load<SpriteFont>("font");
            myPlayerTexture = new Texture2D[3]
            {
                myGame.Content.Load<Texture2D>("squareship"),
                myGame.Content.Load<Texture2D>("squareship_Green"),
                myGame.Content.Load<Texture2D>("squareship_Blue")
            };
            myPlayer = new Player(myPlayerTexture[myPlayertype], myGame, myPlayertype)
            {
            };
            myGame.myGraphics.PreferredBackBufferHeight = 1080;
            myGame.myGraphics.PreferredBackBufferWidth = 800;
            myGame.myGraphics.ApplyChanges();
            myEnemies = new List<Enemy>
            {
                //new CoolEnemy(myEnemyTexture, new Vector2(myGame.myGraphics.PreferredBackBufferWidth / 2, -100), new Vector2(0,1), 5, new Vector2(0.2f, 0.2f), 0, Color.White,1,this,myBulletTexture),
              //  new BlueBossEnemy(myBossTexture, new Vector2(myGame.myGraphics.PreferredBackBufferWidth / 2 , -100), new Vector2(0,1), 5, new Vector2(1f, 1f), 0, Color.White,1,this, myBulletTexture),
               // new ShootingEnemy(myEnemyTexture, new Vector2(myGame.myGraphics.PreferredBackBufferWidth / 2, -100), new Vector2(0,1), 5, new Vector2(0.2f, 0.2f), 0, Color.White,1,this, myBulletTexture)

            };


            myBullets = new List<Bullet>
            {
                new Bullet(2, new Vector2(1, -1), myBulletTexture, new Vector2(3000, 3000), 1, Color.Black, 1),
                new Bullet(2, new Vector2(1, -1), myBulletTexture, new Vector2(300, 300), 1, Color.Black, 1)

            };

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
                if (myEnemies[i] is BlueBossEnemy && myEnemies[i].myAttackTimer <= 0)
                {
                    Vector2 tempDir1 = ((myPlayer.myPosition + myPlayer.myOffset) - (myEnemies[i].myPosition + new Vector2(540, 130) - myEnemies[i].myOffset));
                    tempDir1.Normalize();
                    Vector2 tempDir2 = ((myPlayer.myPosition + myPlayer.myOffset) - (myEnemies[i].myPosition + new Vector2(80, 130) - myEnemies[i].myOffset));
                    tempDir2.Normalize();
                    NormalShoot(-tempDir1, myEnemies[i].myPosition + new Vector2(540, 130) - myEnemies[i].myOffset, 2, 5, Color.White);
                    NormalShoot(-tempDir2, myEnemies[i].myPosition + new Vector2(80, 130) - myEnemies[i].myOffset, 2, 5, Color.White);
                    myEnemies[i].myAttackTimer = myEnemies[i].myAttackSpeed;
                }
                if (myEnemies[i] is CoolEnemy && myEnemies[i].myRectangle.Intersects(myPlayer.myRectangle))
                {
                    myPlayer.myHp--;
                    myEnemies.RemoveAt(i);
                    break;
                }
                myEnemies[i].myAttackTimer -= tempDeltaTime;
            }
            if (myTimers.Count != 0)
            {
                for (int i = myTimers.Count - 1; i > 0; i--)
                {
                    myTimers[i].Update(someGameTime);
                }
            }

            if (myScore >= (myScoreTracker + 5))
            {
                BackDirUp();
                myEnemies.Add(new BlueBossEnemy(myBossTexture, new Vector2(myGame.myGraphics.PreferredBackBufferWidth / 2, 0), new Vector2(0, 1), 5, new Vector2(1f, 1f), 0, Color.White, 1, this, myBulletTexture));
                myScoreTracker = myScore;
            }
            if (myScore >= (myScoreTracker2 + 25))
            {
                myRammingEnemySpeed *= 1.2f;
                myShootingEnemySpeed *= 0.8f;
                myScoreTracker2 = myScore;
            }

            if (myTimer <= 0)
            {
                
                if (myRnd.Next(0,3) == 1)
                {
                    // Down.
                    if (myBackgroundDir == new Point(0, 1))
                    {
                        myEnemies.Add(new ShootingEnemy(myEnemyTexture, new Vector2(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth + 1), 0), new Vector2(0, 1), myShootingEnemySpeed, new Vector2(0.3f, 0.3f), 0, Color.Black, 1, this, myBulletTexture));
                    }
                    // Up.
                    if (myBackgroundDir == new Point(0, -1))
                    {
                        myEnemies.Add(new ShootingEnemy(myEnemyTexture, new Vector2(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth + 1), myGame.myGraphics.PreferredBackBufferHeight + 100), new Vector2(0, -1), myShootingEnemySpeed, new Vector2(0.3f, 0.3f), (float)Math.PI, Color.Black, 1, this, myBulletTexture));
                    }
                    // Right.
                    if (myBackgroundDir == new Point(1, 0))
                    {
                        myEnemies.Add(new ShootingEnemy(myEnemyTexture, new Vector2(-100, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight + 1)), new Vector2(1, 0), myShootingEnemySpeed, new Vector2(0.3f, 0.3f), ((float)Math.PI * 3) / 2, Color.Black, 1, this, myBulletTexture));
                    }
                    // Left.
                    if (myBackgroundDir == new Point(-1, 0))
                    {
                        myEnemies.Add(new ShootingEnemy(myEnemyTexture, new Vector2(myGame.myGraphics.PreferredBackBufferWidth + 100, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight + 1)), new Vector2(-1, 0), myShootingEnemySpeed, new Vector2(0.3f, 0.3f), ((float)Math.PI) / 2, Color.Black, 1, this, myBulletTexture));
                    }
                }
                else
                {
                    // Down.
                    if (myBackgroundDir == new Point(0, 1))
                    {
                        myEnemies.Add(new CoolEnemy(myEnemyTexture, new Vector2(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth + 1), 0), new Vector2(0, 1), myRammingEnemySpeed, new Vector2(0.3f, 0.3f), 0, Color.Black, 1, this, myBulletTexture));
                    }
                    // Up.
                    if (myBackgroundDir == new Point(0, -1))
                    {
                        myEnemies.Add(new CoolEnemy(myEnemyTexture, new Vector2(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth + 1), myGame.myGraphics.PreferredBackBufferHeight + 100), new Vector2(0, -1), myRammingEnemySpeed, new Vector2(0.3f, 0.3f), (float)Math.PI, Color.Black, 1, this, myBulletTexture));
                    }
                    // Right.
                    if (myBackgroundDir == new Point(1, 0))
                    {
                        myEnemies.Add(new CoolEnemy(myEnemyTexture, new Vector2(-100, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight + 1)), new Vector2(1, 0), myRammingEnemySpeed, new Vector2(0.3f, 0.3f), ((float)Math.PI * 3) / 2, Color.Black, 1, this, myBulletTexture));
                    }
                    // Left.
                    if (myBackgroundDir == new Point(-1, 0))
                    {
                        myEnemies.Add(new CoolEnemy(myEnemyTexture, new Vector2(myGame.myGraphics.PreferredBackBufferWidth + 100, myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight + 1)), new Vector2(-1, 0), myRammingEnemySpeed, new Vector2(0.3f, 0.3f), ((float)Math.PI) / 2, Color.Black, 1, this, myBulletTexture));
                    }
                }

                myTimer = myOriginalTimer;
            }
            myTimer -= tempDeltaTime;

            #region BackgroundDir
            if (myPlayertype == 1)
            {
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
            }


            for (int i = 0; i < myStarsFront.Count; i++)
            {
                myStarsFront[i] = new Point(myStarsFront[i].X + (myBackgroundDir.X * 10), myStarsFront[i].Y + (myBackgroundDir.Y * 10));
                myStarsMiddle[i] = new Point(myStarsMiddle[i].X + (myBackgroundDir.X * 6), myStarsMiddle[i].Y + (myBackgroundDir.Y * 6));
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

            myPressTimer += tempDeltaTime;
            if (tempKeyState.IsKeyDown(Keys.Escape) && myPressTimer > 1)
            {
                SaveGameData.mySplittedSaveData[1] = myNumgreenPieces;
                SaveGameData.mySplittedSaveData[2] = myNumBluePieces;
                SaveGameData.Save();
                myGame.ChangeState(new PauseMenu(myGame,myGraphicsDevice,myContentManager, myBackgroundDir));
                myPressTimer = 0;
            }

            // Hits enemy.
            for (int i = myBullets.Count - 1; i >= 0; i--)
            {
                myBullets[i].Update(someGameTime);

                for (int j = 0; j < myEnemies.Count; j++)
                {
                    if (myBullets[i].myRectangle.Intersects(myEnemies[j].myRectangle) && myBullets[i].myOwner == 1)
                    {
                        if (myEnemies[j] is BlueBossEnemy)
                        {
                            if (myEnemies[j].myHP <= 0)
                            {
                                myEnemies.RemoveAt(j);
                                myScore++;
                                if (myRnd.Next(0,2) == 1)
                                {
                                    if (myNumBluePieces >= 5)
                                    {
                                        if (myNumgreenPieces < 5)
                                        {
                                            myNumgreenPieces++;
                                        }
                                    }
                                    else
                                    {
                                        myNumBluePieces++;
                                    }
                                }
                                else
                                {
                                    if (myNumgreenPieces >= 5)
                                    {
                                        if (myNumBluePieces < 5)
                                        {
                                            myNumBluePieces++;
                                        }
                                    }
                                    else
                                    {
                                        myNumgreenPieces++;
                                    }
                                }
                                break;
                            }
                            if (myBullets[i].myType == 4)
                            {
                                if (myBullets[i].myPenetratingPower <= 0)
                                {
                                    myBullets.RemoveAt(i);
                                    myEnemies[j].myHP--;
                                    
                                    break;
                                }
                                else
                                {
                                    myBullets[i].myPenetratingPower--;
                                    myEnemies[j].myHP--;
                                    
                                    break;
                                }
                            }
                            else if (myBullets[i].myType != 4)
                            {
                                myBullets.RemoveAt(i);
                                myEnemies[j].myHP--;
                                
                                break;
                            }

                        }
                        else
                        {
                            if (myBullets[i].myType == 4)
                            {
                                if (myBullets[i].myPenetratingPower <= 0)
                                {
                                    myBullets.RemoveAt(i);
                                    myEnemies.RemoveAt(j);
                                    myScore++;
                                    break;
                                }
                                else
                                {
                                    myBullets[i].myPenetratingPower--;
                                    myEnemies.RemoveAt(j);
                                    myScore++;
                                    break;
                                }
                            }
                            else
                            {
                                myBullets.RemoveAt(i);
                                myEnemies.RemoveAt(j);
                                myScore++;
                                break;
                            }
                        }

                    }
                }



                if (i != myBullets.Count)
                {

                    if (myBullets[i].myPosition.X < 0)
                    {
                        myBullets.RemoveAt(i);
                        //break;
                    }
                    else if (myBullets[i].myPosition.X + myBullets[i].myTexture.Width > myGame.myGraphics.PreferredBackBufferWidth)
                    {
                        myBullets.RemoveAt(i);
                        //break;
                    }
                    else if (myBullets[i].myPosition.Y < 0)
                    {
                        myBullets.RemoveAt(i);
                        //break;
                    }
                    else if (myBullets[i].myPosition.Y + myBullets[i].myTexture.Height > myGame.myGraphics.PreferredBackBufferHeight)
                    {
                        myBullets.RemoveAt(i);
                        //break;
                    }
                }
                
            }
            // Hits player.
            for (int i = myBullets.Count - 1; i >= 0; i--)
            {
                myBullets[i].Update(someGameTime);

                if (myBullets[i].myRectangle.Intersects(myPlayer.myRectangle) && myBullets[i].myOwner == 2)
                {
                    myBullets.RemoveAt(i);
                    myPlayer.myHp--;
                    break;
                }
            }


            if ((tempMouse.LeftButton == ButtonState.Pressed) || tempKeyState.IsKeyDown(Keys.LeftShift))
            {
                if (myPlayer.myAttackTimer <= 0)
                {
                    if (myPlayertype == 0)
                    {
                        if (myBackgroundDir == new Point(1, 0))
                        {
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(-50, 0), 1, 10, Color.Red);
                        }
                        else
                        {
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition), 1, 10, Color.Red);
                        }
                    }
                    else if (myPlayertype == 1)
                    {
                        if (myBackgroundDir == new Point(1, 0))
                        {
                            SinShoot(2f, myBackgroundDir.ToVector2(), (myPlayer.myPosition + myPlayer.myOffset) + new Vector2(-50, 0), 1, Color.Red);
                        }
                        else
                        {
                            SinShoot(2f, myBackgroundDir.ToVector2(), (myPlayer.myPosition /*+ myPlayer.myOffset*/), 1, Color.Red);
                        }
                    }
                    else if (myPlayertype == 2)
                    {
                        if (myBackgroundDir == new Point(1, 0))
                        {
                            Vector2 tempDir = (tempMouse.Position.ToVector2() - (myPlayer.myPosition + myPlayer.myOffset));
                            tempDir.Normalize();
                            NormalShoot(-tempDir, (myPlayer.myPosition) + new Vector2(-50, 0), 1, 10, Color.Red);
                        }
                        else
                        {
                            Vector2 tempDir = (tempMouse.Position.ToVector2() - (myPlayer.myPosition + myPlayer.myOffset));
                            tempDir.Normalize();
                            NormalShoot(-tempDir, (myPlayer.myPosition), 1, 10, Color.Red);
                        }
                    }

                    myPlayer.myAttackTimer = myPlayer.myAttackSpeed;
                }
            }
            if (tempMouse.RightButton == ButtonState.Pressed)
            {
                if (myPlayer.myAttackTimer <= 0)
                {
                    if (myPlayertype == 0)
                    {
                        if (myBackgroundDir == new Point(1, 0))
                        {
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(-50, -50), 1, 10, Color.Red);
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(-50, 0), 1, 10, Color.Red);
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(-50, +50), 1, 10, Color.Red);
                        }
                        else if (myBackgroundDir == new Point(-1, 0))
                        {
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(0, -50), 1, 10, Color.Red);
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition ) + new Vector2(0, 0), 1, 10, Color.Red);
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(0, +50), 1, 10, Color.Red);

                        }
                        else if (myBackgroundDir == new Point(0, 1))
                        {
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(+50, 0), 1, 10, Color.Red);
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition ) + new Vector2(0, 0), 1, 10, Color.Red);
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(-50, 0), 1, 10, Color.Red);

                        }
                        else if (myBackgroundDir == new Point(0, -1))
                        {
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(+50, 0), 1, 10, Color.Red);
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(0, 0), 1, 10, Color.Red);
                            NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(-50, 0), 1, 10, Color.Red);

                        }
                    }
                    else if (myPlayertype == 1)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            if (i == 0)
                            {
                                NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(0, 0), 1, 10, Color.Red);
                            }
                            if (i == 2)
                            {
                                NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(0, 0), 1, 10, Color.Red);
                            }
                            if (i == 5)
                            {
                                NormalShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition) + new Vector2(0, 0), 1, 10, Color.Red);
                            }
                        }

                    }
                    else if (myPlayertype == 2)
                    {
                        PenetratingShoot(myBackgroundDir.ToVector2(), (myPlayer.myPosition), 1, 10, Color.Red);
                    }
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

        public void SinShoot(float aSpeed,Vector2 aDir, Vector2 aStartPos, int aOwner,Color aColor)
        {
            MouseState mouse = Mouse.GetState();

            if (aDir.X == 0)
            {
                myBullets.Add(new Bullet(aSpeed, aDir * -1, myBulletTexture, aStartPos, aOwner, aColor, 1));
            }
            else if (aDir.X != 0)
            {
                myBullets.Add(new Bullet(aSpeed, aDir * -1, myBulletTexture, aStartPos, aOwner, aColor, 2));
            }
        }
        public void NormalShoot(Vector2 aDir, Vector2 aStartPos, int aOwner, float aSpeed, Color aColor)
        {
            myBullets.Add(new Bullet(aSpeed, -aDir, myBulletTexture, aStartPos, aOwner, aColor, 3));
        }
        public void PenetratingShoot(Vector2 aDir, Vector2 aStartPos, int aOwner, float aSpeed, Color aColor)
        {
            myBullets.Add(new Bullet(aSpeed, -aDir, myBulletTexture, aStartPos, aOwner, aColor, 4));
        }


        #region BackDir
        public void BackDirLeft()
        {
            myBackgroundDir = new Point(1, 0);
            myGame.myGraphics.PreferredBackBufferHeight = 800;
            myGame.myGraphics.PreferredBackBufferWidth = 1080;
            myGame.myGraphics.ApplyChanges();
            for (int i = 0; i < 600; i++)
            {
                myStarsFront[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                myStarsMiddle[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                myStarsBack[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
            }

        }
        public void BackDirRight()
        {
            myBackgroundDir = new Point(-1, 0);
            myGame.myGraphics.PreferredBackBufferHeight = 800;
            myGame.myGraphics.PreferredBackBufferWidth = 1080;
            myGame.myGraphics.ApplyChanges();
            for (int i = 0; i < 600; i++)
            {
                myStarsFront[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                myStarsMiddle[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
                myStarsBack[i] = new Point(myRnd.Next(0, myGame.myGraphics.PreferredBackBufferWidth), myRnd.Next(0, myGame.myGraphics.PreferredBackBufferHeight));
            }
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
        #endregion

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
            aSpriteBatch.DrawString(myFont,"Score:" + myScore,new Vector2(100, 50), Color.White);
            aSpriteBatch.DrawString(myFont, "HP:" + myPlayer.myHp, new Vector2(myGame.myGraphics.PreferredBackBufferWidth - 100, 50), Color.White);
            aSpriteBatch.DrawString(myFont, "Green:"+myNumgreenPieces+"           "+"Blue:"+myNumBluePieces, new Vector2(myGame.myGraphics.PreferredBackBufferWidth * 0.5f - (myFont.MeasureString("Green:0           Blue:0").X*0.5f),50),Color.White);


            myPlayer.Draw(aSpriteBatch);
            aSpriteBatch.End();
        }
    }
}

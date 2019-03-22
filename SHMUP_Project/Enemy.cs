using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SHMUP_Project
{
    abstract class Enemy
    {
        public Texture2D myTexture;
        public Rectangle myRectangle;
        public Vector2 myMoveDir;
        public Vector2 myPosition;
        public Vector2 myScale;
        public Vector2 myOffset;
        public bool myEnemySmart;
        public Color mycolor;
        public float mySpeed;
        public float myRotation;
        int myIndex = 0;
        List<Enemy> myEnemyList = new List<Enemy>();

        public void AddEnemy(Enemy aEnemy)
        {
            myEnemyList.Add(aEnemy);
        }

        public Enemy(Texture2D aEnemyTexture, Vector2 aEnemyStartPos,Vector2 aDir, float someEnemySpeed, Vector2 aEnemyScale, float aEnemyRotation, Color aEnemyColor, int aType, States aState)
        {
            myTexture = aEnemyTexture;
            myPosition = aEnemyStartPos;
            mySpeed = someEnemySpeed;
            myMoveDir = aDir;
            myScale = aEnemyScale;
            myOffset = (aEnemyTexture.Bounds.Size.ToVector2() / 2) * myScale;
            myRectangle = new Rectangle((aEnemyStartPos - myOffset).ToPoint(), (aEnemyTexture.Bounds.Size.ToVector2() * aEnemyScale).ToPoint());
            mycolor = aEnemyColor;
            myRotation = aEnemyRotation;
            myIndex = myEnemyList.Count + 1;
        }

        public abstract void Update(GameTime someGameTime);


        public abstract void Draw(SpriteBatch aSpriteBatch);

    }
}

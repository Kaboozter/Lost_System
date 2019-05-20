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
    class CoolEnemy : Enemy
    {
        List<Enemy> myEnemyList = new List<Enemy>();

        public void AddEnemy(Enemy aEnemy)
        {
            myEnemyList.Add(aEnemy);
        }

        public CoolEnemy(Texture2D aEnemyTexture, Vector2 aEnemyStartPos, Vector2 aDir, float someEnemySpeed, Vector2 aEnemyScale, float aEnemyRotation, Color aEnemyColor, int aType, GameState aState, Texture2D aBulletTexture) : base(aEnemyTexture, aEnemyStartPos, aDir, someEnemySpeed, aEnemyScale, aEnemyRotation, aEnemyColor, aType, aState, aBulletTexture)
        {

        }

        public override void Update(GameTime someGameTime)
        {
            myPosition += (myMoveDir * mySpeed);
            myRectangle.Location = (myPosition).ToPoint();
            // Down.
            if (myRotation == 0)
            {
                myRectangle.Width = (int)(myTexture.Width * myScale.Y);
                myRectangle.Height = (int)(myTexture.Height * myScale.Y);
            }
            // Up.
            if (myRotation == (float)Math.PI)
            {
                myRectangle.Width = (int)(myTexture.Width * myScale.Y);
                myRectangle.Height = (int)(myTexture.Height * myScale.Y);
                myRectangle.Location = (myRectangle.Location - myRectangle.Size);
            }
            // Right.
            if (myRotation == ((float)Math.PI * 3) / 2)
            {
                myRectangle.Height = (int)(myTexture.Width * myScale.Y);
                myRectangle.Width = (int)(myTexture.Height * myScale.Y);
                myRectangle.Location = new Point(myRectangle.Location.X, myRectangle.Location.Y - myRectangle.Height);
            }
            // Left.
            if (myRotation == ((float)Math.PI) / 2)
            {
                myRectangle.Width = (int)(myTexture.Height * myScale.Y);
                myRectangle.Height = (int)(myTexture.Width * myScale.Y);
            }

        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myRectangle, Color.Cyan);
            aSpriteBatch.Draw(myTexture, myPosition, null, Color.Blue, myRotation, myOffset, myScale, SpriteEffects.None, 0);
        }

        public override void Attack(Vector2 someDir)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP_Project
{
    class BlueBossEnemy : Enemy
    {

        public BlueBossEnemy(Texture2D aEnemyTexture, Vector2 aEnemyStartPos, Vector2 aDir, float someEnemySpeed, Vector2 aEnemyScale, float aEnemyRotation, Color aEnemyColor, int aType, GameState aState, Texture2D aBulletTexture) : base(aEnemyTexture, aEnemyStartPos, aDir, someEnemySpeed, aEnemyScale, aEnemyRotation, aEnemyColor, aType, aState, aBulletTexture)
        {

        }

        public override void Update(GameTime someGameTime)
        {
            myAttackSpeed = 1f;
            if (myPosition.Y < 150)
            {
                myPosition += (mySpeed * myMoveDir);
            }
            myRectangle = new Rectangle(new Point((int)(myPosition.X - (myOffset.X - (myOffset.X * 0.25))) , (int)(myPosition.Y) ),new Point(400,65));

            
        }
        public override void Draw(SpriteBatch aSpriteBatch)
        {

            aSpriteBatch.Draw(myTexture, myPosition, null, Color.White, myRotation, myOffset, myScale, SpriteEffects.None, 0);
            //aSpriteBatch.Draw(myTexture, myRectangle, Color.Cyan);

        }

        public override void Attack(Vector2 someDir)
        {
            //myCurGame.myBullets.Add(new Bullet(5,new Vector2(0,1),));
        }
    }
}

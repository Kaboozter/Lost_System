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

        public CoolEnemy(Texture2D aEnemyTexture, Vector2 aEnemyStartPos, Vector2 aDir, float someEnemySpeed, Vector2 aEnemyScale, float aEnemyRotation, Color aEnemyColor, int aType, GameState aState) : base(aEnemyTexture, aEnemyStartPos, aDir, someEnemySpeed, aEnemyScale, aEnemyRotation, aEnemyColor, aType, aState)
        {

        }

        public override void Update(GameTime someGameTime)
        {
            myPosition += (myMoveDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, null, Color.White, myRotation, myOffset, myScale, SpriteEffects.None, 0);
        }
        public override void Attack(Vector2 someDir)
        {

        }
    }
}

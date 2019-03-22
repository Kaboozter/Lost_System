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

        public BlueBossEnemy(Texture2D aEnemyTexture, Vector2 aEnemyStartPos, Vector2 aDir, float someEnemySpeed, Vector2 aEnemyScale, float aEnemyRotation, Color aEnemyColor, int aType, States aState) : base(aEnemyTexture, aEnemyStartPos, aDir, someEnemySpeed, aEnemyScale, aEnemyRotation, aEnemyColor, aType, aState)
        {

        }

        public override void Update(GameTime someGameTime)
        {
            if (myPosition.Y < 100)
            {
                myPosition += (mySpeed * myMoveDir);
            }
        }
        public override void Draw(SpriteBatch aSpriteBatch)
        {

        }
    }
}

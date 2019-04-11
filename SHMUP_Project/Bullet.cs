using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP_Project
{
    class Bullet
    {
        #region Variables
        public float mySpeed, myRotation = 0;
        public Vector2 myDir, myPosition, myOffset, myScale = new Vector2(0.07f, 0.07f);
        public Texture2D myTexture;
        public Rectangle myRectangle;
        public float myDamage = 1;
        public Color myColor;
        public float myOwner;
        public int myType;
        #endregion


        public Bullet(float someSpeed, Vector2 aDir, Texture2D aTexture, Vector2 aStartPos, float aOwner, Color somePaint, int aType)
        {
            myOwner = aOwner;
            mySpeed = someSpeed;
            myDir = aDir;
            myTexture = aTexture;
            myPosition = aStartPos;
            myType = aType;
            myOffset = ((myTexture.Bounds.Size.ToVector2()) / 2);
            myRectangle = new Rectangle((myOffset - myPosition).ToPoint(), new Point(20, 20));
            myColor = somePaint;
            myRotation = (float)Math.Atan2(myDir.X, myDir.Y) * -1;
        }

        public void Update(GameTime someGameTime)
        {
            float tempDeltaTime = (float)someGameTime.ElapsedGameTime.Milliseconds;
            if (myType == 1)
            {
                myPosition.Y += (myDir.Y * mySpeed);
                myPosition.X += ((float)Math.Sin(myPosition.Y * 0.05f) * 1.5f) * tempDeltaTime;
                myRectangle.Location = (myPosition - (myRectangle.Size.ToVector2() * 0.5f)).ToPoint();
                myRotation = (float)Math.Atan2(myDir.X, myDir.Y) * -1;
            }
            else if (myType == 2)
            {
                myPosition.X += (myDir.X * mySpeed);
                myPosition.Y += ((float)Math.Sin(myPosition.X * 0.05f) * 1.5f) * tempDeltaTime;
                myRectangle.Location = (myPosition - (myRectangle.Size.ToVector2() * 0.5f)).ToPoint();
                myRotation = (float)Math.Atan2(myDir.X, myDir.Y) * -1;
            }
            else if (myType == 3)
            {
                myPosition += (myDir * mySpeed);
                myRectangle.Location = (myPosition - (myRectangle.Size.ToVector2() * 0.5f)).ToPoint();
            }


        }

        

        public void DrawBullet(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(myTexture, myPosition, null, myColor, myRotation, myOffset, 1f, SpriteEffects.None, 1);
        }
    }
}

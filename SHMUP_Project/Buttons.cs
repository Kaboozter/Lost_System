using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP_Project
{
    class Buttons : Components
    {
        Texture2D myTexture;
        SpriteFont myFont;
        bool myIsHovering;
        MouseState myCurMouse;
        MouseState myPrevMouse;

        public event EventHandler Click;
        public bool AccessClicked { get; private set; }

        public Vector2 AccessPos { get; set; }
        public Color AccessPaint { get; set; }
        public Rectangle AccessRect
        {
            get
            {
                return new Rectangle((int)AccessPos.X, (int)AccessPos.Y, myTexture.Width, myTexture.Height);
            }
        }
        public string AccessText { get; set; }

        public Buttons(Texture2D aTexture, SpriteFont aFont)
        {
            myFont = aFont;
            myTexture = aTexture;
            AccessPaint = Color.Black;
        }

        public override void Draw(SpriteBatch aSpriteBatch, GameTime someGameTime)
        {
            Color tempColor = Color.White;

            if(myIsHovering)
            {
                tempColor = Color.Gray;
            }

            aSpriteBatch.Draw(myTexture, AccessRect, tempColor);

            if (!string.IsNullOrEmpty(AccessText))
            {
                float tempX = (AccessRect.X + (AccessRect.Width * 0.5f) - (myFont.MeasureString(AccessText).X * 0.5f));
                float tempY = (AccessRect.Y + (AccessRect.Height * 0.5f) - (myFont.MeasureString(AccessText).Y * 0.5f));

                aSpriteBatch.DrawString(myFont, AccessText, new Vector2(tempX, tempY), AccessPaint);
            }

        }
        public override void Update(GameTime someGameTime)
        {
            myIsHovering = false;

            myPrevMouse = myCurMouse;
            myCurMouse = Mouse.GetState();

            Rectangle tempMouseRect = new Rectangle(myCurMouse.X,myCurMouse.Y, 1, 1);

            if (tempMouseRect.Intersects(AccessRect))
            {
                myIsHovering = true;
                if (myCurMouse.LeftButton == ButtonState.Released && myPrevMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this,new EventArgs());
                }
            }
        }
    }
}

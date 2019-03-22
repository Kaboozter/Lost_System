using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SHMUP_Project
{
    public abstract class States
    {
        protected ContentManager myContentManager;
        protected GraphicsDevice myGraphicsDevice;
        protected Game1 myGame;
        

        public abstract void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch);

        public abstract bool Update(GameTime aGameTime);

        public States(Game1 aGame, GraphicsDevice someGraphics, ContentManager someContent)
        {
            this.myGame = aGame;
            this.myGraphicsDevice = someGraphics;
            this.myContentManager = someContent;
        }
    }
}

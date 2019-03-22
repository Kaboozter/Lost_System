using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP_Project
{
    public abstract class Components
    {
        public abstract void Draw(SpriteBatch aSpriteBatch, GameTime aGameTime);

        public abstract void Update(GameTime aGameTime); 

    }
}

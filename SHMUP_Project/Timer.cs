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

namespace SHMUP_Project
{
    class Timer
    {
        float myTimerAmount;

        public Timer(float someTimerAmount)
        {
            myTimerAmount = someTimerAmount;
        }
        public bool Update(GameTime someGametime)
        {
            float tempDeltaTime = (float)someGametime.ElapsedGameTime.Milliseconds;
            if (myTimerAmount <= 0)
            {
                return true;
            }
            myTimerAmount -= tempDeltaTime;
            return false;
        }
    }
}

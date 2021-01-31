using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceBaseMono.Timers
{
   
    public class Timer
    {
        public Action Trigger;
        public float Interval;
        float Elapsed;

        Timer() { }

        public void Update(GameTime gameTime)
        {
            Elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Elapsed >= Interval)
            {
                Trigger.Invoke();
                Destroy();
            }
        }

        public void Destroy()
        {
            TimerManager.Remove(this);
        }

        public static void Create(float interval, Action trigger)
        {
            Timer timer = new Timer();
            timer.Interval=interval;
            timer.Trigger = trigger;
            TimerManager.Add(timer);
        }
    }

}


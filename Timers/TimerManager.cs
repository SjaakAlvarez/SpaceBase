using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceBaseMono.Timers
{
    public class TimerManager : GameComponent
    {
                
        List<Timer> ToRemove = new List<Timer>();
        List<Timer> Timers = new List<Timer>();

        public static TimerManager Instance=new TimerManager(null);

        public TimerManager(Game game)
            : base(game) 
        {
        }

        public static void Add(Timer Timer) { Instance.Timers.Add(Timer); }
        public static void Remove(Timer Timer) { Instance.ToRemove.Add(Timer); }

        public override void Update(GameTime gametime)
        {
            foreach (Timer timer in ToRemove) Timers.Remove(timer);
            ToRemove.Clear();
            foreach (Timer timer in Timers) timer.Update(gametime);

        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;

namespace SpaceBaseMono
{
    public static class SoundCache
    {
        public static Dictionary<String, SoundEffect> effects = new Dictionary<string, SoundEffect>();

        public static void LoadSound(ContentManager content, String name)
        {
            if (!effects.ContainsKey(name))
            {
                SoundEffect s = content.Load<SoundEffect>("SoundEffects/"+name);
                effects.Add(name, s);
            }
        }

        public static void PlaySound(String name, float volume=1.0f )
        {
            effects[name].Play(volume,0,0);
        }


    }
}

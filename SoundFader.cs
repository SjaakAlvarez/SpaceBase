using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceBaseMono
{
    class SoundFader : GameComponent
    {
        enum State { FADING_IN, PLAY, FADING_OUT, STOP };
        State state;

        private SoundEffectInstance sound;
        private Game game;
        private float volume;
        private float maxvol;

        public SoundFader(Game game) :base (game) 
        {
            this.game = game;
        }

        public SoundFader(Game game, SoundEffectInstance sound, float maxvol=1.0f)
            : base(game)  
        {
            this.game = game;
            this.sound = sound;
            this.maxvol = maxvol;
        }

        public void FadeOut()
        {
            state = State.FADING_OUT;
        }

        public void Play()
        {
            state = State.PLAY;
            sound.Play();            
            volume = maxvol;
        }

        public void Stop()
        {
            state = State.STOP;
            sound.Stop();
            volume = maxvol;
        }

        public void FadeIn()
        {            
            volume = 0.0f;
            state = State.FADING_IN;
            sound.Play();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (state)
            {
                case State.FADING_IN:
                    volume += 0.005f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (volume >= maxvol)
                    {
                        state = State.PLAY;
                        volume = maxvol;
                    }
                    break;
                case State.FADING_OUT:
                    volume -= 0.0005f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (volume <= 0)
                    {
                        Stop();
                    }
                    break;
                 
            }

            sound.Volume = Easing.EaseOut(volume, EasingType.Quadratic);
         }
    }
}

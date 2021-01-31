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
    public class Light : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected ContentManager content;
        protected SpriteBatch spriteBatch;
        private Texture2D light;
        private Vector2 pos;
        private Game game;
        private float speed;
        private Color color;

        private float opacity=0;

        enum State { SWITCHING_ON, ON, SWITCHING_OFF, OFF, BLINKINGOFF, BLINKINGON };
        State state;

        public void On()
        {
            state = State.SWITCHING_ON;   
        }

        public void Off()
        {
            state = State.SWITCHING_OFF;
        }

        public void Blink(float speed)
        {
            state = State.BLINKINGON;
            this.speed = speed;
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public Light(Game game) :base (game) 
        {
            this.game = game;
        }

        public Light(Game game,Vector2 pos, Color color) : base(game)  
        {
            this.game = game;
            this.pos = pos;
            state = State.OFF;
            opacity = 0;
            this.color = color;
        }     

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            if (content == null)
                content = new ContentManager(game.Services, "Content");

            light = content.Load<Texture2D>("light");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (state)
            {
                case State.SWITCHING_ON:
                    opacity += 0.01f*(float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (opacity >= 1)
                    {
                        state = State.ON;
                        opacity = 1.0f;
                    }
                    break;
                case State.SWITCHING_OFF:
                    opacity -= 0.005f*(float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (opacity <=0)
                    {
                        state = State.OFF;
                        opacity = 0.0f;
                    }
                    break;
                case State.BLINKINGOFF:
                    opacity -= speed*(float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (opacity <= 0)
                    {
                        state = State.BLINKINGON;                        
                    }
                    break;
                case State.BLINKINGON:
                    opacity += speed*(float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (opacity >=1)
                    {
                        state = State.BLINKINGOFF;
                        
                    }
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, null, null);
            float o = Easing.EaseOut(opacity, EasingType.Quadratic);
            spriteBatch.Draw(light, new Vector2(pos.X, pos.Y), new Rectangle(0, 0, 200, 200), color*o, 0.0f, new Vector2(100, 100), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }


    }
}

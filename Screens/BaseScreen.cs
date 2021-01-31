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
using GameStateManagement;

namespace SpaceBaseMono.Screens
{
    public class BaseScreen : GameScreen
    {
        protected double lastBlink;
        protected Color blinkColor = Color.White;
        protected Texture2D blank;

        
        public ContentManager content;
        protected SpriteBatch spriteBatch;
        public SpriteFont arcade;
        
        protected KeyboardState current, last;
        protected GamePadState gamepadstate, oldgamepadstate;
       
        public Random rand = new Random();

      



        protected Game1 GetGame()
        {
            return (Game1)ScreenManager.Game;
        }



        //Game stuff
        public int Credits { get; set; }
        //protected Boolean vertical = false;

        public BaseScreen()
        {
            Credits = 0;
        }




        public override void Activate(bool instancePreserved)
        {
            
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
           

            arcade = content.Load<SpriteFont>("Fonts/Arcade");
            blank = content.Load<Texture2D>("blank");
            



            spriteBatch = ScreenManager.SpriteBatch;
            
            current = Keyboard.GetState();
            last = current = Keyboard.GetState();

           

        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            
           

            last = current;
            current = Keyboard.GetState();

           

            base.HandleInput(gameTime, input);
        }

        public void ClearKeyBuffer()
        {
            last = current;
        }

        public Boolean KeyPressed(Keys key, Boolean repeat = false)
        {
            if (current.IsKeyDown(key) && (last.IsKeyUp(key) || repeat)) return true;



            return false;
        }

       

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > lastBlink + 360)
            {
                lastBlink = gameTime.TotalGameTime.TotalMilliseconds;
                blinkColor = blinkColor == Color.Red ? Color.White : Color.Red;
            }


            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

       
        


    }
}

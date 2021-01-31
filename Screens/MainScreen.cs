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
using System.Diagnostics;


namespace SpaceBaseMono.Screens
{
    class MainScreen : BaseScreen
    {
       

        SpriteFont font;
        
        public Boolean repeat;
        double repeatTime;
        
        float size;
        double updatestats = 1000;
        double framesPerSecond;
        
        String frames="";
        String memory="";


        public override void Activate(bool instancePreserved)
        {
            base.Activate(instancePreserved);
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            spriteBatch = ScreenManager.SpriteBatch;
            
            font = content.Load<SpriteFont>("Fonts/Arcade");
            
        }



        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            size++;

            updatestats -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (updatestats < 0)
            {
                updatestats += 1000;
                framesPerSecond = (1.0d / gameTime.ElapsedGameTime.TotalMilliseconds) * 1000.0d;
                framesPerSecond = Math.Ceiling(framesPerSecond);
                frames = framesPerSecond.ToString() + " FPS";
                
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0:D}", System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024));
                memory = sb.ToString();

                // spriteBatch.DrawString(arcade, "External program running: "+GetGame().gameRunning, new Vector2(0, 12 * 16), Color.White);
            }
            
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            repeat = false;
            if (gameTime.TotalGameTime.TotalMilliseconds > repeatTime + 20)
            {
                repeatTime = gameTime.TotalGameTime.TotalMilliseconds;
                repeat = true;
            }
      
        }

        
        
        

        public override void Draw(GameTime gameTime)
        {            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.Identity);
           
            ScreenManager.GraphicsDevice.Clear(Color.Black);
            
           
            spriteBatch.DrawString(arcade, frames, new Vector2(1*16, 37 * 16), Color.White);
            spriteBatch.DrawString(arcade, memory, new Vector2(1*16, 38 * 16), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    
    }
}

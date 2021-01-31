using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceBaseMono.Screens
{
    public class TitleScreen : BaseScreen
    {
        SpriteFont font;
        Texture2D ball, background;
        Effect reflect;
        int nr=20;
        int anim = 0;
        double animTime;

        List<float> x = new List<float>(20);
        List<float> y = new List<float>(20);
        List<float> dx = new List<float>(20);
        List<float> dy = new List<float>(20);

        Color[] newcolor = new Color[49 * 64];
        
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
            
            //ball = content.Load<Texture2D>("ball0000");
            
            background = content.Load<Texture2D>("title");
            

            //reflect = content.Load<Effect>("Effects/Mirror");

            //Song song = content.Load<Song>("Music/Attract Mode2");  // Put the name of your song in instead of "song_title"
            //MediaPlayer.Play(song);

            /*for (int i = 0; i < nr; i++)
            {
                x.Add(rand.Next(10,810));
                y.Add(rand.Next(10, 1014));
                dx.Add(rand.Next(-50, 50));
                dy.Add(rand.Next(-50, 50));

            }*/


           /* Color[] colors = new Color[784 * 1024];
            background.GetData(colors);

            

            for (int x = 0; x < 49; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    int r = 0;
                    int g = 0;
                    int b = 0;
                    for (int x1 = 0; x1 < 16; x1++)
                    {
                        for (int y1 = 0; y1 < 16; y1++)
                        {
                            
                            r = r + colors[(((y * 16) + y1) * 784) + (x * 16) + x1].R;
                            g = g + colors[(((y * 16) + y1) * 784) + (x * 16) + x1].G;
                            b = b + colors[(((y * 16) + y1) * 784) + (x * 16) + x1].B;

                        }
                    }
                    newcolor[x + (49 * y)] = new Color(r / 256,g / 256, b / 256);
                }
            }
             */


        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                ScreenManager.RemoveScreen(this);
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
           /* for (int i = 0; i < nr; i++)
            {
                x[i] += dx[i] * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                y[i] += dy[i] * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                if (x[i] > 820 || x[i] < 0)
                {
                    dx[i] = -dx[i];
                    x[i] = x[i] < 0 ? 0 : 820;
                }
                if (y[i] > 1024 || y[i] < 0)
                {
                    dy[i] = -dy[i];
                    y[i] = y[i] < 0 ? 0 : 1024;
                }
            }*/
            if (gameTime.TotalGameTime.TotalMilliseconds  > animTime +10)
            {
                if (anim < 1024) anim++;
                animTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.Identity);

            ScreenManager.GraphicsDevice.Clear(Color.Black);
  

            spriteBatch.Draw(background, new Vector2(430,512), new Rectangle(0, 0, 820, 1024), Color.White*1.0f, 0, new Vector2(410, 512), 1.0f, SpriteEffects.None, 0.0f);

            
            /*String chars = "SPACEBASE";

            for (int x = 0; x < 49; x++)
            {
                for (int y = anim/16; y < 64; y++)
                {
                    spriteBatch.DrawString(arcade, chars.Substring(x%9,1), new Vector2((x+1) * 16, y * 16), newcolor[x + (49 * y)]*1.0f);
                }
            }*/

            
            
            spriteBatch.DrawStringOnBlack(arcade, "Le Chuck", new Vector2(1 * 16, 30 * 16), Color.Red,blank);
            spriteBatch.DrawStringOnBlack(arcade, "Fabrication, basic mechanics, art", new Vector2(1 * 16, 31 * 16), Color.White, blank);
            spriteBatch.DrawStringOnBlack(arcade, "IDS", new Vector2(1 * 16, 33 * 16), Color.Red, blank);
            spriteBatch.DrawStringOnBlack(arcade, "Electronics, software, hardware", new Vector2(1 * 16, 34 * 16), Color.White, blank);
            spriteBatch.DrawStringOnBlack(arcade, "eds1275", new Vector2(1 * 16, 36 * 16), Color.Red, blank);
            spriteBatch.DrawStringOnBlack(arcade, "Music, sound effects, voice acting", new Vector2(1 * 16, 37 * 16), Color.White, blank);
            spriteBatch.DrawStringOnBlack(arcade, "Big thanks to his actress", new Vector2(1 * 16, 38 * 16), Color.White, blank);

            spriteBatch.DrawStringOnBlack(arcade, "Sjaak", new Vector2(1 * 16, 40 * 16), Color.Red, blank);
            spriteBatch.DrawStringOnBlack(arcade, "Coding this game", new Vector2(1 * 16, 41 * 16), Color.White, blank);

            spriteBatch.DrawStringOnBlack(arcade, "Big thanks to Jake Nealis for the Space Gal art", new Vector2(1 * 16, 43 * 16), Color.White, blank);

            spriteBatch.DrawStringOnBlack(arcade, "Press SPACE to continue", new Vector2(14 * 16, 50 * 16), blinkColor, blank);
            

            spriteBatch.End();

            /*for (int i = 0; i < nr; i++)
            {
                spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, reflect);
                ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
                ScreenManager.GraphicsDevice.Textures[1] = background;

                reflect.Parameters["x"].SetValue(x[i]);
                reflect.Parameters["y"].SetValue(y[i]);
                reflect.Parameters["scale"].SetValue(0.5f);
                spriteBatch.Draw(ball, new Vector2(x[i], y[i]), new Rectangle(0, 0, 200, 200), Color.White, 0.0f, new Vector2(100, 100), 0.5f, SpriteEffects.None, 0.0f);

                spriteBatch.End();
            }
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, Matrix.Identity);
            
            

            spriteBatch.End();*/

            base.Draw(gameTime);
        }


    }
    
}

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
using System.IO;
using SpaceBase.Debug;
using SpaceBaseMono.Menu;
using ExtensionMethods;

namespace SpaceBaseMono.Screens
{
    public class LevelScreen : BaseScreen
    {
        //game state
        enum LevelState { ATTRACT,INIT, PLAYING, RESETTING, ENDING};
        LevelState state;

        //resources
        SpriteFont digital;
        Texture2D part,ball,light,background,overlay;
        SoundEffectInstance loopmusic,loopalarm;
        SoundEffect music,alarm;
        Song attractsong;
        Effect reflect;
        //VisualizationData data = new VisualizationData();
        
        //keyboard repeat
        double repeatTime;
        Boolean repeat;

        //moving metal part
        float y1=900, y2=900;                
        float maxx;
        float angle;

        Boolean inhole = false;        
        
        //lights
        double lastRandomLight;
        int randomLight;
        List<Light> lights = new List<Light>();

        //attract mode scrolling message
        String msg;
        int msgpos = 0;
        double lastMsgUpdate;
                
        //game logic
        int activeHole=1;
        int lives = 0;
        int score = 0;
        List<Hole> holes = new List<Hole>();

        //ball
        float fallx=0;
        float fally=0;
        float anim = 0;       
        float x = 410;
        float dx = 0;

        //countdown playtime
        int timer;
        double lastTimerUpdate;

        //music
        SoundFader mainLoop;
       
        //end sequence
        double endSequence = 2500;
        

        
#if DEBUG
        MouseState oldState;
        public LineBatchComponent lines;
        StreamWriter sw;
#endif

        public void StartGame()
        {
            state = LevelState.PLAYING;                       
        }

        public void Reset()
        {
            //reset arm to bottom position
            state = LevelState.RESETTING;                        
        }

        public void Init()
        {
            //move arm into view
            timer = 90;
            x = 410;
            inhole = false;
            y1 = 900;
            y2 = 900;
            state = LevelState.INIT;
            dx = 0;
            foreach (Light l in lights) l.Off();
            lights[activeHole - 1].On();
            SoundCache.PlaySound("Pilot Are You Ready");
            mainLoop.Play();
        }

        public void PlayerDie()
        {
            lives--;     
        }

        public void PlayerNextHole()
        {
            score += activeHole * 10;
            lights[activeHole - 1].Off();
            activeHole++;
            if (activeHole == 11) activeHole = 1;
        }

        private void StartAttractMode()
        {
            MediaPlayer.Play(attractsong);            
            state = LevelState.ATTRACT;
        }

        private void EndGame()
        {
            state = LevelState.ENDING;
            mainLoop.FadeOut();
            SoundCache.PlaySound("Goodbye Pilot");

        }
       
        public override void Activate(bool instancePreserved)
        {
            base.Activate(instancePreserved);
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            spriteBatch = ScreenManager.SpriteBatch;
            
            digital = content.Load<SpriteFont>("digital");
            digital.Spacing = -12;
            part = content.Load<Texture2D>("part2");
            ball = content.Load<Texture2D>("ball");
            light = content.Load<Texture2D>("light");
            background = content.Load<Texture2D>("background");
            overlay = content.Load<Texture2D>("overlay");
            music = content.Load<SoundEffect>("Music/Main");
            alarm = content.Load<SoundEffect>("Music/Alarm30seccut");     

            loopmusic = music.CreateInstance();
            loopmusic.IsLooped = true;
            loopmusic.Volume = 0.9f;
            mainLoop = new SoundFader(ScreenManager.Game, loopmusic);

            loopalarm = alarm.CreateInstance();
            loopalarm.Volume = 0.75f;

            attractsong = content.Load<Song>("Music/Attract Mode");
            //MediaPlayer.IsVisualizationEnabled = true;
            MediaPlayer.Play(attractsong);
            MediaPlayer.IsRepeating = true;            


            SoundCache.LoadSound(content, "Oxygen Depleted");
            SoundCache.LoadSound(content, "Oxygen Reserves Are Low");
            SoundCache.LoadSound(content, "Pilot Are You Ready");
            SoundCache.LoadSound(content, "Come Back To Me");
            SoundCache.LoadSound(content, "Entering Wormhole");
            SoundCache.LoadSound(content, "Goodbye Pilot");
            SoundCache.LoadSound(content, "Welcome To Space Base");
            SoundCache.LoadSound(content, "Hurry Up Pilot");


            reflect= content.Load<Effect>("Effects/Reflect");

            #region holes
            holes.Add(new Hole(411, 784, false, 1));
            holes.Add(new Hole(594, 760, false, 2));
            holes.Add(new Hole(227, 709, false, 3));
            holes.Add(new Hole(504, 653, false, 4));
            holes.Add(new Hole(336, 560, false, 5));
            holes.Add(new Hole(582, 468, false, 6));
            holes.Add(new Hole(424, 420, false, 7));
            holes.Add(new Hole(253, 370, false, 8));
            holes.Add(new Hole(512, 302, false, 9));
            holes.Add(new Hole(358, 260, false, 10));

            holes.Add(new Hole(485, 766));
            holes.Add(new Hole(280, 758));
            holes.Add(new Hole(204, 777));
            holes.Add(new Hole(653, 761));
            holes.Add(new Hole(361, 640));
            holes.Add(new Hole(321, 610));
            holes.Add(new Hole(391, 608));
            holes.Add(new Hole(495, 712));
            holes.Add(new Hole(547, 732));
            holes.Add(new Hole(614, 714));
            holes.Add(new Hole(614, 830));
            holes.Add(new Hole(412, 733));
            holes.Add(new Hole(271, 679));
            holes.Add(new Hole(166, 727));
            holes.Add(new Hole(194, 562));
            holes.Add(new Hole(288, 578));
            holes.Add(new Hole(438, 562));
            holes.Add(new Hole(471, 587));
            holes.Add(new Hole(516, 590));
            holes.Add(new Hole(583, 540));
            holes.Add(new Hole(545, 505));
            holes.Add(new Hole(642, 510));
            holes.Add(new Hole(655, 468));
            holes.Add(new Hole(618, 433));
            holes.Add(new Hole(507, 469));
            holes.Add(new Hole(423, 495));
            holes.Add(new Hole(336, 485));
            holes.Add(new Hole(294, 520));
            holes.Add(new Hole(171, 521));
            holes.Add(new Hole(348, 422));
            holes.Add(new Hole(298, 414));
            holes.Add(new Hole(254, 431));
            holes.Add(new Hole(209, 415));
            holes.Add(new Hole(192, 371));
            holes.Add(new Hole(211, 331));
            holes.Add(new Hole(254, 317));
            holes.Add(new Hole(314, 370));
            holes.Add(new Hole(424, 350));
            holes.Add(new Hole(512, 361));
            holes.Add(new Hole(582, 395));
            holes.Add(new Hole(544, 334));
            holes.Add(new Hole(574, 303));
            holes.Add(new Hole(544, 273));
            holes.Add(new Hole(513, 245));
            holes.Add(new Hole(480, 273));
            holes.Add(new Hole(451, 303));
            holes.Add(new Hole(296, 261));
            holes.Add(new Hole(359, 322));
            holes.Add(new Hole(422, 261));

            holes.Add(new Hole(345, 754,true,0,false));
            holes.Add(new Hole(416, 664, true, 0, false));
            holes.Add(new Hole(555, 652, true, 0, false));
            holes.Add(new Hole(675, 664, true, 0, false));
            holes.Add(new Hole(674, 718, true, 0, false));
            holes.Add(new Hole(146, 666, true, 0, false));
            holes.Add(new Hole(233, 621, true, 0, false));
            holes.Add(new Hole(619, 585, true, 0, false));
            holes.Add(new Hole(241, 539, true, 0, false));
            holes.Add(new Hole(145, 476, true, 0, false));
            holes.Add(new Hole(152, 387, true, 0, false));
            holes.Add(new Hole(386, 456, true, 0, false));
            holes.Add(new Hole(387, 384, true, 0, false));
            holes.Add(new Hole(461, 385, true, 0, false));
            holes.Add(new Hole(461, 456, true, 0, false));
            holes.Add(new Hole(507, 421, true, 0, false));
            holes.Add(new Hole(222, 273, true, 0, false));
            holes.Add(new Hole(326, 229, true, 0, false));
            holes.Add(new Hole(326, 289, true, 0, false));
            holes.Add(new Hole(390, 231, true, 0, false));
            holes.Add(new Hole(390, 289, true, 0, false));
            holes.Add(new Hole(652, 291, true, 0, false));
            holes.Add(new Hole(675, 428, true, 0, false));
            #endregion

#if DEBUG
            lines = new LineBatchComponent(ScreenManager.Game);
            lines.Initialize();
#endif

            for (int i = 0; i < 10; i++)
            {
                Hole h2 = holes.Find(h => h.nr == i+1);
                Light l = new Light(ScreenManager.Game, new Vector2(h2.x, h2.y),Color.LightYellow);
                l.Initialize();
                lights.Add(l);
            }

            msg = "     SPACE BASE     PRESS " + GameSettings.getKey("Settings.Start").ToString() + " TO START     ";
           
#if DEBUG
            sw=new StreamWriter("holes.txt",true);
#endif
        }

        public override void Unload()
        {
#if DEBUG
            sw.Close();
#endif
            base.Unload();
        }
       
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //update all lights
            foreach (Light l in lights) l.Update(gameTime);   
            
            //update all sounds
            mainLoop.Update(gameTime);

            //game state
            switch (state)
            {
                case LevelState.INIT:   
                    //move bar up until in start position
                    y1 -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 30;
                    y2 -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 30;
                    if (y1 < 830)
                    {
                        y1 = 830;
                        y2 = 830;
                        StartGame();                                                        
                    }
                    break;
                case LevelState.RESETTING:
                    //move bar down to get new ball
                    if (gameTime.TotalGameTime.TotalMilliseconds > lastRandomLight + 100)
                    {
                        if(randomLight!=0) lights[randomLight - 1].Off();
                        randomLight++;
                        if (randomLight == 11) randomLight = 1;
                        lights[randomLight - 1].On();
                        lastRandomLight = gameTime.TotalGameTime.TotalMilliseconds;
                    }                    
                    if(y1<900) y1 += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
                    if(y2<900) y2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
                    if (y1 >=900 && y2>=900)
                    {
                        if (lives != 0)
                        {
                            Init();                            
                        }
                        else
                        {
                            EndGame();          
                        }
                    }
                    break;
                case LevelState.ENDING:                   
                    endSequence -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (endSequence < 0)
                    {
                        StartAttractMode();
                    }
                    break;
                case LevelState.ATTRACT:
                    //update attract mode message
                    if (gameTime.TotalGameTime.TotalMilliseconds > lastMsgUpdate + 200)
                    {
                        msgpos++;
                        if (msgpos == msg.Length - 5) msgpos = 0;
                        lastMsgUpdate = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                    //update attract mode lights
                    if (gameTime.TotalGameTime.TotalMilliseconds > lastRandomLight + 5)
                    {
                        //if (MediaPlayer.State == MediaState.Playing)
                        /*{                            
                            MediaPlayer.GetVisualizationData(data);
                            float total = 0;
                            for (int i = 0; i < 256; i++)
                            {
                                total += data.Samples[i];
                            }
                            total = Math.Abs(total / 256)*40;

                            if (total > 1.0f) total = 1.0f;
                            for(int i=0;i<10;i++) lights[i].Off();
                            randomLight = (int)(total * 9) + 1;
                            for(int i=0;i<randomLight;i++) lights[i].On();                            
                        }*/

                        lastRandomLight = gameTime.TotalGameTime.TotalMilliseconds;
                    }                    
                    break;
                case LevelState.PLAYING:                   
                    //update timer
                    if (gameTime.TotalGameTime.TotalMilliseconds > lastTimerUpdate + 1000)
                    {
                        timer--;                        
                        if (timer == 0)
                        {                            
                            PlayerDie();
                            Reset();
                        }
                        if (timer == 2)
                        {
                            SoundCache.PlaySound("Oxygen Depleted");                            
                        }
                        if (timer == 30)
                        {
                            SoundCache.PlaySound("Oxygen Reserves Are Low");                                                   
                        }
                        if (timer == 45)
                        {
                            SoundCache.PlaySound("Hurry Up Pilot");
                        }
                        lastTimerUpdate = gameTime.TotalGameTime.TotalMilliseconds;
                    }

                    
                    break;

            }
                

            //physics
            angle = (float)Math.Atan((y2 - y1) / 820);

            if (!inhole)
            {
                //max angle
                if (angle > 0.15f) angle = 0.15f;
                if (angle < -0.15f) angle = -0.15f;

                //max left and right
                maxx = (float)Math.Sqrt(Math.Pow(820.0f, 2) - Math.Pow(y1 - y2, 2));
                maxx /= 2;
                maxx -= 140;

                //acceleration
                dx += (float)Math.Sin(angle) / 200.0f;

                //drag
                if (dx > 0)
                {
                    dx -= 0.00001f;
                    if (dx < 0) dx = 0;
                }
                if (dx < 0)
                {
                    dx += 0.00001f;
                    if (dx > 0) dx = 0;
                }

                x += dx * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                //bounce
                if (x > 410 + maxx)
                {
                    x = 410 + maxx;
                    dx = -dx / 5.0f;
                    if (Math.Abs(dx) < 0.01f) dx = 0;
                }
                if (x < 410 - maxx)
                {
                    x = 410 - maxx;
                    dx = -dx / 5.0f;
                    if (Math.Abs(dx) < 0.01f) dx = 0;
                }


                float by = 2 + (y1) + ((y2 - y1) * (x / 820.0f));
                Rectangle ballrect = new Rectangle((int)x - 5, (int)by - 3, 10, 6);

                foreach (Hole h in holes)
                {
                    Rectangle holerect = new Rectangle(h.x - 5, h.y - 8, 10, 10);
                    if (!h.small) holerect = new Rectangle(h.x - 8, h.y - 10, 16, 16);
                    if (ballrect.Intersects(holerect))
                    {
                        if (x > h.x) dx -= 0.0002f;
                        if (x < h.x) dx += 0.0002f;
                    }
                }

                ballrect = new Rectangle((int)x - 1, (int)by - 3, 2, 6);

                foreach (Hole h in holes)
                {
                    Rectangle holerect = new Rectangle(h.x - 1, h.y - 8, 2, 10);
                    if (!h.small) holerect = new Rectangle(h.x - 4, h.y - 10, 8, 16);
                    if (ballrect.Intersects(holerect) && !inhole)
                    {
                        inhole = true;
                        dx = 0;                        
                        fallx = x;
                        fally = 2 + (y1) + ((y2 - y1) * (x / 820.0f));
                        if (h.isBlack || h.nr != activeHole)
                        {
                            SoundCache.PlaySound("Come Back To Me");
                            PlayerDie();
                            Reset();
                        }
                        else
                        {
                            SoundCache.PlaySound("Entering Wormhole");
                            PlayerNextHole();
                            Reset();
                        }
                    }
                }
            }
            else
            {                               
                anim += 0.2f;                
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

            if (state == LevelState.ATTRACT && KeyPressed(GameSettings.getKey("Settings.Start"), repeat))
            {
                activeHole = 1;
                lives = 3;                
                MediaPlayer.Stop();                
                Init();
            }

            if (current.IsKeyDown(Keys.Tab) && last.IsKeyUp(Keys.Tab) && state == LevelState.ATTRACT)
            {
                ScreenManager.AddScreen(new MainMenu(), null);
            }

            if (state == LevelState.PLAYING)
            {                                               
                float angle2 = (float)Math.Atan((y2 - y1) / 820);

                if (KeyPressed(GameSettings.getKey("Settings.LeftUp"), repeat) && angle2 < 0.15f)
                {                   
                    y1--;
                }
                if (KeyPressed(GameSettings.getKey("Settings.LeftDown"), repeat) && angle2 > -0.15f)
                {                    
                    y1++;
                }

                if (KeyPressed(GameSettings.getKey("Settings.RightUp"), repeat) && angle2 > -0.15f)
                {                 
                    y2--;
                }
                if (KeyPressed(GameSettings.getKey("Settings.RightDown"), repeat) && angle2 < 0.15f)
                {                    
                    y2++;
                }
            }
            
        }
      
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.Identity);

            ScreenManager.GraphicsDevice.Clear(Color.Black);

            foreach (Light l in lights) l.Draw(gameTime);
           
            if (inhole)
            {                                
                spriteBatch.Draw(ball, new Vector2(fallx, anim + fally), new Rectangle(0, 0, 200, 200), Color.White, 0.0f, new Vector2(100, 100), 0.16f, SpriteEffects.None, 0.0f);                
            }

            spriteBatch.Draw(background, new Vector2(410, 512), new Rectangle(0, 0, 820, 1024), Color.White, 0, new Vector2(410, 512), 1.0f, SpriteEffects.None, 0.0f);

                     
            if (!inhole)
            {                
                spriteBatch.End();
                
                spriteBatch.Begin(0, BlendState.AlphaBlend,SamplerState.AnisotropicClamp,null,null, reflect);
                ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;                
                reflect.Parameters["back"].SetValue(background);
                reflect.Parameters["x"].SetValue(x);
                reflect.Parameters["y"].SetValue(2 + (y1) + ((y2 - y1) * (x / 820.0f)));  
                spriteBatch.Draw(ball, new Vector2(x, 2 + (y1) + ((y2 - y1) * (x / 820.0f))), new Rectangle(0,0,200, 200), Color.White, 0.0f, new Vector2(100, 100), 0.16f, SpriteEffects.None, 0.0f);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null, null, Matrix.Identity);
            }

            spriteBatch.Draw(part, new Vector2(410, (y1+y2)/2+4), new Rectangle(0, 0, 820, 43), Color.White, angle, new Vector2(410, 22), 1.0f, SpriteEffects.None, 0.0f);

#if DEBUG
            MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed && oldState.LeftButton==ButtonState.Released)
            {
                //holes.Add(new Hole(236, 490, false, 1));
                //holes.Add(new Hole(283, 480));
                sw.WriteLine("holes.Add(new Hole({0},{1}));", ms.X, ms.Y);
            }
            if (ms.RightButton == ButtonState.Pressed && oldState.RightButton == ButtonState.Released)
            {
                //holes.Add(new Hole(236, 490, false, 1));
                //holes.Add(new Hole(283, 480));
                sw.WriteLine("holes.Add(new Hole({0},{1},false,1));", ms.X, ms.Y);
            }
            oldState = ms;
           
            
            spriteBatch.Draw(ball, new Vector2(ms.X, ms.Y), new Rectangle(0, 0, 200, 200), Color.White, 0.0f, new Vector2(100, 100), 0.16f, SpriteEffects.None, 0.0f);
#endif
            
            
            spriteBatch.Draw(overlay, new Vector2(410, 512), new Rectangle(0, 0, 820, 1024), Color.White, 0, new Vector2(410, 512), 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, null, null, null, Matrix.Identity);

            //draw score or attract message
            spriteBatch.DrawStringMono(digital, "88888", new Vector2(40 * 16 -5 , -12 + 2 * 16), new Color(50, 0, 0));
                        
            if (state != LevelState.ATTRACT)
            {
                spriteBatch.DrawStringMono(digital, score.ToString("00000"), new Vector2(40 * 16 -5 , -12 + 2 * 16), Color.White);            
            }
            if (state == LevelState.ATTRACT)
            {
                spriteBatch.DrawStringMono(digital, msg.Substring(msgpos, 5), new Vector2(40 * 16 - 5, -12 + 2 * 16), Color.White);                
            }
          
            //draw timer
            spriteBatch.DrawStringMono(digital, "888", new Vector2(43 * 16 + 10, -14 + 6 * 16), new Color(50, 0, 0));
            spriteBatch.DrawStringMono(digital, timer.ToString("000"), new Vector2(43 * 16 + 10, -14 + 6 * 16), Color.White);            
           
            //draw lives
            spriteBatch.DrawStringMono(digital, "8", new Vector2(40 * 16 - 5, -14 + 6 * 16), new Color(50, 0, 0));
            spriteBatch.DrawStringMono(digital, lives.ToString("0"), new Vector2(40 * 16 - 5, -14 + 6 * 16), Color.White);



#if DEBUG
            double framesPerSecond = (1.0d / gameTime.ElapsedGameTime.TotalMilliseconds) * 1000.0d;
            framesPerSecond = Math.Ceiling(framesPerSecond);
            String frames = framesPerSecond.ToString() + " FPS";
            spriteBatch.DrawString(arcade, frames, new Vector2(0 * 16 , 0 * 16), Color.White);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:D}", System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024));
            String memory = sb.ToString()+" MB";
            spriteBatch.DrawString(arcade, memory, new Vector2(0 * 16, 1 * 16), Color.White);

            foreach (Hole h in holes)
            {
                if (h.small)
                {
                    LineBatchComponent.AddRect(new Rectangle(h.x - 5, h.y - 8, 10, 10), Color.Red);
                    LineBatchComponent.AddRect(new Rectangle(h.x - 1, h.y - 8, 2, 10), Color.Green);
                }
                else
                {
                    LineBatchComponent.AddRect(new Rectangle(h.x - 8, h.y - 10, 16, 16), Color.Red);
                    LineBatchComponent.AddRect(new Rectangle(h.x - 4, h.y - 10, 8, 16), Color.Green);
                }
            }


            float by = 2 + (y1) + ((y2 - y1) * (x / 820.0f));
            Rectangle ballrect = new Rectangle((int)x - 5, (int)by -3, 10,6);
            LineBatchComponent.AddRect(ballrect, Color.Yellow);

            ballrect = new Rectangle((int)x - 1, (int)by - 3, 2, 6);
            LineBatchComponent.AddRect(ballrect, Color.Green);

            
            lines.Draw(gameTime);
#endif
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}

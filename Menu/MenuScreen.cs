using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceBaseMono.Screens;

namespace SpaceBaseMono.Menu
{
    class MenuScreen : BaseScreen
    {       
        protected ContentManager content;        
        SpriteFont impact;
        SoundEffect bloop;
        
        int idx = 0;
        float y = 72;
       

        protected List<MenuItem> items=new List<MenuItem>();

        protected Game1 GetGame()
        {
            return (Game1)ScreenManager.Game;
        }

        public MenuScreen()
        {
            IsPopup = true;
        }


        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                impact = content.Load<SpriteFont>("Fonts/Arcade");
                bloop = content.Load<SoundEffect>("SoundEffects/laser");
            }
            
        }

        public void AddSpacer()
        {
            y+=16;
        }
       
        public MenuItem<String> AddMenuItem(String name,String[] values,String settingsName, Boolean enabled=false)
        {
            MenuItem<String> item = new MenuItem<String>(ScreenManager.Game, new Vector2(410, y), name, values, settingsName);
            item.Initialize();
            item.ItemEnabled = enabled;            
            items.Add(item);
            y += 24;
            return item;
        }

        public MenuItem<String> AddMenuItem(String name)
        {
            MenuItem<String> item = new MenuItem<String>(ScreenManager.Game, new Vector2(410, y), name,null, "");
            item.Initialize();
            item.ItemEnabled = false;
            items.Add(item);
            y += 24;
            return item;
        }

        public MenuItem<int> AddMenuItem(String name, int[] values, String settingsName, Boolean enabled = false)
        {
            MenuItem<int> item = new MenuItem<int>(ScreenManager.Game, new Vector2(410, y), name, values, settingsName);
            item.Initialize();
            item.ItemEnabled = enabled;
            items.Add(item);
            y += 24;
            return item;
        }

       
        public override void Unload()
        {
            //MediaPlayer.Stop();
            content.Unload();
            foreach(MenuItem item in items) item.Dispose();
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            //KeyboardState current = Keyboard.GetState();
            base.HandleInput(gameTime, input);

            /*if (current.IsKeyDown(Keys.Tab) && last.IsKeyUp(Keys.Tab))
            {                
                ScreenManager.RemoveScreen(this);                
            }*/

            
            if (KeyPressed(Keys.Down))
            {
                if (idx < items.Count - 1)
                {
                    items[idx].ItemEnabled = false;                        
                    idx++;
                    while (!items[idx].IsEnabled()) idx++;
                    items[idx].ItemEnabled = true;
                    //bloop.Play();
                }                
            }
            if (KeyPressed(Keys.Up))
            {
                if (idx >0)
                {
                    items[idx].ItemEnabled = false;
                    idx--;
                    while (!items[idx].IsEnabled()) idx--;
                    items[idx].ItemEnabled = true;
                    //bloop.Play();
                }
            }
            if(KeyPressed(Keys.Enter))
            {                
                items[idx].SelectItem();
                items[idx].Fire();
                if(!bloop.IsDisposed)bloop.Play();
            }

            last = current;
        }

        public void ChangeItems(String[] names, Boolean enabled)
        {
            foreach(String name in names)
            {
            if (enabled)
                foreach (MenuItem m in items.Where(mi=>mi.SettingsName==name)) m.Enable();
            if (!enabled)
                foreach (MenuItem m in items.Where(mi => mi.SettingsName == name)) m.Disable();
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            foreach(MenuItem item in items) item.Update(gameTime);
        }


       
        public override void Draw(GameTime gameTime)
        {
            

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            ScreenManager.FadeBackBufferToBlack(0.8f);
            //ScreenManager.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach(MenuItem item in items)
            {
                item.Draw(gameTime);
            }

            spriteBatch.DrawString(impact, "SPACE BASE CONFIGURATION", new Vector2(3 * 16, 2 * 16), Color.DarkOrchid);
            spriteBatch.DrawString(impact, "USE UP AND DOWN TO SELECT, ENTER TO CHANGE VALUE", new Vector2(2 * 16, 60 * 16), Color.White);
            //spriteBatch.DrawString(impact, "SPACE TO TOGGLE", new Vector2(2 * 16, 62 * 16), Color.White);

            spriteBatch.End();
        }

    }

    
}

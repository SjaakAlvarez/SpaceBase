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

namespace SpaceBaseMono.Menu
{
    public abstract class MenuItem :Microsoft.Xna.Framework.DrawableGameComponent
    {
        public MenuItem(Game game):base(game)
        {
        }

        public abstract void SelectItem();
        public abstract Boolean IsEnabled();
        public abstract void Enable();
        public abstract void Disable();
        public abstract void Fire();

        public abstract Boolean ItemEnabled {get;set;}
        public abstract String ItemName { get; set; }
        public abstract String SettingsName { get; set; }
    }

    public class MenuItem<T> : MenuItem
    {
        SpriteFont font;
        
        ContentManager content;
        SpriteBatch spriteBatch;

        int frame = 0;

        public override String ItemName { get; set; }
        public override String SettingsName { get; set; }
        public T[] values;
        public T value;
        Boolean enabled=true;
        double animtimer;
        double anim;
        Boolean fire;

        public Vector2 pos;
        Texture2D spacegun;
        Game game;
        public override Boolean ItemEnabled {get;set;}

        public delegate void SelectHandler(object sender, EventArgs e);
        public event SelectHandler Select;

        protected virtual void OnSelect(EventArgs e)
        {
            if (Select != null)
                Select(this, e);
        }

        public MenuItem(Game game,Vector2 pos,String itemName, T[] itemValues, String settingsName)
            : base(game)
        {
            this.ItemName = itemName;
            if(typeof(T)==typeof(String))
                if(settingsName!="") this.value = (T)Convert.ChangeType(GameSettings.getString(settingsName),typeof(T));

            if (typeof(T) == typeof(int))
                if (settingsName != "") this.value = (T)Convert.ChangeType(GameSettings.getInt(settingsName), typeof(T));

            this.values = itemValues;
            this.SettingsName = settingsName;
            this.pos = pos;
            this.game = game;

            
            //GameSettings.saveSettings();
        }

        public override void Fire()
        {
            fire = true;
            anim=1;
        }

        public void SetItem(T v)
        {
            value=v;
            GameSettings.set(SettingsName, v);
            GameSettings.saveSettings();
        }

        public override void Enable()
        {
            enabled = true;
        }

        public override void Disable()
        {
            enabled = false;
        }

        public override Boolean IsEnabled()
        {
            return enabled;
        }

        public override void SelectItem()
        {
            if (values != null)
            {
                int idx = values.ToList().IndexOf(value);
                if (idx < values.Length - 1) idx++; else idx = 0;
                value = values[idx];

                if (SettingsName != "")
                {
                    GameSettings.set(SettingsName,value);
                    GameSettings.saveSettings();
                }

            }
            OnSelect(new EventArgs());
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            if (content == null)
                content = new ContentManager(Game.Services, "Content");

            font = content.Load<SpriteFont>("Fonts/Arcade");            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            
            spacegun = content.Load<Texture2D>("lasergunanim");

        }

        
        protected override void UnloadContent()
        {
            content.Unload();
            base.UnloadContent();
        }


        

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            frame++;

            if (fire)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > animtimer + 41)
                {
                    anim++;
                    if (anim == 9) fire = false;
                    animtimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
        }



        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            Vector2 offset = Vector2.Zero;// font.MeasureString(itemName + " " + value) / 2;

            float size;            
            float rotation=0.0f;
            float alpha;
            if (enabled)
            {
                //rotation =  (float)Math.Sin((float)frame / 29.0f) * 0.02f;
                size = 1.0f;// (float)Math.Sin((float)frame / 15.0f) * 0.04f + 0.8f;                
                alpha = 1.0f;                
            }
            else
            {
                alpha=0.4f;
                rotation = 0.0f;
                size = 1.0f;
            }

            if(!fire)
            {
                if(ItemEnabled) spriteBatch.Draw(spacegun, new Vector2(2, pos.Y-8), new Rectangle(0,0,820,64),Color.White,0.0f,new Vector2(0,16),1.0f,SpriteEffects.None,0);
            }
            else
            {
                if(ItemEnabled) spriteBatch.Draw(spacegun, new Vector2(2, pos.Y-8), new Rectangle(0,(int)(anim*64),820,64),Color.White,0.0f,new Vector2(0,16),1.0f,SpriteEffects.None,0);
            }
            
            spriteBatch.DrawString(font, ItemName.ToUpper(), new Vector2(3*16,pos.Y), (ItemEnabled?Color.White:Color.Gray)*alpha,rotation, offset, size, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, value==null?"":value.ToString().ToUpper(), new Vector2(26*16, pos.Y), (ItemEnabled ? Color.White : Color.Gray) * alpha, rotation, offset, size, SpriteEffects.None, 0.0f);            
           
            spriteBatch.End();

        }
    }
}

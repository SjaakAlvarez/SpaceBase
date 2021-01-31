using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ExtensionMethods
{
    public static class MyExtensions
    {

        public static void DrawStringMono(this SpriteBatch spriteBatch, SpriteFont font1, String text, Vector2 position, Color color1)
        {
            for (int i = 0; i < text.Length; i++)
            {
                spriteBatch.DrawString(font1, text.Substring(i,1), position+new Vector2(i*31,0), color1);
            }
            
        }

        public static void DrawStringOnBlack(this SpriteBatch spriteBatch, SpriteFont font1, String text, Vector2 position, Color color1,Texture2D black)
        {
            Vector2 size=font1.MeasureString(text);
            spriteBatch.Draw(black,new Rectangle((int)position.X-1,(int)position.Y-1,(int)size.X,(int)size.Y-1),Color.Black);
            spriteBatch.DrawString(font1, text.ToUpper(), position, color1);
            

        }
            
    }
}

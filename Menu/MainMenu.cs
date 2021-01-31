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


namespace SpaceBaseMono.Menu
{
    class MainMenu : MenuScreen
    {

        

        public MainMenu() :base()
        {
        }

        public override void Activate(bool instancePreserved)
        {
            base.Activate(instancePreserved);



            AddMenuItem("Lives", new int[] { 3, 4, 5 }, "Settings.Lives",true);            
            AddMenuItem("Free Play", new String[] { "true", "false" }, "Settings.FreePlay");
            AddMenuItem("Show Title Screen", new String[] { "true", "false" }, "Settings.ShowTitleScreen");
//            AddSpacer();

            AddMenuItem("HLSL effects", new String[] { "true", "false" }, "HLSL.Enabled");
            
            AddSpacer();

            
            MenuItem<String> exit = AddMenuItem("Exit configuration");            
            exit.Select += new MenuItem<String>.SelectHandler(exit_Select);

            
            
        }

        

       

        void exit_Select(object sender, EventArgs e)
        {
            GameSettings.saveSettings();
            ScreenManager.RemoveScreen(this);
        }

        
    }
}

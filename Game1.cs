using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using GameStateManagement;
using SpaceBaseMono.Screens;
using System.Diagnostics;
using System.Xml.Linq;
using SpaceBaseMono.Menu;
using SpaceBaseMono.Timers;


namespace SpaceBaseMono
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        
        public Camera camera;
       
       
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            GameSettings.registerSetting("Game", "Graphics.FullScreen", "false", GameSettings.ValidateBool, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "Graphics.Width", "820", GameSettings.ValidateInt, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "Graphics.Height", "1024", GameSettings.ValidateInt, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "Graphics.Vertical", "false", GameSettings.ValidateBool, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "Graphics.Flip", "false", GameSettings.ValidateBool, GameSettings.SaveFormatStd);
            
            GameSettings.registerSetting("Game", "HLSL.Enabled", "false", GameSettings.ValidateString, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "HLSL.ScanLineOffIntensity", 0.7f, GameSettings.ValidateFloat, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "HLSL.Curvature", 0.03f, GameSettings.ValidateFloat, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "HLSL.RedShift", 1.0f, GameSettings.ValidateFloat, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "HLSL.GreenShift", 2.0f, GameSettings.ValidateFloat, GameSettings.SaveFormatStd);

            GameSettings.registerSetting("Game", "Settings.FreePlay", "false", GameSettings.ValidateString, GameSettings.SaveFormatStd);            
            GameSettings.registerSetting("Game", "Settings.Lives", "3", GameSettings.ValidateInt, GameSettings.SaveFormatStd);

            GameSettings.registerSetting("Game", "Settings.LeftUp", "A", GameSettings.ValidateString, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "Settings.LeftDown", "Z", GameSettings.ValidateString, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "Settings.RightUp", "K", GameSettings.ValidateString, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "Settings.RightDown", "M", GameSettings.ValidateString, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "Settings.Start", "S", GameSettings.ValidateString, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game", "Settings.Quit", "Escape", GameSettings.ValidateString, GameSettings.SaveFormatStd);

            GameSettings.registerSetting("Game", "Settings.ShowTitleScreen", "true", GameSettings.ValidateString, GameSettings.SaveFormatStd);

            this.graphics.PreferredBackBufferWidth = GameSettings.getInt("Graphics.Width");
            this.graphics.PreferredBackBufferHeight = GameSettings.getInt("Graphics.Height");
            this.graphics.IsFullScreen = GameSettings.getBool("Graphics.FullScreen");


            camera = new Camera(this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);

            if (GameSettings.getBool("Graphics.Vertical"))
            {
                float scaleW = this.graphics.PreferredBackBufferWidth / Constants.HEIGHT;
                float scaleH = this.graphics.PreferredBackBufferHeight / Constants.WIDTH;

                camera.Zoom = MathHelper.Min(scaleH, scaleW);
                if (GameSettings.getBool("Graphics.Flip"))
                    camera.Rotation = -MathHelper.PiOver2;
                else
                    camera.Rotation = MathHelper.PiOver2;
            }
            else
            {
                float scaleW = this.graphics.PreferredBackBufferWidth / Constants.WIDTH;
                float scaleH = this.graphics.PreferredBackBufferHeight / Constants.HEIGHT;

                camera.Zoom = MathHelper.Min(scaleH, scaleW);
                if (GameSettings.getBool("Graphics.Flip"))
                    camera.Rotation = -MathHelper.Pi;
                else
                    camera.Rotation = 0;
            }

            camera.Move(new Vector2(Constants.WIDTH / 2, Constants.HEIGHT/2));

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);


            screenManager.AddScreen(new MainScreen(), null);
            screenManager.AddScreen(new LevelScreen(), null);
            //if(GameSettings.getBool("Settings.ShowTitleScreen")) 
                screenManager.AddScreen(new TitleScreen(), null);
            

             
        }


        
        

      
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            

           

            // TODO: use this.Content to load your game content here
        }

      

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(GameSettings.getKey("Settings.Quit")))
                this.Exit();

            // TODO: Add your update logic here

            TimerManager.Instance.Update(gameTime); 

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {                        
            

            base.Draw(gameTime);
        }
    }
}

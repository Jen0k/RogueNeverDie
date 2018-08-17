using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueNeverDie.Engine;
using RogueNeverDie.Engine.Common;
using RogueNeverDie.Engine.GameObjects;

namespace RogueNeverDie
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRogue : Game
    {      
		public static GraphicsDeviceManager Graphics;      
		public static LogManager LogManager;

		protected SpriteBatch _spriteBatch;
		protected ResourceManager _resourceManager;
		protected StateManager _stateManager;
		protected ResourseLoader _resourseLoader;

		protected SpriteFont commonFont;
              
        public GameRogue()
        {
            Graphics = new GraphicsDeviceManager(this);

			Content.RootDirectory = Config.ContentDirectory;
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
			_stateManager = new StateManager();

			Graphics.PreferredBackBufferWidth = Config.ScreenWight;
			Graphics.PreferredBackBufferHeight = Config.ScreenHeight;
			Graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			commonFont = Content.Load<SpriteFont>(Config.CommonFont);

			if (LogManager == null)
			{
				LogManager = new LogManager(new Vector2(4, Graphics.PreferredBackBufferHeight - 4), 0, commonFont, Color.Green, 5000);
			}

			_resourceManager = new ResourceManager();

			_resourseLoader = new ResourseLoader(Path.Combine(Environment.CurrentDirectory, Content.RootDirectory));
			_resourseLoader.LoadFromConfig(Config.ResoursesRootIndex, _resourceManager, Content);         
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
			_stateManager.Update(gameTime);

			KeyboardState keyboardState = Keyboard.GetState();

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Q))
                Exit();

			// TODO: Add your update logic here
			if (keyboardState.IsKeyDown(Keys.A)) {
				Keys[] pressedKeys = keyboardState.GetPressedKeys();
				string kk = "";
				foreach (Keys k in pressedKeys) {
					kk += k.ToString();
				}
				LogManager.SendMessage(kk);
			}

			LogManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(Color.Black);
            
			// TODO: Add your drawing code here
			_spriteBatch.Begin();

			LogManager.Draw(_spriteBatch);

			_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}



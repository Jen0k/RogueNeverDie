using System;
using System.IO;
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
		public GraphicsDeviceManager Graphics;
		public SpriteBatch SpriteBatch;
		public GameConsole GameConsole;
		public StateManager StateManager;
		public ResourceManager ResourceManager;
		public ResourseLoader ResourseLoader;

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
			StateManager = new StateManager();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			commonFont = Content.Load<SpriteFont>(Config.CommonFont);

			GameConsole = new GameConsole(new Vector2(4, Graphics.PreferredBackBufferHeight - 4), 0, commonFont, Color.Green, 5000);
			ResourceManager = new ResourceManager();
            ResourseLoader = new ResourseLoader(Path.Combine(Environment.CurrentDirectory, Content.RootDirectory), this);

            
			ResourseLoader.LoadFromConfig(Config.ResoursesRootIndex);         
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
			StateManager.Update(gameTime);

			KeyboardState keyboardState = Keyboard.GetState();

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Q))
                Exit();

			// TODO: Add your update logic here
			if (keyboardState.IsKeyDown(Keys.A)) {
				try
				{
					GameConsole.SendMessage(new GameConsoleMessage("Hello World!", ResourceManager.Load<SpriteFont>("onsole"), Color.Green, DateTime.Now, new TimeSpan(0, 0, 0, 0, 5000)));
				}
				catch (Exception e) 
				{
					GameConsole.SendError(e.Message);
				}
			}

			GameConsole.Update(gameTime);

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
			SpriteBatch.Begin();

			GameConsole.Draw(SpriteBatch);

			SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}



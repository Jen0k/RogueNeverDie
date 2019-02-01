using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueNeverDie.Engine;
using RogueNeverDie.Engine.Common;
using RogueNeverDie.Engine.GameObjects;
using RogueNeverDie.Engine.Factories;

namespace RogueNeverDie
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRogue : Game
    {      
		public static GraphicsDeviceManager Graphics;      
		public static LogManager LogManager;

        public static SpriteFactory SpriteFactory;
        public static TileFactory TileFactory;

		protected SpriteBatch _spriteBatch;
		protected ResourceManager _resourceManager;
		protected StateManager _stateManager;
		protected ResourseLoader _resourseLoader;
		protected Commander _commander;

		protected SpriteFont commonFont;

		protected Level testLevel;

        ISprite test;

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

            _stateManager.AddState("global", CommonStates.Global, CommonStates.DrawNothing, StateStatus.Update, 
                new Dictionary<string, object> { { "game", this } });

			_stateManager.AddState("testLoop", CommonStates.TestsUpdates, CommonStates.DrawNothing, StateStatus.Update, 
                new Dictionary<string, object> { { "graphics", Graphics } });

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
				LogManager = new LogManager(new Vector2(4, Graphics.PreferredBackBufferHeight - 32), 0, commonFont, Color.Green, 5000);

				_stateManager.AddState("logger", LogManager.Update, LogManager.Draw, StateStatus.UpdateAndDraw, new Dictionary<string, object>());
			}

			_resourceManager = new ResourceManager();

			_resourseLoader = new ResourseLoader(Path.Combine(Environment.CurrentDirectory, Content.RootDirectory));
			_resourseLoader.LoadFromConfig(Config.ResoursesRootIndex, _resourceManager, Content);

            SpriteFactory = new SpriteFactory(_resourceManager);
            _stateManager.AddState("updateSprites", SpriteFactory.Update, CommonStates.DrawNothing, StateStatus.Update,
                new Dictionary<string, object>());
            TileFactory = new TileFactory(_resourceManager, SpriteFactory);

            _commander = new Commander(_resourceManager.Load<SpriteFont>("console"));
			_stateManager.AddState("commander", _commander.Update, _commander.Draw, StateStatus.DoNothing, 
                new Dictionary<string, object> { { "gameWindow", Window } });

			testLevel = new Level(new Point(100, 100));

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    //Tile newTile = new Tile(testLevel, new Point(i, j), new Sprite(_resourceManager.Load<Texture2D>("defaultTileTexture"), new Rectangle(0, 0, 32, 32)));
                }
            }
            //_stateManager.AddState("testLevel", testLevel.Update, testLevel.Draw, StateStatus.UpdateAndDraw, new Dictionary<string, object>());

            test = TileFactory.CreateSubtile("testTileset", "testTileset", 0, 0, 32, 0, true, false, false, false);
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
			_stateManager.UpdateStates(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(Color.Green);
            
			// TODO: Add your drawing code here
			_spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            test.Draw(_spriteBatch, new Vector2(128, 128));
			_stateManager.DrawStates(_spriteBatch, gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}



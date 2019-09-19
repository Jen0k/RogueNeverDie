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

        public static SpriteFactory SpriteFactory;
        public static TileFactory TileFactory;
        public static LevelFactory LevelFactory;

        protected StateManager StateManager;
        protected SpriteBatch SpriteBatch;
        protected ResourceManager ResourceManager;

        public ScreenMessager ScreenMessager;
        protected Commander Commander;
        protected InformationPanel InformationPanel;
        protected FPSCounter FpsCounter;

        protected Level testLevel;

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
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            StateManager = new StateManager();
            ResourceManager = new ResourceManager();

            StateManager.AddState("global", CommonStates.Global, CommonStates.DrawNothing, StateStatus.Update,
                new Dictionary<string, object> { { "game", this } });

            StateManager.AddState("testLoop", CommonStates.TestsUpdates, CommonStates.DrawNothing, StateStatus.Update,
                new Dictionary<string, object> { {"game", this }, { "graphics", Graphics } });

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
            ResourceManager.LoadFromConfig(Path.Combine(Environment.CurrentDirectory, Content.RootDirectory), Config.ResoursesRootIndex, Content);

            ScreenMessager = new ScreenMessager(new Vector2(4, Graphics.PreferredBackBufferHeight - 32), 0, ResourceManager.Load<SpriteFont>("console"), Color.Green, 5000);
            StateManager.AddState("logger", ScreenMessager.Update, ScreenMessager.Draw, StateStatus.UpdateAndDraw, new Dictionary<string, object>());

            Commander = new Commander(ResourceManager.Load<SpriteFont>("console"));
            StateManager.AddState("commander", Commander.Update, Commander.Draw, StateStatus.DoNothing,
                new Dictionary<string, object> { { "screenMessager", ScreenMessager }, { "gameWindow", Window } });

            FpsCounter = new FPSCounter();
            StateManager.AddState("fpscounter", FpsCounter.Update, CommonStates.DrawNothing, StateStatus.Update,
                new Dictionary<string, object>());

            InformationPanel = new InformationPanel(ResourceManager.Load<SpriteFont>("console"));
            StateManager.AddState("infpanel", CommonStates.UpdateNonthing, InformationPanel.Draw, StateStatus.Draw,
                new Dictionary<string, object>());

            if (SpriteFactory == null) 
            {
                SpriteFactory = new SpriteFactory(ResourceManager);
                StateManager.AddState("updateSprites", SpriteFactory.Update, CommonStates.DrawNothing, StateStatus.Update,
                    new Dictionary<string, object>());
            }
            if (TileFactory == null) { TileFactory = new TileFactory(ResourceManager, SpriteFactory); }
            if (LevelFactory == null) { LevelFactory = new LevelFactory(ResourceManager, TileFactory); }

            FpsCounter.FramesPerSecond += InformationPanel.CreateIndicator("Frames per second");
            //FpsCounter.FramesPerSecond -= InformationPanel.DeleteIndicator("Frames per second");

            testLevel = new Level(new Point(400, 400));
            testLevel.SpriteDrawed += InformationPanel.CreateIndicator("Tiles drawed");
            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    Tile newTile = new Tile(testLevel, new Point(i, j));
                    newTile.SetLayer(0, TileFactory.CreateLayer("dungeonBricks", Color.Green));
                }
            }

            //LevelFactory.FillRectangle(testLevel, "dungeonBricks", Color.Red, new Rectangle(1, 1, 5, 5), 2, true);
            //LevelFactory.FillRectangle(testLevel, "dungeonBricks", Color.Blue, new Rectangle(3, 3, 5, 5), 1, true);

            LevelFactory.GenerateNonRegularDungeon(testLevel);

            StateManager.AddState("testLevel", testLevel.Update, testLevel.Draw, StateStatus.UpdateAndDraw, new Dictionary<string, object>());
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
            StateManager.UpdateStates(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Cyan);

            // TODO: Add your drawing code here
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp);
            StateManager.DrawStates(SpriteBatch, gameTime);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}



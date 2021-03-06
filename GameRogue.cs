﻿using System;
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
        public static LevelFactory LevelFactory;

        protected SpriteBatch _spriteBatch;
        protected ResourceManager _resourceManager;
        protected StateManager _stateManager;
        protected ResourseLoader _resourseLoader;
        protected Commander _commander;
        protected InformationPanel _informationPanel;
        protected FPSCounter _fpsCounter;

        protected SpriteFont commonFont;

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
            LevelFactory = new LevelFactory(_resourceManager, TileFactory);

            _commander = new Commander(_resourceManager.Load<SpriteFont>("console"));
            _stateManager.AddState("commander", _commander.Update, _commander.Draw, StateStatus.DoNothing,
                new Dictionary<string, object> { { "gameWindow", Window } });

            _fpsCounter = new FPSCounter();
            _stateManager.AddState("fpscounter", _fpsCounter.Update, CommonStates.DrawNothing, StateStatus.Update,
                new Dictionary<string, object>());

            _informationPanel = new InformationPanel(_resourceManager.Load<SpriteFont>("console"));
            _stateManager.AddState("infpanel", CommonStates.UpdateNonthing, _informationPanel.Draw, StateStatus.Draw,
                new Dictionary<string, object>());

            _fpsCounter.FramesPerSecond += _informationPanel.CreateIndicator("Frames per second");
            //_fpsCounter.FramesPerSecond -= _informationPanel.DeleteIndicator("Frames per second");

            testLevel = new Level(new Point(400, 400));
            testLevel.SpriteDrawed += _informationPanel.CreateIndicator("Tiles drawed");
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

            _stateManager.AddState("testLevel", testLevel.Update, testLevel.Draw, StateStatus.UpdateAndDraw, new Dictionary<string, object>());
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
            GraphicsDevice.Clear(Color.Cyan);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp);
            _stateManager.DrawStates(_spriteBatch, gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}



using System;
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
        protected GraphicsDeviceManager graphics;
		protected SpriteBatch spriteBatch;
        
		protected SpriteFont commonFont;

		protected Texture2D sonicTex;
		protected Sprite test;
        
		protected StateManager stateManager;
              
        public GameRogue()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
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
			stateManager = new StateManager();


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			commonFont = Content.Load<SpriteFont>("Fonts/Common");

			sonicTex = Content.Load<Texture2D>("Textures/Common/sonic");

			test = new Sprite(sonicTex, new Rectangle(62, 0, 31, 40), 3, 3, 4);
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
			stateManager.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Q))
                Exit();

			// TODO: Add your update logic here
			test.Update(gameTime);

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
			spriteBatch.Begin();
			//spriteBatch.Draw(sonicTex, new Vector2(16, 16), Color.White);
			test.Draw(spriteBatch, new Vector2(0, 0));
			//spriteBatch.DrawString(commonFont, panda.GetHashCode().ToString(), new Vector2(0, 0), Color.Green);
			spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}



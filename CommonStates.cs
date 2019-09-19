using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueNeverDie.Engine;

namespace RogueNeverDie
{
    public class CommonStates
    {
		protected CommonStates() { }

		public static void UpdateNonthing(GameTime gameTime, Dictionary<string, object> parameters) { }
		public static void DrawNothing(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters) { }

		public static void Global(GameTime gameTime, Dictionary<string, object> parameters)
		{
			StateManager stateManager = (StateManager)parameters["stateManager"];
			KeyboardState keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Keys.Q))
			{
				((GameRogue)parameters["game"]).Exit();
			}

			if (keyboardState.IsKeyDown(Keys.Tab)) {
				stateManager.SetStateStatus("commander", StateStatus.UpdateAndDraw, new List<string> { "global" }, new List<string>());
				stateManager.SetStateStatus("global", StateStatus.DoNothing);
			}
		}

		public static void TestsUpdates(GameTime gameTime, Dictionary<string, object> parameters) 
		{
			KeyboardState keyboardState = Keyboard.GetState();
			StateManager stateManager = (StateManager)parameters["stateManager"];

            GraphicsDeviceManager graphics = (GraphicsDeviceManager)parameters["graphics"];

            // TODO: Add your update logic here
            /*if (keyboardState.IsKeyDown(Keys.A)) {
                Keys[] pressedKeys = keyboardState.GetPressedKeys();
                string kk = "";
                foreach (Keys k in pressedKeys) {
                    kk += k.ToString();
                }
                LogManager.SendMessage(kk);
            }*/


            if (keyboardState.IsKeyDown(Keys.A))
            {
                ((GameRogue)parameters["game"]).ScreenMessager.SendMessage("!!!!!!!");
            }

            if (keyboardState.IsKeyDown(Keys.Z))
            {
                graphics.PreferredBackBufferWidth = 320;
                graphics.PreferredBackBufferHeight = 240;
                graphics.ApplyChanges();
            }
            if (keyboardState.IsKeyDown(Keys.X))
            {
                graphics.PreferredBackBufferWidth = 800;
                graphics.PreferredBackBufferHeight = 600;
                graphics.ApplyChanges();
            }
            if (keyboardState.IsKeyDown(Keys.C))
            {
                graphics.PreferredBackBufferWidth = 1024;
                graphics.PreferredBackBufferHeight = 768;
                graphics.ApplyChanges();
            }
            if (keyboardState.IsKeyDown(Keys.V))
            {
				//stateManager.SetStateStatus("logger", StateStatus.Draw);
            }
		}
    }
}

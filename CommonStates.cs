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
			KeyboardState keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Keys.Q))
			{
				((GameRogue)parameters["game"]).Exit();
			}
		}

		public static void TestsUpdates(GameTime gameTime, Dictionary<string, object> parameters) 
		{
			KeyboardState keyboardState = Keyboard.GetState();
			StateManager stateManager = (StateManager)parameters["stateManager"];

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
				GameRogue.LogManager.SendMessage("!!!!!!!");
            }

            if (keyboardState.IsKeyDown(Keys.Z))
            {
				stateManager.SetStateStatus("logger", StateStatus.DoNothing);
            }
            if (keyboardState.IsKeyDown(Keys.X))
            {
				stateManager.SetStateStatus("logger", StateStatus.Update);
            }
            if (keyboardState.IsKeyDown(Keys.C))
            {
				stateManager.SetStateStatus("logger", StateStatus.UpdateAndDraw);
            }
            if (keyboardState.IsKeyDown(Keys.V))
            {
				stateManager.SetStateStatus("logger", StateStatus.Draw);
            }
		}
    }
}

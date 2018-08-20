using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine
{
	public delegate void StateUpdateTask(GameTime gameTime, Dictionary<string, object> parameters);
	public delegate void StateDrawTask(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters);

    public struct State
    {
		public State(StateUpdateTask UpdateTask, StateDrawTask DrawTask, Dictionary<string, object> Parameters) {
			this.UpdateTask = UpdateTask;
			this.DrawTask = DrawTask;
			this.Parameters = Parameters;
		}
        
		public StateUpdateTask UpdateTask;
		public StateDrawTask DrawTask;
		public Dictionary<string, object> Parameters;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine
{
	public enum StateStatus { DoNothing = 0, Update = 1, Draw = 2, UpdateAndDraw = 3 }

	public class StateManager
	{
		public StateManager() {
			_storage = new Dictionary<string, State>();
			_statues = new Dictionary<string, StateStatus>();
		}

		protected Dictionary<string, State> _storage;
		protected Dictionary<string, StateStatus> _statues;

		public void AddState(string id, StateUpdateTask updateTask, StateDrawTask drawTask, StateStatus status, Dictionary<string, object> parameters) {
			RemoveState(id);

			if (parameters.ContainsKey("stateManager")) {
				parameters.Remove("stateManager");
			}

			parameters.Add("stateManager", this);

			_storage.Add(id, new State(updateTask, drawTask, parameters));
			_statues.Add(id, status);
		}
        
		public void SetStateStatus(string id, StateStatus status) {
			if (_statues.ContainsKey(id)) {
				_statues[id] = status;
			}
		}

		public void RemoveState(string id) {
			if (_statues.ContainsKey(id))
            {
				_statues.Remove(id);
				_storage.Remove(id);
            }
		}

		public void UpdateStates(GameTime gameTime) {
			foreach (KeyValuePair<string, State> state in _storage) {
				if (_statues[state.Key] == StateStatus.Update || _statues[state.Key] == StateStatus.UpdateAndDraw) {
					state.Value.UpdateTask(gameTime, state.Value.Parameters);
				}
			}
		}
        
		public void DrawStates(SpriteBatch spriteBatch, GameTime gameTime) {
			foreach (KeyValuePair<string, StateStatus> status in _statues)
            {
                if (status.Value == StateStatus.Draw || status.Value == StateStatus.UpdateAndDraw)
                {
					_storage[status.Key].DrawTask(spriteBatch, gameTime, _storage[status.Key].Parameters);
                }
            }
		}
	}
}

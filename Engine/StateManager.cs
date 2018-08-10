using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace RogueNeverDie.Engine
{
	public delegate void StateUpdateTask(GameTime gameTime, StateManager instanse);

	public class StateManager
	{
		public StateManager()
		{
			_stateTasks = new Dictionary<string, StateUpdateTask>();
			_stateStatus = new Dictionary<string, bool>();
		}

		protected Dictionary<string, StateUpdateTask> _stateTasks;
		protected Dictionary<string, bool> _stateStatus;

		public void AddState(string id, bool status, StateUpdateTask task)
		{
			RemoveState(id);
			_stateTasks.Add(id, task);
			_stateStatus.Add(id, status);
		}

		public void RemoveState(string id)
		{
			if (_stateStatus.Keys.Contains(id))
			{
				_stateStatus.Remove(id);
				_stateTasks.Remove(id);
			}
		}

		public void SetStateStatus(string id, bool status) {
			if (_stateStatus.Keys.Contains(id))
			{
				_stateStatus[id] = status;
			}        
		}

		public void Update(GameTime gameTime)
		{
			foreach (KeyValuePair<string, bool> status in _stateStatus)
			{
				if (status.Value)
				{
					_stateTasks[status.Key](gameTime, this);
				}
			}
		}
	}
}

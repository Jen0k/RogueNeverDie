using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine
{
	public class LogManager
    {
		public LogManager(Vector2 Position, float MessageSpacing, SpriteFont DefaultFont, Color DefaultColor, int DefaultLifeTime)
        {
			_messageList = new List<LogMessage>();
			_elapsedLifeTimes = new List<TimeSpan>();
			_defaultLifeTime = new TimeSpan(0, 0, 0, 0, DefaultLifeTime);

			this.DefaultColor = DefaultColor;
			this.DefaultFont = DefaultFont;
			this.Position = Position;
			this.MessageSpacing = MessageSpacing;
        }
        
		public Vector2 Position;
		public float MessageSpacing;
		public SpriteFont DefaultFont;
		public Color DefaultColor;
		public int DefaultLifeTime { get => (int)_defaultLifeTime.TotalMilliseconds; set => _defaultLifeTime = new TimeSpan(0, 0, 0, 0, value); }

		protected TimeSpan _defaultLifeTime;
		protected List<LogMessage> _messageList;
		protected List<TimeSpan> _elapsedLifeTimes;
        
		public void SendError(string text) {
			SendMessage(new LogMessage(String.Format("Ошибка: {0}", text), DefaultFont, Color.Red, DateTime.Now, _defaultLifeTime));
		}

		public void SendMessage(string text) {
			SendMessage(new LogMessage(text, DefaultFont, DefaultColor, DateTime.Now, _defaultLifeTime));
		}

		public void SendMessage(LogMessage message) {
			_messageList.Add(message);
			_elapsedLifeTimes.Add(TimeSpan.Zero);
		}
        
		public void Update(GameTime gameTime) {
			List<int> lifeIsOverIndexes = new List<int>();

			for (int i = 0; i < _elapsedLifeTimes.Count; i++) {
				_elapsedLifeTimes[i] += gameTime.ElapsedGameTime;
				if (_elapsedLifeTimes[i] >= _messageList[i].LifeTimeTotal) {
					lifeIsOverIndexes.Add(i);
				}
			}

			for (int i = lifeIsOverIndexes.Count - 1; i >= 0; i--)
			{
				_messageList.RemoveAt(lifeIsOverIndexes[i]);
				_elapsedLifeTimes.RemoveAt(lifeIsOverIndexes[i]);
			}
		}

		public void Draw(SpriteBatch spriteBatch) {
			float postionY = Position.Y - MessageSpacing / 2.0f;
			for (int i = _messageList.Count - 1; i >= 0; i--)
			{
				postionY -= _messageList[i].Font.LineSpacing;

				spriteBatch.DrawString(_messageList[i].Font, 
				                       String.Format("{0}: {1}", _messageList[i].DateCreated.ToLocalTime().ToShortTimeString(), _messageList[i].Text),
				                       new Vector2(Position.X, postionY), _messageList[i].Color);

				postionY -= MessageSpacing;
			}
		}
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueNeverDie.Engine
{
	public class Commander : IState
    {
		public Commander(SpriteFont Font)
        {
			this.Font = Font;


			_buffer = new StringBuilder();
			_keyStateOne = Keyboard.GetState();
			_keyStateTwo = Keyboard.GetState();
        }

		public Rectangle Bounds;
		public SpriteFont Font;
		public Color Background;
		public Color Border;
		public int BorderSize;

		protected StringBuilder _buffer;
		protected KeyboardState _keyStateOne;
		protected KeyboardState _keyStateTwo;

		public void Update(GameTime gameTime, Dictionary<string, object> parameters)
		{
			_keyStateTwo = _keyStateOne;
			_keyStateOne = Keyboard.GetState();

			foreach (Keys key in _keyStateOne.GetPressedKeys()) {
				if (_keyStateTwo.IsKeyUp(key)) {
					switch (key) {
						case Keys.Back:
							if (_buffer.Length > 0)
							{
								_buffer.Remove(_buffer.Length - 1, 1);
							}
							break;
						case Keys.Enter:
							GameRogue.LogManager.SendMessage(_buffer.ToString());
							_buffer.Clear();
							break;
						default:
							_buffer.Append(key.ToString());
							break;
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters)
		{
			
			spriteBatch.DrawString(Font, _buffer.ToString(), new Vector2(16, 16), new Color(new Vector4(1, 1, 1, 0.5f)));
		}
	}
}

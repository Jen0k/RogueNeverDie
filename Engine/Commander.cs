using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueNeverDie.Engine
{
	public class Commander : IState
    {
		public Commander(SpriteFont Font, Rectangle Bounds)
        {
			this.Font = Font;
			this.Bounds = Bounds;

			_buffer = new StringBuilder();
			_keyStateOne = Keyboard.GetState();
			_keyStateTwo = Keyboard.GetState();
            
			_capslock = false;

			_borderTexture= new Texture2D(GameRogue.Graphics.GraphicsDevice, Bounds.Width, Bounds.Height);
			_backgroundTexture = new Texture2D(GameRogue.Graphics.GraphicsDevice, Bounds.Width, Bounds.Height);
        }
        
		public Rectangle Bounds;
		public SpriteFont Font;
		public Color Background
		{
			get
			{
				Color[] pixels = new Color[Bounds.Width * Bounds.Height];
				_backgroundTexture.GetData(pixels);
				return pixels[0];
			}
			set
			{
				_backgroundTexture.SetData(Enumerable.Repeat(value, Bounds.Width * Bounds.Height).ToArray());
			}
		}
		public Color Border { get => _border; }
		public int BorderSize { get => _borderSize; }

		protected Color _border;
		protected int _borderSize;
        
		protected Texture2D _borderTexture;
		protected Texture2D _backgroundTexture;

		protected StringBuilder _buffer;
		protected KeyboardState _keyStateOne;
		protected KeyboardState _keyStateTwo;

		protected bool _capslock;

		protected static List<Keys> _pureCharKeys = new List<Keys> 
		{
            Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
            Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
            Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z
        };
		protected static Dictionary<Keys, Char> _convertingCharKeys = new Dictionary<Keys, char>
		{
            { Keys.NumPad0, '0' }, { Keys.NumPad1, '1' }, { Keys.NumPad2, '2' }, { Keys.NumPad3, '3' }, { Keys.NumPad4, '4' },
            { Keys.NumPad5, '5' }, { Keys.NumPad6, '6' }, { Keys.NumPad7, '7' }, { Keys.NumPad8, '8' }, { Keys.NumPad9, '9' },
            { Keys.Space, ' ' }, { Keys.Divide, '/' }, { Keys.Multiply, '*' }, { Keys.Subtract, '-' }, { Keys.Add, '+' }, { Keys.Decimal, '.' }
		};
		protected static Dictionary<Keys, Char> _oemNormalCharKeys = new Dictionary<Keys, char>
		{
			{ Keys.D0, '0' }, { Keys.D1, '1' }, { Keys.D2, '2' }, { Keys.D3, '3' }, { Keys.D4, '4' },
			{ Keys.D5, '5' }, { Keys.D6, '6' }, { Keys.D7, '7' }, { Keys.D8, '8' }, { Keys.D9, '9' },
			{ Keys.OemPlus, '=' }, { Keys.OemMinus, '-' }, { Keys.OemTilde, '`' }, 
			{ Keys.OemOpenBrackets, '[' }, { Keys.OemCloseBrackets, ']' }, { Keys.OemQuotes, '\'' },
			{ Keys.OemPipe, '\\' }, { Keys.OemSemicolon, ';' }, { Keys.OemQuestion, '/' },
			{ Keys.OemComma, ',' }, { Keys.OemPeriod, '.' }, { Keys.OemBackslash, '<' }
		};
		protected static Dictionary<Keys, Char> _oemShiftCharKeys = new Dictionary<Keys, char>
		{
			{ Keys.D0, ')' }, { Keys.D1, '!' }, { Keys.D2, '@' }, { Keys.D3, '#' }, { Keys.D4, '$' },
            { Keys.D5, '%' }, { Keys.D6, '^' }, { Keys.D7, '&' }, { Keys.D8, '*' }, { Keys.D9, '(' },
			{ Keys.OemPlus, '+' }, { Keys.OemMinus, '_' }, { Keys.OemTilde, '~' },
			{ Keys.OemOpenBrackets, '{' }, { Keys.OemCloseBrackets, '}' }, { Keys.OemQuotes, '"' },
			{ Keys.OemPipe, '|' }, { Keys.OemSemicolon, ':' }, { Keys.OemQuestion, '?' },
			{ Keys.OemComma, '<' }, { Keys.OemPeriod, '>' }, { Keys.OemBackslash, '>' }
		};

		public void SetBorder(Color color, int size) {
			Color[] pixels = new Color[Bounds.Width * Bounds.Height];
            for (int x = 0; x < Bounds.Width; x++)
            {
                for (int y = 0; y < Bounds.Height; y++)
                {
					pixels[x + (y * Bounds.Width)] = color;
					if (x >= size && y >= size && x < Bounds.Width - size && y < Bounds.Height - size)
                    {
                        pixels[x + (y * Bounds.Width)].A = 0;
                    }
                }
            }
            _borderTexture.SetData(pixels);

			_border = color;
			_borderSize = size;
		}

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
							StateManager stateManager = (StateManager)parameters["stateManager"];
							string thisTaskId = (string)parameters["stateId"];
							List<string> turnUpAfter = (List<string>)parameters["turnUpAfter"];
                            
							GameRogue.LogManager.SendMessage(_buffer.ToString());                     
							_buffer.Clear();

							stateManager.SetStateStatus(thisTaskId, StateStatus.DoNothing);

							foreach (string upId in turnUpAfter) {
								stateManager.SetStateStatus(upId, StateStatus.Update);
							}

							break;
						case Keys.CapsLock:
							_capslock = !_capslock;
							break;
						default:
							bool found = false;
							char character = ' ';
                     
							if (_pureCharKeys.Contains(key))
							{
								found = true;
								if (_capslock)
								{
									if (_keyStateOne.IsKeyDown(Keys.LeftShift) || _keyStateOne.IsKeyDown(Keys.RightShift))
									{
										character = char.ToLower(key.ToString()[0]);
									}
									else
									{
										character = char.ToUpper(key.ToString()[0]);
									}
								}
								else
								{
									if (_keyStateOne.IsKeyDown(Keys.LeftShift) || _keyStateOne.IsKeyDown(Keys.RightShift))
									{
										character = char.ToUpper(key.ToString()[0]);
									}
									else
									{
										character = char.ToLower(key.ToString()[0]);
									}
								}
							}
                                                 
							if (_oemNormalCharKeys.ContainsKey(key)) {
								found = true;
								if (_keyStateOne.IsKeyDown(Keys.LeftShift) || _keyStateOne.IsKeyDown(Keys.RightShift)) {
									character = _oemShiftCharKeys[key];
								}
								else {
									character = _oemNormalCharKeys[key];
								}
							}

							if (_convertingCharKeys.ContainsKey(key))
                            {
                                found = true;
                                character = _convertingCharKeys[key];
                            }

							if (found)
							{
								_buffer.Append(character);
								if (Font.MeasureString(_buffer.ToString()).X >= Bounds.Width)
								{
									if (_buffer.Length > 0)
									{
										_buffer.Remove(_buffer.Length - 1, 1);
									}
									_buffer.Append(Environment.NewLine);
									_buffer.Append(character);
								}
							}

							break;
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters)
		{
			spriteBatch.Draw(_backgroundTexture, Bounds, Color.White);
			spriteBatch.Draw(_borderTexture, Bounds, Color.White);
			spriteBatch.DrawString(Font, _buffer.ToString(), new Vector2(Bounds.X, Bounds.Y), new Color(new Vector4(1, 1, 1, 1f)));
		}
	}
}

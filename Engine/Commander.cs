using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueNeverDie.Engine
{
	public class Commander : IStateUpdate, IStateDraw
    {
		public Commander(SpriteFont Font, float Padding = 16, char Cursor = '|',
            Color FontColor = default(Color), Color BackgroundColor = default(Color), Color FrameColor = default(Color), 
            int FrameWidth = 4, float DrawDepth = 1.0f)
        {
			this.Font = Font;
            this.Padding = Padding;
            this.Cursor = Cursor;
            this.FontColor = FontColor != default(Color) ? FontColor : Color.Green;

            _backgroundPixel = new Texture2D(GameRogue.Graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            this.BackgroundColor = BackgroundColor;
            _framePixel = new Texture2D(GameRogue.Graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            this.FrameColor = FrameColor;

            this.FrameWidth = FrameWidth;

            this.DrawDepth = DrawDepth;

            _buffer = new StringBuilder();
			_keyStateOne = Keyboard.GetState();
			_keyStateTwo = Keyboard.GetState();

			_capslock = false;
        }

		public SpriteFont Font;
        public float Padding;
        public char Cursor;
        public Color FontColor;
        public Color BackgroundColor
        {
            get
            {
                Color[] texData = new Color[_backgroundPixel.Width * _backgroundPixel.Height];
                _backgroundPixel.GetData<Color>(texData);
                return texData[0];
            }
            set => _backgroundPixel.SetData<Color>(new Color[] { value != default(Color) ? value : new Color(Color.White, 0.8f) });
        }
        public Color FrameColor
        {
            get
            {
                Color[] texData = new Color[_framePixel.Width * _framePixel.Height];
                _framePixel.GetData<Color>(texData);
                return texData[0];
            }
            set => _framePixel.SetData<Color>(new Color[] { value != default(Color) ? value : new Color(Color.Green, 1f) });
        }
        public int FrameWidth;
        public float DrawDepth;

        protected Texture2D _backgroundPixel;
        protected Texture2D _framePixel;

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
                            
							((ScreenMessager)parameters["screenMessager"]).SendMessage(_buffer.ToString());                     
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
							}

							break;
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters)
		{
            GameWindow window = (GameWindow)parameters["gameWindow"];

            StringBuilder displayString = new StringBuilder(_buffer.ToString());

            if (gameTime.TotalGameTime.Milliseconds >= 500)
            {
                displayString.Append(Cursor);
            }
            else
            {
                displayString.Append(' ');
            }

            bool wrapPossible = true;
            while (wrapPossible && Font.MeasureString(displayString).X > window.ClientBounds.Width - (Padding * 2))
            {
                StringBuilder partialString = new StringBuilder(displayString.ToString());
                while (wrapPossible && Font.MeasureString(partialString).X > window.ClientBounds.Width - (Padding * 2))
                {
                    if (partialString.Length > 0)
                    {
                        partialString.Length -= 1;
                    }
                    else
                    {
                        wrapPossible = false;
                    }
                }
                if (wrapPossible)
                {
                    displayString.Insert(partialString.Length, Environment.NewLine);
                }
            }

            Vector2 stringSize = Font.MeasureString(displayString);

            spriteBatch.Draw(_backgroundPixel, 
                new Rectangle(
                    (int)Padding, 
                    (int)(window.ClientBounds.Height - stringSize.Y - Padding),
                    (int)(window.ClientBounds.Width - (Padding * 2)), 
                    (int)stringSize.Y), 
                null, Color.White, 0, Vector2.Zero, SpriteEffects.None, DrawDepth - 0.0002f);
            spriteBatch.Draw(_framePixel,
                new Rectangle(
                    (int)(Padding - FrameWidth),
                    (int)(window.ClientBounds.Height - stringSize.Y - Padding - FrameWidth),
                    (int)(window.ClientBounds.Width - (Padding * 2) + (FrameWidth * 2)),
                    FrameWidth),
                null, Color.White, 0, Vector2.Zero, SpriteEffects.None, DrawDepth - 0.0001f);
            spriteBatch.Draw(_framePixel,
                new Rectangle(
                    (int)(Padding - FrameWidth),
                    (int)(window.ClientBounds.Height - stringSize.Y - Padding + stringSize.Y),
                    (int)(window.ClientBounds.Width - (Padding * 2) + (FrameWidth * 2)),
                    FrameWidth),
                null, Color.White, 0, Vector2.Zero, SpriteEffects.None, DrawDepth - 0.0001f);
            spriteBatch.Draw(_framePixel,
                new Rectangle(
                    (int)(Padding - FrameWidth),
                    (int)(window.ClientBounds.Height - stringSize.Y - Padding - FrameWidth),
                    FrameWidth,
                    (int)(stringSize.Y + (FrameWidth * 2))),
                null, Color.White, 0, Vector2.Zero, SpriteEffects.None, DrawDepth - 0.0001f);
            spriteBatch.Draw(_framePixel,
                new Rectangle(
                    (int)(window.ClientBounds.Width - Padding),
                    (int)(window.ClientBounds.Height - stringSize.Y - Padding - FrameWidth),
                    FrameWidth,
                    (int)(stringSize.Y + (FrameWidth * 2))),
                null, Color.White, 0, Vector2.Zero, SpriteEffects.None, DrawDepth - 0.0001f);
            spriteBatch.DrawString(Font, displayString, new Vector2(Padding, window.ClientBounds.Height - stringSize.Y - Padding), 
                FontColor, 0, Vector2.Zero, 1, SpriteEffects.None, DrawDepth);
		}
	}
}

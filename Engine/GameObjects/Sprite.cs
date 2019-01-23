using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RogueNeverDie.Engine.GameObjects
{
    public class Sprite
    {
		public Sprite(Texture2D texture, Rectangle viewRect, int framesTotal = 1, int framesPerLine = 1, double framesPerSecond = 0)
        {
			_texture = texture;
			_viewrect = viewRect;
			_framesTotal = Math.Max(framesTotal, 1);
			_framesPerLine = Math.Max(framesPerLine, 1);
            
			_currentFrameTime = TimeSpan.Zero;
			_currentFrame = 0;
			_frameRect = viewRect;

			if (Math.Max(framesPerSecond, 0) > 0) {
				_millisecondsPerFrame = 1000.0 / framesPerSecond;
			}
			else {
				_millisecondsPerFrame = -1;
			}

			Origin = Vector2.Zero;
			Scale = new Vector2(1, 1);
			Rotation = 0;
        }

		protected Texture2D _texture;
		protected Rectangle _viewrect;
		protected int _framesTotal;
		protected int _framesPerLine;
         
		protected double _millisecondsPerFrame;
		protected TimeSpan _currentFrameTime;
		protected int _currentFrame;
		protected Rectangle _frameRect;

		public Vector2 Origin { get; set; }
		public Vector2 Scale { get; set; }
		public float Rotation { get; set; }

		public void Update(GameTime gameTime) 
		{
			if (_millisecondsPerFrame > 1)
			{
				_currentFrameTime += gameTime.ElapsedGameTime;

				if (_currentFrameTime.TotalMilliseconds >= _millisecondsPerFrame)
				{
					_currentFrameTime = TimeSpan.Zero;

					_currentFrame++;
					if (_currentFrame >= _framesTotal)
					{
						_currentFrame = 0;
					}
                    
					int frameX = _currentFrame % _framesPerLine;
					int frameY = _currentFrame / _framesPerLine;
                    
					_frameRect.X = _viewrect.X + (_viewrect.Width * frameX);
					_frameRect.Y = _viewrect.Y + (_viewrect.Height * frameY);
				}
			}
		}
        
		public void Draw(SpriteBatch spriteBatch, Vector2 position) {
			Draw(spriteBatch, position, Color.White, 0, new Vector2(1, 1));
		}
        
		public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects effect = SpriteEffects.None, float layer = 0.5f)
        {
			spriteBatch.Draw(_texture, position, _frameRect, color, rotation + this.Rotation, this.Origin, scale * this.Scale, effect, layer);
        }
    }
}

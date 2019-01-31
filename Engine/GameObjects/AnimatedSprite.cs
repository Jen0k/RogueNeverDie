using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RogueNeverDie.Engine.GameObjects
{
    public class AnimatedSprite : Sprite, ISprite
    {
        public AnimatedSprite(Texture2D Texture, int FramesTotal, int FramesPerLine, int FramesPerSecond, Rectangle ViewRectangle = default(Rectangle), Vector2 Origin = default(Vector2), float Rotation = 0, Color Color = default(Color), float Scale = 1, float DrawDepth = 0)
            : base(Texture, ViewRectangle, Origin, Rotation, Color, Scale, DrawDepth)
        {
            if (ViewRectangle == default(Rectangle))
            {
                this.ViewRectangle = new Rectangle(
                    Texture.Bounds.X,
                    Texture.Bounds.Y,
                    (int)Math.Round((float)Texture.Bounds.Width / FramesPerLine),
                    (int)Math.Round(Texture.Bounds.Height / Math.Ceiling((float)FramesTotal / FramesPerLine))
                );
            }

            this.FramesTotal = FramesTotal;
            this.FramesPerLine = FramesPerLine;
            this.FramesPerSecond = FramesPerSecond;

            _currentFrame = 0;
        }

        public int FramesTotal;
        public int FramesPerLine;
        public int FramesPerSecond
        {
            get => (int)(1000 / _msecPerFrame);
            set => _msecPerFrame = 1000.0 / value;
        }

        protected int _currentFrame;
        protected double _msecPerFrame;
        protected double _msecAfterLastFrame;

        public void Update(GameTime gameTime)
        {
            _msecAfterLastFrame += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_msecAfterLastFrame >= _msecPerFrame)
            {
                _currentFrame++;
                if (_currentFrame >= FramesTotal)
                {
                    _currentFrame = 0;
                }
                _msecAfterLastFrame = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle currentFrame = new Rectangle(
                ViewRectangle.X + ((_currentFrame % FramesPerLine) * ViewRectangle.Width),
                ViewRectangle.Y + ((_currentFrame / FramesPerLine) * ViewRectangle.Height),
                ViewRectangle.Width,
                ViewRectangle.Height
            );

            spriteBatch.Draw(Texture, position, currentFrame, Color.White, Rotation, Origin, Scale, SpriteEffects.None, DrawDepth);
        }
    }
}

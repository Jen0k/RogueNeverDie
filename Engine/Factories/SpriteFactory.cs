using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueNeverDie.Engine.GameObjects;

namespace RogueNeverDie.Engine.Factories
{
    public class SpriteFactory : IStateUpdate
    {
        public SpriteFactory(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            _animatedSprites = new LinkedList<AnimatedSprite>(); 
        }

        public Sprite Create(string Texture)
        {
            return new Sprite(_resourceManager.Load<Texture2D>(Texture));
        }

        public Sprite Create(string Texture, Rectangle ViewRectangle, float Rotation)
        {
            return new Sprite(_resourceManager.Load<Texture2D>(Texture), ViewRectangle, Vector2.Zero, Rotation);
        }

        public Sprite Create(string Texture, Rectangle ViewRectangle, float Rotation, Color Color)
        {
            return new Sprite(_resourceManager.Load<Texture2D>(Texture), ViewRectangle, Vector2.Zero, Rotation, Color);
        }

        public Sprite CreateAnimated(string Texture, int FramesTotal, int FramesPerLine, int FramesPerSecond)
        {
            AnimatedSprite sprite = new AnimatedSprite(_resourceManager.Load<Texture2D>(Texture), FramesTotal, FramesPerLine, FramesPerSecond);
            _animatedSprites.AddLast(sprite);
            return sprite;
        }

        public Sprite CreateAnimated(string Texture, Rectangle ViewRectangle, float Rotation, int FramesTotal, int FramesPerLine, int FramesPerSecond)
        {
            AnimatedSprite sprite = new AnimatedSprite(_resourceManager.Load<Texture2D>(Texture), FramesTotal, FramesPerLine, FramesPerSecond, ViewRectangle, Vector2.Zero, Rotation);
            _animatedSprites.AddLast(sprite);
            return sprite;
        }

        public Sprite CreateAnimated(string Texture, Rectangle ViewRectangle, float Rotation, Color Color, int FramesTotal, int FramesPerLine, int FramesPerSecond)
        {
            AnimatedSprite sprite = new AnimatedSprite(_resourceManager.Load<Texture2D>(Texture), FramesTotal, FramesPerLine, FramesPerSecond, ViewRectangle, Vector2.Zero, Rotation, Color);
            _animatedSprites.AddLast(sprite);
            return sprite;
        }

        protected ResourceManager _resourceManager;
        protected LinkedList<AnimatedSprite> _animatedSprites;

        public void Update(GameTime gameTime, Dictionary<string, object> parameters)
        {
            foreach(AnimatedSprite sprite in _animatedSprites)
            {
                sprite.Update(gameTime);
            }
        }
    }
}

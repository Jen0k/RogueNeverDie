using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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

        public Sprite CreateSprite(string Texture)
        {
            return default(Sprite);
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

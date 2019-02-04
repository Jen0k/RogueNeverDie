﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RogueNeverDie.Engine.GameObjects
{
    public class SandwichSprite : ISprite
    {
        public SandwichSprite(Vector2 Origin = default(Vector2))
        {
            _spriteSandwich = new ISprite[] { };
            this.Origin = Origin != default(Vector2) ? Origin : Vector2.Zero;
        }

        public ISprite[] _spriteSandwich;
        public Vector2 Origin;

        public void Add(ISprite sprite)
        {
            ISprite[] newSpriteSandwich = new ISprite[_spriteSandwich.Length + 1];

            Array.Copy(_spriteSandwich, newSpriteSandwich, _spriteSandwich.Length);
            newSpriteSandwich[_spriteSandwich.Length] = sprite;

            _spriteSandwich = newSpriteSandwich;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            foreach(ISprite sprite in _spriteSandwich)
            {
                sprite.Draw(spriteBatch, Origin + position);
            }
        }
    }
}

using System;
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

            _drawDepth = 0;
            _nextDrawDepth = _drawDepth + Config.DepthBetweenSpriteLayers;
        }

        public ISprite[] _spriteSandwich;
        public Vector2 Origin;

        protected float _drawDepth;
        protected float _nextDrawDepth;
        public float DrawDepth
        {
            get => _drawDepth;
            set
            {
                _drawDepth = value;
                if (_spriteSandwich.Length > 0)
                {
                    _nextDrawDepth = value;
                    for (int i = 0; i < _spriteSandwich.Length; i++)
                    {
                        _spriteSandwich[i].DrawDepth = _nextDrawDepth;
                        _nextDrawDepth += _spriteSandwich[i].DrawGauge;
                    }
                }
                else
                {
                    _nextDrawDepth = _drawDepth + Config.DepthBetweenSpriteLayers;
                }
            }
        }

        public float DrawGauge
        {
            get => _nextDrawDepth - _drawDepth;
        }

        public void Add(ISprite sprite)
        {
            ISprite[] newSpriteSandwich = new ISprite[_spriteSandwich.Length + 1];

            Array.Copy(_spriteSandwich, newSpriteSandwich, _spriteSandwich.Length);
            newSpriteSandwich[_spriteSandwich.Length] = sprite;
            newSpriteSandwich[_spriteSandwich.Length].DrawDepth = _nextDrawDepth;

            _spriteSandwich = newSpriteSandwich;
            _nextDrawDepth += sprite.DrawGauge; 
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

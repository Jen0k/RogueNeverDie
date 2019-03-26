using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine.GameObjects
{
    public class PhantomSprite : ISprite
    {
        public PhantomSprite(float DrawDepth = 0)
        {
            _drawDepth = DrawDepth;
        }

        public float _drawDepth;

        public float DrawDepth { get => _drawDepth; set { _drawDepth = value; } }
        public float DrawGauge { get => Config.DepthBetweenSpriteLayers; }

        public void Draw(SpriteBatch spriteBatch, Vector2 position) { }
    }
}

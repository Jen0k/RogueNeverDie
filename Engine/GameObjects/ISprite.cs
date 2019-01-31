using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RogueNeverDie.Engine.GameObjects
{
    public interface ISprite
    {
        void Draw(SpriteBatch spriteBatch, Vector2 position);
    }
}

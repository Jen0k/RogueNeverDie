using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine.GameObjects
{
    public class PhantomSprite : ISprite
    {
        public static PhantomSprite Instanse = new PhantomSprite();

        public float DrawDepth { get => 0; set { } }

        public void Draw(SpriteBatch spriteBatch, Vector2 position) { }
    }
}

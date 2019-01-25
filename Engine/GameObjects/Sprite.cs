using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RogueNeverDie.Engine.GameObjects
{
    public class Sprite
    {
		public Sprite(Texture2D Texture, Rectangle ViewRectangle = default(Rectangle), Vector2 Origin = default(Vector2), float Rotation = 0, float Scale = 1, float DrawDepth = 0)
        {
            this.Texture = Texture;
            this.ViewRectangle = ViewRectangle != default(Rectangle) ? ViewRectangle : Texture.Bounds;
            this.Origin = Origin != default(Vector2) ? Origin : Vector2.Zero;
            this.Rotation = Rotation;
            this.Scale = Scale;
            this.DrawDepth = DrawDepth;
        }

        public Texture2D Texture;
        public Rectangle ViewRectangle;
        public Vector2 Origin;
        public float Rotation;
        public float Scale;
        public float DrawDepth;

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, position, ViewRectangle, Color.White, Rotation, Origin, Scale, SpriteEffects.None, DrawDepth);
        }
    }
}

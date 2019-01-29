using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine.GameObjects
{
    public struct TileSpriteSubGroup
    {
        public Sprite[] LeftTop;
        public Sprite[] RightTop;
        public Sprite[] LeftBottom;
        public Sprite[] RightBottom;
    }

    public class Tile
    {
		public Tile(Level parentLevel, Point coordinates)
        {
			_parent = parentLevel;
			_coordinates = coordinates;

            _sprites = new LinkedList<TileSpriteSubGroup>();

			parentLevel.SetTile(this);
        }

        public const int Size = 32;

		protected Level _parent;  
		protected Point _coordinates;

		protected LinkedList<TileSpriteSubGroup> _sprites;
        
		public Point Coordinates { get => _coordinates; }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            foreach(TileSpriteSubGroup spriteGroup in _sprites)
            {
                foreach(Sprite sprite in spriteGroup.LeftTop)
                {
                    sprite.Draw(spriteBatch, position);
                }
                foreach (Sprite sprite in spriteGroup.LeftBottom)
                {
                    sprite.Draw(spriteBatch, new Vector2(position.X, position.Y + Size/2.0f));
                }
                foreach (Sprite sprite in spriteGroup.LeftBottom)
                {
                    sprite.Draw(spriteBatch, new Vector2(position.X + (Size / 2.0f), position.Y));
                }
                foreach (Sprite sprite in spriteGroup.LeftBottom)
                {
                    sprite.Draw(spriteBatch, new Vector2(position.X + (Size / 2.0f), position.Y + (Size / 2.0f)));
                }
            }
        }
    }
}

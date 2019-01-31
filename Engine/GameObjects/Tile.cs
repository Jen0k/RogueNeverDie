using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine.GameObjects
{
    public struct TileSpriteSubGroup
    {
        public ISprite LeftTop;
        public ISprite RightTop;
        public ISprite LeftBottom;
        public ISprite RightBottom;
    }

    public class Tile
    {
		public Tile(Level parentLevel, TileSpriteSubGroup SpriteSubGroup, Point coordinates)
        {
			_parent = parentLevel;
			_coordinates = coordinates;

            _sprites = new TileSpriteSubGroup[] { SpriteSubGroup };

            parentLevel.SetTile(this);
        }

        public const int Size = 32;

		protected Level _parent;  
		protected Point _coordinates;

		protected TileSpriteSubGroup[] _sprites;
        
		public Point Coordinates { get => _coordinates; }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            foreach(TileSpriteSubGroup spriteGroup in _sprites)
            {
                spriteGroup.LeftTop.Draw(spriteBatch, position);
                spriteGroup.LeftBottom.Draw(spriteBatch, new Vector2(position.X, position.Y + Size / 2.0f));
                spriteGroup.RightTop.Draw(spriteBatch, new Vector2(position.X + (Size / 2.0f), position.Y));
                spriteGroup.RightBottom.Draw(spriteBatch, new Vector2(position.X + (Size / 2.0f), position.Y + (Size / 2.0f)));
            }
        }
    }
}

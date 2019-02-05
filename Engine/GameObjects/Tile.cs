using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine.GameObjects
{
    public class Tile
    {
		public Tile(Level parentLevel, ISprite Sprite, Point coordinates)
        {
			_parent = parentLevel;
			_coordinates = coordinates;

            _sprite = Sprite;

            parentLevel.SetTile(this);
        }

		protected Level _parent;  
		protected Point _coordinates;

		protected ISprite _sprite;
        
		public Point Coordinates { get => _coordinates; }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            _sprite.Draw(spriteBatch, position);
        }
    }
}

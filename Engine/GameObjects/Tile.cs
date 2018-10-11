using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine.GameObjects
{
	public class Tile
    {
		public Tile(Level parentLevel, Point coordinates, Sprite backgroundSprite)
        {
			_parent = parentLevel;

			_coordinates = coordinates;

			_sprites = new List<Sprite>(1);         
			_sprites.Add(backgroundSprite);

			_entities = new List<Entity>();
			_creatures = new List<Creature>();

			parentLevel.SetTile(this);
        }

		public const int Size = 32;

		protected Level _parent;  
		protected Point _coordinates;

		protected List<Sprite> _sprites;
		protected List<Entity> _entities;
		protected List<Creature> _creatures;
        
		public Point Coordinates { get => _coordinates; }
		public IReadOnlyList<Sprite> Sprites { get => _sprites.AsReadOnly(); }
	}
}

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine.GameObjects
{
	public class Level : IState
    {
		public Level(Point levelSize)
        {
			_levelSize = levelSize;
			_tiles = new Dictionary<Point, Tile>(_levelSize.X * _levelSize.Y);         

			_entities = new List<Entity>();
			_creatures = new List<Creature>();
        }

		public Vector2 CameraPosition {
			get
			{
				return _cameraPosition;
			}
			set
			{
				_cameraPosition = new Vector2(Math.Min(Math.Max(0, value.X), _levelSize.X), Math.Min(Math.Max(0, value.Y), _levelSize.Y));
			}
		}

		protected Vector2 _cameraPosition;
		protected Point _levelSize;
		protected Dictionary<Point, Tile> _tiles;
		protected List<Entity> _entities;
		protected List<Creature> _creatures;
        
		public void SetTile(Tile tile) {
			if (tile.Coordinates.X < 0 || tile.Coordinates.X >= _levelSize.X || tile.Coordinates.Y < 0 || tile.Coordinates.Y >= _levelSize.Y) {
				throw new IndexOutOfRangeException("Координаты тайла за пределами уровня!");
			}

			_tiles[tile.Coordinates] = tile;
		}

		public bool TileBelongs(Tile tile) {
			return _tiles.ContainsValue(tile);
		}

		public void Update(GameTime gameTime, Dictionary<string, object> parameters)
		{
			//throw new NotImplementedException();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters)
		{
			Point tilesInScreen = 
				new Point((int)Math.Ceiling(GameRogue.Graphics.PreferredBackBufferWidth / (double)Tile.Size), 
				          (int)Math.Ceiling(GameRogue.Graphics.PreferredBackBufferHeight / (double)Tile.Size));
			Point flooredCameraPosition = new Point((int)Math.Floor(_cameraPosition.X), (int)Math.Floor(_cameraPosition.Y));
			for (int x = flooredCameraPosition.X; x <= (tilesInScreen.X + flooredCameraPosition.X); x++) {
				for (int y = flooredCameraPosition.Y; y <= (tilesInScreen.Y + flooredCameraPosition.Y); y++) {
					Point keyPoint = new Point(x, y);
					if (_tiles.ContainsKey(keyPoint))
					{
						foreach (Sprite sprite in _tiles[keyPoint].Sprites)
						{
							sprite.Draw(spriteBatch, new Vector2((x - _cameraPosition.X) * Tile.Size, (y - _cameraPosition.Y) * Tile.Size));
						}
					}
				}
			}
		}
	}
}

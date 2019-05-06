using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueNeverDie.Engine.GameObjects
{
    public class Level : IStateUpdate, IStateDraw
    {
        public Level(Point levelSize, float DrawDepth = 0.5f)
        {
            _drawDepth = DrawDepth;
            _levelSize = levelSize;
            _tileGrid = new Dictionary<Point, Tile>(_levelSize.X * _levelSize.Y);
        }

        public event InformationPanel.ChangeHandler SpriteDrawed;

        public Vector2 CameraPosition {
            get
            {
                return _cameraPosition;
            }
            set
            {
                int halfWidth = (int)(GameRogue.Graphics.PreferredBackBufferWidth / (double)Config.TileSize) / 2;
                int halfHeight = (int)(GameRogue.Graphics.PreferredBackBufferHeight / (double)Config.TileSize) / 2;

                _cameraPosition = new Vector2(Math.Min(Math.Max(-halfWidth, value.X), _levelSize.X - halfWidth), Math.Min(Math.Max(-halfHeight, value.Y), _levelSize.Y - halfHeight));
            }
        }

        protected float _drawDepth;
        public float DrawDepth
        {
            get => _drawDepth;
            set => _drawDepth = value;
        }

        protected Vector2 _cameraPosition;
        protected Point _levelSize;
        protected Dictionary<Point, Tile> _tileGrid;

        public void SetTile(Tile tile) {
            if (tile.Coordinates.X < 0 || tile.Coordinates.X >= _levelSize.X || tile.Coordinates.Y < 0 || tile.Coordinates.Y >= _levelSize.Y) {
                throw new IndexOutOfRangeException("Координаты тайла за пределами уровня!");
            }

            _tileGrid[tile.Coordinates] = tile;
        }

        public bool TileBelongs(Tile tile) {
            return _tileGrid.ContainsValue(tile);
        }

        public IReadOnlyDictionary<Point, Tile> TileGrid { get => _tileGrid; }

        public void Update(GameTime gameTime, Dictionary<string, object> parameters)
		{
			KeyboardState keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Keys.Up))
			{
				CameraPosition = new Vector2(CameraPosition.X, CameraPosition.Y - 1);
			}
			else if (keyboardState.IsKeyDown(Keys.Down))
			{
				CameraPosition = new Vector2(CameraPosition.X, CameraPosition.Y + 1);
			}
			else if (keyboardState.IsKeyDown(Keys.Left))
            {
                CameraPosition = new Vector2(CameraPosition.X - 1, CameraPosition.Y);
            }
			else if (keyboardState.IsKeyDown(Keys.Right))
            {
                CameraPosition = new Vector2(CameraPosition.X + 1, CameraPosition.Y);
            }

			//GameRogue.LogManager.SendMessage(String.Format("X: {0}, Y: {1}", CameraPosition.X, CameraPosition.Y));
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters)
		{
            int spritesDrawed = 0;

			Point tilesInScreen = 
				new Point((int)Math.Ceiling(GameRogue.Graphics.PreferredBackBufferWidth / (double)Config.TileSize), 
				          (int)Math.Ceiling(GameRogue.Graphics.PreferredBackBufferHeight / (double)Config.TileSize));
			Point flooredCameraPosition = new Point((int)Math.Floor(_cameraPosition.X), (int)Math.Floor(_cameraPosition.Y));
			for (int x = flooredCameraPosition.X; x <= (tilesInScreen.X + flooredCameraPosition.X); x++) {
				for (int y = flooredCameraPosition.Y; y <= (tilesInScreen.Y + flooredCameraPosition.Y); y++) {
					Point keyPoint = new Point(x, y);
					if (_tileGrid.ContainsKey(keyPoint))
					{
                        _tileGrid[keyPoint].Draw(spriteBatch, new Vector2((x - _cameraPosition.X) * Config.TileSize, (y - _cameraPosition.Y) * Config.TileSize));
                        spritesDrawed++;
                    }
				}
			}

            SpriteDrawed?.Invoke(spritesDrawed);
        }
    }
}

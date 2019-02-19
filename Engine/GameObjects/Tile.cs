using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine.GameObjects
{
    public class Tile
    {
		public Tile(Level parentLevel, Point coordinates, int layers)
        {
			_parent = parentLevel;
			_coordinates = coordinates;

            _spriteLayers = new ISprite[layers];

            for (int i = 0; i < _spriteLayers.Length; i++)
            {
                _spriteLayers[i] = PhantomSprite.Instanse;
            }

            parentLevel.SetTile(this);
        }

		protected Level _parent;  
		protected Point _coordinates;

		protected ISprite[] _spriteLayers;
        
		public Point Coordinates { get => _coordinates; }

        public void SetLayer(int layer, ISprite sprite)
        {
            if (_spriteLayers.Length <= layer)
            {
                ISprite[] newSpriteLayers = new ISprite[layer + 1];

                Array.Copy(_spriteLayers, newSpriteLayers, _spriteLayers.Length);

                for (int i = _spriteLayers.Length; i < layer; i++)
                {
                    newSpriteLayers[i] = PhantomSprite.Instanse;
                }

                _spriteLayers = newSpriteLayers;
            }

            _spriteLayers[layer] = sprite;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            foreach (ISprite layer in _spriteLayers)
            {
                layer.Draw(spriteBatch, position);
            }
        }
    }
}

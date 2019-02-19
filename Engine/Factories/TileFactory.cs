using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RogueNeverDie.Engine.Common;
using RogueNeverDie.Engine.GameObjects;

namespace RogueNeverDie.Engine.Factories
{
	public class TileFactory
	{
        public TileFactory(ResourceManager ResourceManager, SpriteFactory SpriteFactory)
        {
            _resourceManager = ResourceManager;
            _spriteFactory = SpriteFactory;
        }

        protected ResourceManager _resourceManager;
        protected SpriteFactory _spriteFactory;

        public ISprite CreateLayerEmpty()
        {
            return PhantomSprite.Instanse;
        }

        public ISprite CreateLayer(string atlas, Color Color, float drawDepth)
        {
            TileAtlas tileAtlas = _resourceManager.Load<TileAtlas>(atlas);
            Point fillTexture = tileAtlas.Atlas.GetValue("fill");

            return _spriteFactory.Create(tileAtlas.Texture, new Rectangle(fillTexture.X, fillTexture.Y, Config.TileSize, Config.TileSize), Color, drawDepth);
        }

        public ISprite CreateLayer(string atlas, Color Color, float drawDepth, bool topleft, bool topcenter, bool topright, bool middleleft, bool middlecenter, bool middleright, bool bottomleft, bool bottomcenter, bool bottomright)
        {
            if (topleft && topcenter && topright && middleleft && middlecenter && middleright && bottomleft && bottomcenter && bottomright)
            {
                return CreateLayer(atlas, Color, drawDepth);
            }

            int tileHalfSize = Config.TileSize / 2;

            TileAtlas tileAtlas = _resourceManager.Load<TileAtlas>(atlas);
            Point fillTexture = tileAtlas.Atlas.GetValue("fill");
            Point borderTexture = tileAtlas.Atlas.GetValue("border");

            SandwichSprite newTileLayer = new SandwichSprite();

            newTileLayer.Add(CreateLayerSubtile(tileAtlas.Texture, fillTexture, borderTexture, new Vector2(0, 0), Color, drawDepth, topleft, topcenter, middleleft, middlecenter));
            newTileLayer.Add(CreateLayerSubtile(tileAtlas.Texture, fillTexture + new Point(tileHalfSize, 0), borderTexture, new Vector2(tileHalfSize, 0), Color, drawDepth, topcenter, topright, middlecenter, middleright));
            newTileLayer.Add(CreateLayerSubtile(tileAtlas.Texture, fillTexture + new Point(0, tileHalfSize), borderTexture, new Vector2(0, tileHalfSize), Color, drawDepth, middleleft, middlecenter, bottomleft, bottomcenter));
            newTileLayer.Add(CreateLayerSubtile(tileAtlas.Texture, fillTexture + new Point(tileHalfSize, tileHalfSize), borderTexture, new Vector2(tileHalfSize, tileHalfSize), Color, drawDepth, middlecenter, middleright, bottomcenter, bottomright));

            return newTileLayer;
        }

        public ISprite CreateLayerSubtile(string textureID, Point fillTexture, Point borderTexture, Vector2 Origin, Color Color, float drawDepth, bool topleft, bool topright, bool bottomleft, bool bottomright)
        {
            int tileHalfSize = Config.TileSize / 2;
            int tileQuarterSize = Config.TileSize / 4;
            int tileThreeQuarters = Config.TileSize - tileQuarterSize;

            SandwichSprite subtileSprites = new SandwichSprite(Origin);

            if (topleft)
            {
                subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(fillTexture.X, fillTexture.Y, tileQuarterSize, tileQuarterSize), Color, drawDepth));
            }
            if (topright)
            {
                subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(fillTexture.X + tileQuarterSize, fillTexture.Y, tileQuarterSize, tileQuarterSize), new Vector2(-tileQuarterSize, 0), Color, drawDepth));
            }
            if (bottomleft)
            {
                subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(fillTexture.X, fillTexture.Y + tileQuarterSize, tileQuarterSize, tileQuarterSize), new Vector2(0, -tileQuarterSize), Color, drawDepth));
            }
            if (bottomright)
            {
                subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(fillTexture.X + tileQuarterSize, fillTexture.Y + tileQuarterSize, tileQuarterSize, tileQuarterSize), new Vector2(-tileQuarterSize, -tileQuarterSize), Color, drawDepth));
            }
            if (topleft != topright)
            {
                if (topleft)
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X, borderTexture.Y, tileQuarterSize, Config.TileSize), new Vector2(0, tileThreeQuarters) , MathHelper.PiOver2, Color, drawDepth));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileThreeQuarters, borderTexture.Y, tileQuarterSize, Config.TileSize), new Vector2(tileQuarterSize, tileQuarterSize), -MathHelper.PiOver2, Color, drawDepth));
                }
            }
            if (topleft != bottomleft)
            {
                if (topleft)
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileQuarterSize, borderTexture.Y, tileQuarterSize, tileHalfSize), new Vector2(tileQuarterSize, tileThreeQuarters), MathHelper.Pi, Color, drawDepth));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileHalfSize, borderTexture.Y, tileQuarterSize, tileHalfSize), new Vector2(0, tileQuarterSize), Color, drawDepth));
                }
            }
            if (bottomleft != bottomright)
            {
                if (bottomleft)
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X, borderTexture.Y, tileQuarterSize, Config.TileSize), new Vector2(-tileQuarterSize, tileThreeQuarters), MathHelper.PiOver2, Color, drawDepth));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileThreeQuarters, borderTexture.Y, tileQuarterSize, Config.TileSize), new Vector2(tileHalfSize, tileQuarterSize), -MathHelper.PiOver2, Color, drawDepth));
                }
            }
            if (bottomright != topright)
            {
                if (topright)
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileQuarterSize, borderTexture.Y, tileQuarterSize, tileHalfSize), new Vector2(tileHalfSize, Config.TileSize - tileQuarterSize), MathHelper.Pi, Color, drawDepth));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileHalfSize, borderTexture.Y, tileQuarterSize, tileHalfSize), new Vector2(-tileQuarterSize, tileQuarterSize), Color, drawDepth));
                }
            }

            return subtileSprites;
        }
    }
}

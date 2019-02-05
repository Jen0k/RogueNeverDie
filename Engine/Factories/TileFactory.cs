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

        public ISprite Create(string atlas, bool topleft, bool topcenter, bool topright, bool middleleft, bool middlecenter, bool middleright, bool bottomleft, bool bottomcenter, bool bottomright)
        {
            TileAtlas tileAtlas = _resourceManager.Load<TileAtlas>(atlas);
            Point fillTexture = tileAtlas.Atlas.GetValue("fill");
            Point borderTexture = tileAtlas.Atlas.GetValue("border");

            SandwichSprite newTileLayer = new SandwichSprite();

            newTileLayer.Add(CreateSubtile(tileAtlas.Texture, fillTexture, borderTexture, new Vector2(0, 0), topleft, topcenter, middleleft, middlecenter));
            newTileLayer.Add(CreateSubtile(tileAtlas.Texture, fillTexture + new Point(16, 0), borderTexture, new Vector2(16, 0), topcenter, topright, middlecenter, middleright));
            newTileLayer.Add(CreateSubtile(tileAtlas.Texture, fillTexture + new Point(0, 16), borderTexture, new Vector2(0, 16), middleleft, middlecenter, bottomleft, bottomcenter));
            newTileLayer.Add(CreateSubtile(tileAtlas.Texture, fillTexture + new Point(16, 16), borderTexture, new Vector2(16, 16), middlecenter, middleright, bottomcenter, bottomright));

            return newTileLayer;
        }

        public ISprite CreateSubtile(string textureID, Point fillTexture, Point borderTexture, Vector2 Origin, bool topleft, bool topright, bool bottomleft, bool bottomright)
        {
            int tileHalfSize = Config.TileSize / 2;
            int tileQuarterSize = Config.TileSize / 4;
            int tileThreeQuarters = Config.TileSize - tileQuarterSize;

            SandwichSprite subtileSprites = new SandwichSprite(Origin);

            if (topleft)
            {
                subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(fillTexture.X, fillTexture.Y, tileQuarterSize, tileQuarterSize)));
            }
            if (topright)
            {
                subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(fillTexture.X + tileQuarterSize, fillTexture.Y, tileQuarterSize, tileQuarterSize), new Vector2(-tileQuarterSize, 0)));
            }
            if (bottomleft)
            {
                subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(fillTexture.X, fillTexture.Y + tileQuarterSize, tileQuarterSize, tileQuarterSize), new Vector2(0, -tileQuarterSize)));
            }
            if (bottomright)
            {
                subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(fillTexture.X + tileQuarterSize, fillTexture.Y + tileQuarterSize, tileQuarterSize, tileQuarterSize), new Vector2(-tileQuarterSize, -tileQuarterSize)));
            }
            if (topleft != topright)
            {
                if (topleft)
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X, borderTexture.Y, tileQuarterSize, Config.TileSize), new Vector2(0, tileThreeQuarters) , MathHelper.PiOver2));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileThreeQuarters, borderTexture.Y, tileQuarterSize, Config.TileSize), new Vector2(tileQuarterSize, tileQuarterSize), -MathHelper.PiOver2));
                }
            }
            if (topleft != bottomleft)
            {
                if (topleft)
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileQuarterSize, borderTexture.Y, tileQuarterSize, tileHalfSize), new Vector2(tileQuarterSize, tileThreeQuarters), MathHelper.Pi));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileHalfSize, borderTexture.Y, tileQuarterSize, tileHalfSize), new Vector2(0, tileQuarterSize)));
                }
            }
            if (bottomleft != bottomright)
            {
                if (bottomleft)
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X, borderTexture.Y, tileQuarterSize, Config.TileSize), new Vector2(-tileQuarterSize, tileThreeQuarters), MathHelper.PiOver2));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileThreeQuarters, borderTexture.Y, tileQuarterSize, Config.TileSize), new Vector2(tileHalfSize, tileQuarterSize), -MathHelper.PiOver2));
                }
            }
            if (bottomright != topright)
            {
                if (topright)
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileQuarterSize, borderTexture.Y, tileQuarterSize, tileHalfSize), new Vector2(tileHalfSize, Config.TileSize - tileQuarterSize), MathHelper.Pi));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(textureID, new Rectangle(borderTexture.X + tileHalfSize, borderTexture.Y, tileQuarterSize, tileHalfSize), new Vector2(-tileQuarterSize, tileQuarterSize)));
                }
            }

            return subtileSprites;
        }
    }
}

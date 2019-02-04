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

        public ISprite CreateSubtile(Vector2 Origin, string fillTexture, string borderTexture, int X, int Y, int Xb, int Yb, bool topleft, bool topright, bool bottomleft, bool bottomright)
        {
            int tileHalfSize = Tile.Size / 2;
            int tileQuarterSize = Tile.Size / 4;
            int tileThreeQuarters = Tile.Size - tileQuarterSize;

            SandwichSprite subtileSprites = new SandwichSprite(Origin);

            if (topleft)
            {
                subtileSprites.Add(_spriteFactory.Create(fillTexture, new Rectangle(X, Y, tileQuarterSize, tileQuarterSize)));
            }
            if (topright)
            {
                subtileSprites.Add(_spriteFactory.Create(fillTexture, new Rectangle(X + tileQuarterSize, Y, tileQuarterSize, tileQuarterSize), new Vector2(-tileQuarterSize, 0)));
            }
            if (bottomleft)
            {
                subtileSprites.Add(_spriteFactory.Create(fillTexture, new Rectangle(X, Y + tileQuarterSize, tileQuarterSize, tileQuarterSize), new Vector2(0, -tileQuarterSize)));
            }
            if (bottomright)
            {
                subtileSprites.Add(_spriteFactory.Create(fillTexture, new Rectangle(X + tileQuarterSize, Y + tileQuarterSize, tileQuarterSize, tileQuarterSize), new Vector2(-tileQuarterSize, -tileQuarterSize)));
            }
            if (topleft != topright)
            {
                if (topleft)
                {
                    subtileSprites.Add(_spriteFactory.Create(borderTexture, new Rectangle(Xb, Yb, tileQuarterSize, Tile.Size), new Vector2(0, tileThreeQuarters) , MathHelper.PiOver2));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(borderTexture, new Rectangle(Xb + tileThreeQuarters, Yb, tileQuarterSize, Tile.Size), new Vector2(tileQuarterSize, tileQuarterSize), -MathHelper.PiOver2));
                }
            }
            if (topleft != bottomleft)
            {
                if (topleft)
                {
                    subtileSprites.Add(_spriteFactory.Create(borderTexture, new Rectangle(Xb + tileQuarterSize, Yb, tileQuarterSize, tileHalfSize), new Vector2(tileQuarterSize, tileThreeQuarters), MathHelper.Pi));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(borderTexture, new Rectangle(Xb + tileHalfSize, Yb, tileQuarterSize, tileHalfSize), new Vector2(0, tileQuarterSize)));
                }
            }
            if (bottomleft != bottomright)
            {
                if (bottomleft)
                {
                    subtileSprites.Add(_spriteFactory.Create(borderTexture, new Rectangle(Xb, Yb, tileQuarterSize, Tile.Size), new Vector2(-tileQuarterSize, tileThreeQuarters), MathHelper.PiOver2));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(borderTexture, new Rectangle(Xb + tileThreeQuarters, Yb, tileQuarterSize, Tile.Size), new Vector2(tileHalfSize, tileQuarterSize), -MathHelper.PiOver2));
                }
            }
            if (bottomright != topright)
            {
                if (topright)
                {
                    subtileSprites.Add(_spriteFactory.Create(borderTexture, new Rectangle(Xb + tileQuarterSize, Yb, tileQuarterSize, tileHalfSize), new Vector2(tileHalfSize, Tile.Size - tileQuarterSize), MathHelper.Pi));
                }
                else
                {
                    subtileSprites.Add(_spriteFactory.Create(borderTexture, new Rectangle(Xb + tileHalfSize, Yb, tileQuarterSize, tileHalfSize), new Vector2(-tileQuarterSize, tileQuarterSize)));
                }
            }

            return subtileSprites;
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RogueNeverDie.Engine.Common;
using RogueNeverDie.Engine.GameObjects;

namespace RogueNeverDie.Engine.Factories
{
	public class TileFactory
	{
        public TileFactory()
        {

        }

        ResourceManager _resourceManager;
        SpriteFactory _spriteFactory;

        public ISprite CreateSubtile(bool topleft, bool topright, bool bottomleft, bool bottomright)
        {
            string dammyName = String.Empty;
            int dammyX = 0;
            int dammyY = 0;

            SandwichSprite subtileSprites = new SandwichSprite(default(Sprite));

            if (topleft)
            {
                Sprite topLeft = _spriteFactory.Create(dammyName, new Rectangle(dammyX, dammyY, Tile.Size / 2, Tile.Size / 2), 0);
            }
            if (topright)
            {
                Sprite topRight = _spriteFactory.Create(dammyName, new Rectangle(dammyX + (Tile.Size / 2), dammyY, Tile.Size / 2, Tile.Size / 2), 0);
            }
            if (bottomleft)
            {
                Sprite bottomLeft = _spriteFactory.Create(dammyName, new Rectangle(dammyX, dammyY + (Tile.Size / 2), Tile.Size / 2, Tile.Size / 2), 0);
            }
            if (bottomright)
            {
                Sprite bottomLeft = _spriteFactory.Create(dammyName, new Rectangle(dammyX + (Tile.Size / 2), dammyY + (Tile.Size / 2), Tile.Size / 2, Tile.Size / 2), 0);
            }

            return default(ISprite);
        }
    }
}

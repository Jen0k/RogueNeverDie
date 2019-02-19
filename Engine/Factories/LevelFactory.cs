using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RogueNeverDie.Engine.GameObjects;

namespace RogueNeverDie.Engine.Factories
{
    public class LevelFactory
    {
        public LevelFactory(ResourceManager ResourceManager, TileFactory TileFactory)
        {
            _resourceManager = ResourceManager;
            _tileFactory = TileFactory;
        }

        protected ResourceManager _resourceManager;
        protected TileFactory _tileFactory;

        protected void MakeRoom(Point size, int wallWidth, bool outerBorder)
        {

        }

        protected void FillRectangle(Level level, string atlas, Rectangle field, int layer)
        {

        }
    }
}

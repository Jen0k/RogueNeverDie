using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RogueNeverDie.Engine.Factories
{
    public class LevelAtlas
    {
        public LevelAtlas(Point Size, string GenerationType)
        {
            this.Size = Size;
            this.GenerationType = GenerationType;

            GenerationParams = new Dictionary<string, string>();
            TileAtlases = new Dictionary<int, string>();
        }

        public Point Size;
        public string GenerationType;
        public Dictionary<string, string> GenerationParams;
        public Dictionary<int, string> TileAtlases;
    }
}

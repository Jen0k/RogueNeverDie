using System;
using Microsoft.Xna.Framework;
using RogueNeverDie.Engine.Common;

namespace RogueNeverDie.Engine.Factories
{
    public class TileAtlas
    {
        public TileAtlas(string Texture, Color Color)
        {
            this.Texture = Texture;
            this.Color = Color;

            Atlas = new WeightSet<string, Point>();
        }

        public string Texture;
        public Color Color;
        public WeightSet<string, Point> Atlas;
    }
}

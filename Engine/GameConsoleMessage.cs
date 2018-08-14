using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine
{
    public struct GameConsoleMessage
    {
		public string Text;
		public SpriteFont Font;
		public Color Color;
		public DateTime DateCreated { get; }
		public TimeSpan LifeTimeTotal;
		public TimeSpan ElapsedLifeTime;
    }
}

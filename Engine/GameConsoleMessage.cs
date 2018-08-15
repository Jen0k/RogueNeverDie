using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine
{
    public struct GameConsoleMessage
    {
		public GameConsoleMessage(string Text, SpriteFont Font, Color Color, DateTime DateCreated, TimeSpan LifeTimeTotal) {
			this.Text = Text;
			this.Font = Font;
			this.Color = Color;
			this.DateCreated = DateCreated;
			this.LifeTimeTotal = LifeTimeTotal;
		}

		public string Text;
		public SpriteFont Font;
		public Color Color;
		public DateTime DateCreated { get; }
		public TimeSpan LifeTimeTotal;
    }
}

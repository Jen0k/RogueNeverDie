using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine
{
    public interface IState
    {
		void Update(GameTime gameTime, Dictionary<string, object> parameters);
		void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters);
    }
}

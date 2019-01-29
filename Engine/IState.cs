using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine
{
    public interface IStateDraw
    {
		void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters);
    }

    public interface IStateUpdate
    {
        void Update(GameTime gameTime, Dictionary<string, object> parameters);
    }
}

﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RogueNeverDie.Engine.GameObjects
{
    public interface ISprite
    {
        float DrawDepth { get; set; }
        void Draw(SpriteBatch spriteBatch, Vector2 position);
    }
}

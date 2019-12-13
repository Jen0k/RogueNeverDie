using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine
{
    public class Widget : IStateUpdate, IStateDraw
    {
        public Widget()
        {
            neighbours = new Widget[] { null, null, null, null };
            fixedSides = new bool[] { false, false, false, false };
            margins = new int[] { 0, 0, 0, 0 };
        }

        public Widget parent;
        public Widget[] neighbours;
        public bool[] fixedSides;
        public int[] margins;

        public Point baseSize;
        public Rectangle drawBounds;

        public void Update(GameTime gameTime, Dictionary<string, object> parameters)
        {
            GameWindow window = (GameWindow)parameters["gameWindow"];

            Rectangle parentBounds = window.ClientBounds;
            if (parent != null)
            {
                parentBounds = parent.drawBounds; 
            }

            // Вычисление координат в окне
            if (fixedSides[0])
            {
                drawBounds.X = parentBounds.X + margins[0];
            }
            else if (fixedSides[2])
            {
                drawBounds.X = parentBounds.Right - margins[2] - baseSize.X;
            }
            else
            {
                drawBounds.X = parentBounds.X;
            }

            if (fixedSides[1])
            {
                drawBounds.Y = parentBounds.Y + margins[1];
            }
            else if (fixedSides[3])
            {
                drawBounds.Y = parentBounds.Bottom - margins[3] - baseSize.Y;
            }
            else
            {
                drawBounds.Y = parentBounds.Y; 
            }

            // Вычисление размеров

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }

    public class WidgetManager : IStateUpdate, IStateDraw
    {
        public WidgetManager()
        {
            widgetStorage = new LinkedList<Widget>();
        }

        protected LinkedList<Widget> widgetStorage;

        public void Update(GameTime gameTime, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}

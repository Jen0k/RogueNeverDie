using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine
{
    public class InformationPanel : IStateDraw
    {
        public InformationPanel(SpriteFont Font, float Padding = 16, float DrawDepth = 0.9f)
        {
            this.Font = Font;
            this.FontColor = Color.Blue;
            this.DrawDepth = DrawDepth;

            _indicatorNames = new Dictionary<ChangeHandler, string>();
            _indicatorValues = new Dictionary<ChangeHandler, object>();
        }

        public delegate void ChangeHandler(object value);

        public SpriteFont Font;
        public float Padding;
        public Color FontColor;
        public float DrawDepth;

        protected Dictionary<ChangeHandler, string> _indicatorNames;
        protected Dictionary<ChangeHandler, object> _indicatorValues;

        public ChangeHandler CreateIndicator(string name)
        {
            if (_indicatorNames.ContainsValue(name))
            {
                throw new Exception("Параметр с таким именем уже наблюдается!");
            }

            ChangeHandler handler = null;
            handler = delegate (object value)
            {
                _indicatorValues[handler] = value;
            };

            _indicatorValues.Add(handler, new object());
            _indicatorNames.Add(handler, name);

            return handler;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters)
        {
            int lineNumber = 0;
            foreach (KeyValuePair<ChangeHandler, string> indicator in _indicatorNames)
            {
                string line = String.Format("{0}: {1}", indicator.Value, _indicatorValues[indicator.Key].ToString());
                Vector2 stringSize = Font.MeasureString(line);

                spriteBatch.DrawString(Font, line, new Vector2(Padding, (stringSize.Y * lineNumber) + Padding),
                    FontColor, 0, Vector2.Zero, 1, SpriteEffects.None, DrawDepth);
                lineNumber++;
            }
        }
    }
}

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

            _indicatorHandlers = new Dictionary<string, ChangeHandler>();
            _indicatorValues = new Dictionary<ChangeHandler, object>();
        }

        public delegate void ChangeHandler(object value);

        public SpriteFont Font;
        public float Padding;
        public Color FontColor;
        public float DrawDepth;

        protected Dictionary<string, ChangeHandler> _indicatorHandlers;
        protected Dictionary<ChangeHandler, object> _indicatorValues;

        public ChangeHandler CreateIndicator(string name)
        {
            if (_indicatorHandlers.ContainsKey(name))
            {
                throw new Exception("Индикатор с таким именем уже создан!");
            }

            ChangeHandler handler = null;
            handler = delegate (object value)
            {
                if (!_indicatorValues.ContainsKey(handler))
                {
                    throw new Exception("Некорректно удалён индикатор! Удаление требуется совмещать с отпиской от наблюдаймого эвента!");
                }
                _indicatorValues[handler] = value;
            };

            _indicatorHandlers.Add(name, handler);
            _indicatorValues.Add(handler, String.Empty);

            return handler;
        }

        public ChangeHandler DeleteIndicator(string name)
        {
            if (!_indicatorHandlers.ContainsKey(name))
            {
                throw new Exception("Индикатор с таким именем не сущетсвует!");
            }

            ChangeHandler handler = _indicatorHandlers[name];

            _indicatorHandlers.Remove(name);
            _indicatorValues.Remove(handler);

            return handler;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Dictionary<string, object> parameters)
        {
            int lineNumber = 0;
            foreach (KeyValuePair<string, ChangeHandler> indicator in _indicatorHandlers)
            {
                string line = String.Format("{0}: {1}", indicator.Key, _indicatorValues[indicator.Value]);
                Vector2 stringSize = Font.MeasureString(line);

                spriteBatch.DrawString(Font, line, new Vector2(Padding, (stringSize.Y * lineNumber) + Padding),
                    FontColor, 0, Vector2.Zero, 1, SpriteEffects.None, DrawDepth);
                lineNumber++;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RogueNeverDie.Engine
{
    public class FPSCounter : IStateUpdate
    {
        public FPSCounter()
        {
            _timeCounter = TimeSpan.Zero;
            _framesCounter = 0;
        }

        public event InformationPanel.ChangeHandler FramesPerSecond;

        protected TimeSpan _timeCounter;
        protected int _framesCounter;

        public void Update(GameTime gameTime, Dictionary<string, object> parameters)
        {
            _timeCounter += gameTime.ElapsedGameTime;
            _framesCounter++;
            if (_timeCounter.TotalMilliseconds >= 1000)
            {
                FramesPerSecond?.Invoke(_framesCounter);

                _timeCounter = TimeSpan.Zero;
                _framesCounter = 0;
            }
        }
    }
}

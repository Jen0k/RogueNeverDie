using System;
using System.Collections.Generic;

namespace RogueNeverDie.Engine
{
    public class GameConsole
    {
        public GameConsole()
        {
			_messageList = new LinkedList<GameConsoleMessage>();
        }

		protected LinkedList<GameConsoleMessage> _messageList;
    }
}

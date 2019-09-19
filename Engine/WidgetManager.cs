using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RogueNeverDie.Engine
{
    public enum WidgetSizeType { Fixed = 0, Dynamic = 1 }
    public enum WidgetPositionType { Fixed = 0, Dynamic = 1 }

    public class Widget
    {
        public WidgetSizeType sizeType;
        public WidgetPositionType positionType;

        public Point basePosition;
    }

    public class WidgetManager
    {
        public WidgetManager()
        {
            widgetStorage = new LinkedList<Widget>();
        }

        protected LinkedList<Widget> widgetStorage;
    }
}

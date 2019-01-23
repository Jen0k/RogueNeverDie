using System;
using System.Collections.Generic;
using RogueNeverDie.Engine.Common;
using RogueNeverDie.Engine.GameObjects;

namespace RogueNeverDie.Engine.Factories
{
	public class TileFactory
	{
		public TileFactory()
		{
			_tileSet = new WeightSet<string, Sprite>();
		}

		protected WeightSet<string, Sprite> _tileSet;

		public enum PartTypes
		{
			Filling = 0,
			BorderShort = 1,
			BorderLong = 2,
		};

		public enum SideTypes
		{
			FillAll = 0,
			HorizontalBorder = 1,
            VerticalBorder = 2,
            InnerAngle = 3,
            OuterAngle = 4
		}

		public struct Form
		{
			public SideTypes LeftTop;
			public SideTypes RightTop;
			public SideTypes LeftBottom;
			public SideTypes RightBottom;
		}
        
		protected PartTypes _stringToTilePart(string type) {
			PartTypes detectedType;

            if (!Enum.TryParse(type, out detectedType))
            {
                throw new ArgumentException(
                    //String.Format("Аргумент {0} не является действительным {1)!", type, typeof(detectedType).Name)
                );
            }

			return detectedType;
		}
        
		public void AddTilePartPattern(PartTypes type, Sprite sprite, int rate = 100) {
			_tileSet.Add(Enum.GetName(type.GetType(), type), sprite, rate);
		}

		public void AddTilePartPattern(string type, Sprite sprite, int rate = 100)
        {
			AddTilePartPattern(_stringToTilePart(type), sprite, rate);
        }
        
		//public Tile Build(Form form, string texMain, string texBackground) {
			
		//}
    }
}

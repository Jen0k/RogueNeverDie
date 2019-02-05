using System;
namespace RogueNeverDie
{
    public class Config
    {
        protected Config() { }

		public const string CommonFont = "Fonts/Common";

        public const int TileSize = 32;

		public const string ContentDirectory = "Content";
		public const string ResoursesRootIndex = "Index.json";

		public static int ScreenWight = 1024;
		public static int ScreenHeight = 768;
    }
}

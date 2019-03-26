using System;
namespace RogueNeverDie
{
    public class Config
    {
        protected Config() { }

		public const string CommonFont = "Fonts/Common";

        public const int TileSize = 32;

        public const float DepthBetweenSpriteLayers = 0.0000001f;
        public const float DepthBetweenTileLayers = 0.0001f;

		public const string ContentDirectory = "Content";
		public const string ResoursesRootIndex = "Index.json";

		public static int ScreenWight = 1024;
		public static int ScreenHeight = 768;
    }
}

using System;
namespace RogueNeverDie.Engine.Common
{
	public class RandomSingle
	{
		protected RandomSingle() { }

		protected static Random _random;

		public static Random Instanse
		{
			get 
			{
				if (_random == null)
                {
					_random = new Random((int)DateTime.Now.Ticks);
                }
				return _random;
			}
		}
    }
}

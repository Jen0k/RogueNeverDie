using System;
namespace RogueNeverDie.Engine.Common
{
	public struct WeightSetElement<T>
    {
        public WeightSetElement(T value, int weight)
        {
			this.value = value;
			this.weight = weight;
        }
        
		public T value;
		public int weight;
    }
}

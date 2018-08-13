using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueNeverDie.Engine.Common
{
	public class WeightSet<K, T>
    {
        public WeightSet()
        {
			_storage = new Dictionary<K, List<WeightSetElement<T>>>();
        }

		protected Dictionary<K, List<WeightSetElement<T>>> _storage;

		public void Add(K key, T value, int weight) {
			if (!_storage.ContainsKey(key)) {
				_storage[key] = new List<WeightSetElement<T>>();
			}

			_storage[key].Add(new WeightSetElement<T>(value, weight));
		}

		public T GetValue(K key) {
			if (_storage.ContainsKey(key)) {
				int summ = _storage[key].Select(sim => sim.weight).Sum();
				int counter = 0;
				int marker = RandomSingle.Instanse.Next(counter, summ);

				foreach (WeightSetElement<T> element in _storage[key]) {
					counter += element.weight;
					if (counter >= marker) {
						return element.value;
					}
				}

				throw new ArgumentOutOfRangeException();
			}
			else {
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}

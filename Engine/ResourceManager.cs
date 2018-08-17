using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueNeverDie.Engine
{
	public class ResourceManager
	{
		public ResourceManager()
		{
			_storage = new Dictionary<Type, Dictionary<string, object>>();
		}

		protected Dictionary<Type, Dictionary<string, object>> _storage;
        
		public T Load<T>(string key)
		{
			Type type = typeof(T);

			if (_storage.ContainsKey(type) && _storage[type].ContainsKey(key))
			{
				return (T)_storage[type][key];
			}

			throw new NullReferenceException(String.Format("Ресурс {0} с идентификатором {1} отсутсвует в базе!", type.Name, key));         
		}

		public void Store(string key, object resourse) {
			Type type = resourse.GetType();

			if (!_storage.ContainsKey(type)) {
				_storage[type] = new Dictionary<string, object>();
			}

			_storage[type].Add(key, resourse);
		}
    }
}

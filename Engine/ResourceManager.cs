using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RogueNeverDie.Engine.Factories;

namespace RogueNeverDie.Engine
{
    public class ResourceManager
	{
		public ResourceManager()
		{
			_storage = new Dictionary<Type, Dictionary<string, object>>();
		}

		protected Dictionary<Type, Dictionary<string, object>> _storage;
        protected Dictionary<string, Type> typeNameToType = new Dictionary<string, Type>() {
            { "Fonts", typeof(SpriteFont) },
            { "Textures", typeof(Texture2D) }
        };

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

        public void LoadFromConfig(string contentDirectory, string indexFile, ContentManager contentManager)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(contentDirectory, indexFile)))
            {
                JObject deserializedData = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());

                List<string> mainNodes = deserializedData.Properties().Select(k => k.Name).ToList();

                foreach (string node in mainNodes)
                {
                    switch (node)
                    {
                        case "Index":
                            List<string> indexes = deserializedData["Index"].Children().Values<string>().ToList();

                            foreach (string index in indexes)
                            {
                                LoadFromConfig(contentDirectory, index, contentManager);
                            }

                            break;
                        case "Config":
                            break;
                        case "Fonts":
                        case "Textures":
                        case "TileAtlases":
                        case "LevelAtlases":
                            List<JToken> childs = deserializedData[node].Children().ToList();

                            foreach (JToken child in childs)
                            {
                                if (typeNameToType.ContainsKey(node))
                                {
                                    Store(child["id"].Value<string>(),
                                                        typeof(ContentManager).
                                                            GetMethod("Load").
                                                                MakeGenericMethod(typeNameToType[node]).
                                                                    Invoke(contentManager, new object[] { child["path"].Value<string>() }));
                                    // Это просто _contentManager.Load<T>() с динамически подставляемым Generic типом.
                                }
                                if (node == "TileAtlases")
                                {
                                    TileAtlas tileAtlas = new TileAtlas(child["texture"].Value<string>(), new Color(
                                        child["color"]["R"].Value<int>(),
                                        child["color"]["G"].Value<int>(),
                                        child["color"]["B"].Value<int>()));

                                    foreach (JToken atlasNode in child["atlas"])
                                    {
                                        tileAtlas.Atlas.Add(atlasNode["type"].Value<string>(), new Point(
                                            atlasNode["position"]["X"].Value<int>(),
                                            atlasNode["position"]["Y"].Value<int>()
                                        ), atlasNode["weight"].Value<int>());
                                    }

                                    Store(child["id"].Value<string>(), tileAtlas);
                                }
                                if (node == "LevelAtlases")
                                {
                                    LevelAtlas levelAtlas = new LevelAtlas(
                                        new Point(child["size"]["X"].Value<int>(),
                                                  child["size"]["Y"].Value<int>()),
                                        child["generation"]["type"].Value<string>()
                                    );

                                    foreach (JToken generationParam in child["generation"]["params"])
                                    {
                                        levelAtlas.GenerationParams.Add(generationParam["key"].Value<string>(),
                                                                        generationParam["value"].Value<string>());
                                    }

                                    foreach (JToken tilesetAtlas in child["tileset"])
                                    {
                                        levelAtlas.TileAtlases.Add(tilesetAtlas["layer"].Value<int>(),
                                                                   tilesetAtlas["atlas"].Value<string>());
                                    }

                                    Store(child["id"].Value<string>(), levelAtlas);
                                }
                            }
                            break;
                        default:
                            throw new ArgumentException(String.Format("Некорректный раздел {0} в конфиге!", node));
                    }
                }
            }
        }
    }
}

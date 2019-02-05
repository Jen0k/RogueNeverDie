using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueNeverDie.Engine.Factories;

namespace RogueNeverDie.Engine
{
    public class ResourseLoader
    {      
		public ResourseLoader(string rootPath)
        {
            _rootPath = rootPath;
        }

        protected string _rootPath;

        protected static Dictionary<string, Type> typeNameToType = new Dictionary<string, Type>() {
            { "Fonts", typeof(SpriteFont) },
            { "Textures", typeof(Texture2D) }
        };
        
		public void LoadFromConfig(string pathToFile, ResourceManager resourceManager, ContentManager contentManager) {
			try
			{
				using (StreamReader reader = new StreamReader(Path.Combine(_rootPath, pathToFile)))
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
									LoadFromConfig(index, resourceManager, contentManager);
								}

								break;
							case "Config":
								break;
                            case "Fonts":
							case "Textures":
                            case "TileAtlases":
                                List<JToken> childs = deserializedData[node].Children().ToList();

                                foreach (JToken child in childs)
                                {
                                    if (typeNameToType.ContainsKey(node))
                                    {
                                        resourceManager.Store(child["id"].Value<string>(),
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

                                        foreach(JToken atlasNode in child["atlas"])
                                        {
                                            tileAtlas.Atlas.Add(atlasNode["type"].Value<string>(), new Point(
                                                atlasNode["position"]["X"].Value<int>(),
                                                atlasNode["position"]["Y"].Value<int>()
                                            ), atlasNode["weight"].Value<int>());
                                        }

                                        resourceManager.Store(child["id"].Value<string>(), tileAtlas);
                                    }
                                }
                                break;
                            default:
								GameRogue.LogManager.SendError(String.Format("Некорректный раздел {0} в конфиге!", node));
								break;
						}
					}
				}
			}
			catch (Exception e)
			{
				GameRogue.LogManager.SendError(e.Message);
			}
        }
    }
}

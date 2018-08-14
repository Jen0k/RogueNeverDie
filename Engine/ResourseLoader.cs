using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueNeverDie.Engine
{
    public class ResourseLoader
    {      
        public ResourseLoader(string rootPath, ResourceManager resourceManager, ContentManager contentManager)
        {
            _rootPath = rootPath;
            _resourceManager = resourceManager;
            _contentManager = contentManager;
        }

        protected string _rootPath;
        protected ResourceManager _resourceManager;
        protected ContentManager _contentManager;

        protected static Dictionary<string, Type> typeNameToType = new Dictionary<string, Type>() {
            { "Fonts", typeof(SpriteFont) },
            { "Textures", typeof(Texture2D) }
        };
        
        public void LoadFromConfig(string pathToFile) {         
            using (StreamReader reader = new StreamReader(Path.Combine(_rootPath, pathToFile))) {
                JObject deserializedData = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());

                List<string> mainNodes = deserializedData.Properties().Select(k => k.Name).ToList();

                foreach (string node in mainNodes) {
                    switch (node) {
                        case "Index":
                            List<string> indexes = deserializedData["Index"].Children().Values<string>().ToList();
                            
                            foreach (string index in indexes) {
                                LoadFromConfig(index);
                            }
                            
                            break;
                            
                        case "Fonts":
                        case "Textures":
                            List<JToken> childs = deserializedData[node].Children().ToList();
                     
                            foreach (JToken child in childs)
                            {
                                _resourceManager.Store(child["id"].Value<string>(), 
                                                        typeof(ContentManager).
                                                            GetMethod("Load").
                                                                MakeGenericMethod(typeNameToType[node]).
                                                                    Invoke(_contentManager, new object[] { child["path"].Value<string>() }));
                                                       // Это просто _contentManager.Load<T>() с динамически подставляемым Generic типом.
                            }

                            break;
                        default:
                            break;
                    }
                }
            }         
        }
    }
}

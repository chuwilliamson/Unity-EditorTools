using ChuTools.NodeEditor.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ChuTools.NodeEditor.View
{
    public partial class NodeEditor
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ObjectCreationHandling = ObjectCreationHandling.Reuse,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            Formatting = Formatting.Indented,
            DefaultValueHandling = DefaultValueHandling.Populate,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        private void Save()
        {
            var n = new SaveLoad { Nodes = Nodes, Connections = Connections };

            var json = JsonConvert.SerializeObject(n, _settings);
            File.WriteAllText(_path, json);
        }

        private void Load()
        {
            InitializeComponents();
            var json = File.ReadAllText(_path);
            var n = JsonConvert.DeserializeObject<SaveLoad>(json, _settings);
            Nodes = n.Nodes;
            Connections = n.Connections;
        }

        [Serializable]
        public class SaveLoad //just for saving
        {
            public List<IDrawable> Nodes { get; set; }
            public List<IDrawable> Connections { get; set; }
        }
    }
}
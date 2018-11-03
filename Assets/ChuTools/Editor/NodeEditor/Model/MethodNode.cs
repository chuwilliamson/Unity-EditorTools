using ChuTools.NodeEditor.Interfaces;
using Newtonsoft.Json;
using System;

namespace ChuTools.NodeEditor.Model
{
    [Serializable]
    public class MethodNode : INode
    {
        [JsonConstructor]
        public MethodNode(MethodObject methodObject)
        {
            Value = methodObject;
        }

        public object Value { get; set; }
    }
}
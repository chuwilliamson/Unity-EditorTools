using System;
using Interfaces;
using Newtonsoft.Json;

namespace JeremyTools
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
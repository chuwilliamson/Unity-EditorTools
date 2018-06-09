using Interfaces;
using Newtonsoft.Json;
using System;

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
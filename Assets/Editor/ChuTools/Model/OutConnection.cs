using System;
using Interfaces;
using Newtonsoft.Json;

namespace ChuTools.Model
{
    [Serializable]
    public class OutConnection : IConnectionOut
    {
        [JsonConstructor]
        public OutConnection(INode inputNode)
        {
            Node = inputNode;
        }

        public object Value => Node?.Value ?? 0;
        public INode Node { get; set; }
    }
}
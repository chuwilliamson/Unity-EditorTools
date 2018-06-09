using System;
using Interfaces;
using Newtonsoft.Json;

namespace ChuTools.Model
{
    [Serializable]
    public class InConnection : IConnectionIn
    {
        [JsonConstructor]
        public InConnection(IConnectionOut outConnection)
        {
            Out = outConnection;
        }

        public IConnectionOut Out { get; set; }
        public object Value => Out?.Value;
    }
}
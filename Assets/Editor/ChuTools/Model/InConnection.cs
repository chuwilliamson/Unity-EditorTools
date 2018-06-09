using Interfaces;
using Newtonsoft.Json;
using System;

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
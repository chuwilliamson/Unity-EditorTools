using Interfaces;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ChuTools.Model
{
    [Serializable]
    public class DisplayNode : INode
    {
        [JsonConstructor]
        public DisplayNode(IConnectionIn inConnection)
        {
            InConnection = inConnection;
        }

        public object Value
        {
            get { return InConnection?.Value ?? 0; }
            set { Debug.LogWarning("no you shouldn't be setting the inconnection value through the node" + value); }
        }

        public IConnectionIn InConnection { get; set; }
    }
}
using System;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace JeremyTools
{
    [Serializable]
    public class DelegateNode : INode
    {
        [JsonConstructor]
        public DelegateNode(IConnectionIn inConnection)
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
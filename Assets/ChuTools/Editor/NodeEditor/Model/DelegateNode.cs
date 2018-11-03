using ChuTools.NodeEditor.Interfaces;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ChuTools.NodeEditor.Model
{
    [Serializable]
    public class DelegateNode : INode
    {
        [JsonConstructor]
        public DelegateNode(IConnectionIn inConnection)
        {
            InConnection = inConnection;
        }

        public IConnectionIn InConnection { get; set; }

        public object Value
        {
            get { return InConnection?.Value ?? 0; }
            set { Debug.LogWarning("no you shouldn't be setting the inconnection value through the node" + value); }
        }
    }
}
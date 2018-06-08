using System;
using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class DisplayNode : INode
    {
        public IConnectionIn InConnection { get; set; }

        public DisplayNode(IConnectionIn inConnection)
        {
            InConnection = inConnection;
        }

        public object Value
        {
            get { return InConnection?.Value ?? 0; }
            set { Debug.LogWarning("no you shouldn't be setting the inconnection value through the node" + value); }
        }
    }
}
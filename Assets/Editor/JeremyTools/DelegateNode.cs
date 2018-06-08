using Interfaces;
using UnityEngine;

namespace JeremyTools
{
    public class DelegateNode : INode
    {
        public IConnectionIn InConnection { get; set; }

        public DelegateNode(IConnectionIn inConnection)
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
using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [System.Serializable]
    public class DisplayNode : INode
    {
        private readonly IConnectionIn _inConnection;

        public DisplayNode(IConnectionIn inConnection)
        {
            _inConnection = inConnection;
        }

        public int Value
        {
            get { return _inConnection?.Value ?? 0; }
            set { Debug.LogWarning("no you shouldn't be setting the inconnection value through the node" + value); }
        }
    }
}
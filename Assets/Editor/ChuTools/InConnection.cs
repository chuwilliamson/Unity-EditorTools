using System;
using Interfaces;

namespace ChuTools
{
    [Serializable]
    public class InConnection : IConnectionIn
    {
        public InConnection(IConnectionOut outConnection)
        {
            Out = outConnection;
        }

        public IConnectionOut Out { get; set; }
        public object Value => Out.Value;
    }
}
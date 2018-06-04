using Interfaces;

namespace ChuTools
{
    [System.Serializable]
    public class InConnection : IConnectionIn
    {
        public InConnection(IConnectionOut outConnection)
        {
            Out = outConnection;
        }

        public IConnectionOut Out { get; set; }
        public int Value => Out.Value;
    }
}
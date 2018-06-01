using UnityEditor;

namespace JeremyTools
{
    public class Connection
    {
        INode inNode;
        INode outNode;

        public Connection(INode inN, INode outN)
        {
            inNode = inN;
            outNode = outN;
        }

        public void Draw()
        {
            Handles.DrawLine(inNode.InCenter, outNode.OutCenter);
        }
    }
}
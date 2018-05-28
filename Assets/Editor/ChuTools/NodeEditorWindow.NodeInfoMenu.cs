using System;

namespace ChuTools
{
    public partial class NodeEditorWindow
    {
        public class NodeInfoMenu
        {
            public Action DrawElements;

            public void Draw()
            {
                DrawElements?.Invoke();
            }
        }
    }
}
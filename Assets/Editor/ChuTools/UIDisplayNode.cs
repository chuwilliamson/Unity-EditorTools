using System;
using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIDisplayNode : UIElement
    {
        public UIInConnectionPoint In;
        public INode Node { get; set; }

        public UIDisplayNode(Rect rect)
        {
            Node = new DisplayNode(null);
            In = new UIInConnectionPoint(new Rect(base.rect.position, new Vector2(50, 50)), Connect);
            Base("Display Node: ", "flow node 1", "flow node 1 on", rect);
        }

        private bool Connect(IConnectionOut outConnection)
        {
            if (outConnection == null)
                return false;
            Node = new DisplayNode(new InConnection(outConnection));
            return true;
        }

        public void Disconnect()
        {
            Node = null;
        }


        public override void Draw()
        {
            base.Draw();
            In.rect = new Rect(rect.position.x - 55, rect.position.y, 50, 50);
            In?.Draw();
            GUILayout.BeginArea(rect);
            var value = Node?.Value;
            GUILayout.Label("Value  ::  " + value);
            GUILayout.EndArea();
        }
    }
}
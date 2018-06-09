using System;
using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIDisplayNode : UIElement
    {
        public UIInConnectionPoint In { get; set; }
        public INode Node { get; set; }

        public UIDisplayNode(Rect rect)
        {
            Node = new DisplayNode(null);
            In = new UIInConnectionPoint(new Rect(base.rect.position, new Vector2(50, 50)), Connect);
            Base(name: "Display Node: ", normalStyleName: "flow node 1", selectedStyleName: "flow node 1 on", rect: rect);
        }

        private bool Connect(IConnectionOut outConnection)
        {
            if (outConnection == null)
                return false;
            Node = new DisplayNode(inConnection: new InConnection(outConnection: outConnection));
            return true;
        }

        public void Disconnect()
        {
            Node = null;
        }


        public override void Draw()
        {
            base.Draw();
            In.rect = new Rect(x: rect.position.x - 55, y: rect.position.y, width: 50, height: 50);
            In?.Draw();
            GUILayout.BeginArea(screenRect: rect);
            var value = Node?.Value;
            GUILayout.Label(text: "Value  ::  " + value);
            GUILayout.EndArea();
        }
    }
}
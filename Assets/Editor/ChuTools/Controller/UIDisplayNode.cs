using System;
using ChuTools.Model;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public class UIDisplayNode : UIElement
    {
        [JsonConstructor]
        public UIDisplayNode()
        {
            Node = new DisplayNode(null);
            In = new UIInConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), Connect, Disconnect)
            {
                Rect = new Rect(Rect.position.x - 55, Rect.position.y, 50, 50)
            };
            Base(name: "Display Node: ", normalStyleName: "flow node 1", selectedStyleName: "flow node 1 on",
                rect: Rect, resize: true);
        }


        public UIDisplayNode(Rect rect)
        {
            Node = new DisplayNode(null);
            In = new UIInConnectionPoint(new Rect(this.Rect.position, new Vector2(50, 50)), Connect, Disconnect)
            {
                Rect = new Rect(rect.position.x - 55, rect.position.y, 50, 50)
            };
            Base(name: "Display Node: ", normalStyleName: "flow node 1", selectedStyleName: "flow node 1 on",
                rect: rect, resize: true);
        }

        private bool Disconnect(UIInConnectionPoint point)
        {
            return false;
        }

        public bool Connect(IConnectionOut outConnection)
        {
            if(outConnection == null)
                return false;
            Node = new DisplayNode(new InConnection(outConnection));
            return true;
        }

        public bool Connect(IConnectionOut outConnection, UIInConnectionPoint connectionPoint)
        {
            return Connect(outConnection);
        }

        public void Disconnect()
        {
            Node = null;
        }

        public override void Draw()
        {
            base.Draw();
            In.Rect = new Rect(Rect.position.x - 55, Rect.position.y, 50, 50);
            In?.Draw();
            GUILayout.BeginArea(Rect);
            var value = Node?.Value;
            GUILayout.Label("Value  ::  " + value);
            GUILayout.EndArea();
        }

        public UIInConnectionPoint In { get; set; }
        public INode Node { get; set; }
    }
}
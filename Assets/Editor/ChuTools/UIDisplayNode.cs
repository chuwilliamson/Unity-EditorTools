using System;
using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIDisplayNode : UIElement
    {
        private UIInConnectionPoint _in;

        public UIInConnectionPoint In
        {
            get { return _in; }
            set { _in = value; }
        }
        private INode _node;

        public UIDisplayNode(Vector2 pos, Vector2 size) : base("Display Node: ", "flow node 1", "flow node 1 on", pos, size)
        {
            _node = new DisplayNode(null);
            _in = new UIInConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), Connect);
        }

        public bool Connect(IConnectionOut outConnection)
        {
            if (outConnection == null)
                return false;
            _node = new DisplayNode(new InConnection(outConnection));
            return true;
        }

        public void Disconnect()
        {
            _node = null;
        }


        public override void Draw()
        {
            base.Draw();
            _in.Rect = new Rect(Rect.position.x - 55, Rect.position.y, 50, 50);
            _in?.Draw();
            GUILayout.BeginArea(Rect);
            var value = _node?.Value;
            GUILayout.Label("Value  ::  " + value);
            GUILayout.EndArea();
        }
    }
}
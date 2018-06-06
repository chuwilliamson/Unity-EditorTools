using System;
using Interfaces;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIInputNode : UIElement
    {
        private readonly INode _node;
        private UIOutConnectionPoint _out;

        public UIOutConnectionPoint Out
        {
            get { return _out; }
            set { _out = value; }
        }

        public UIInputNode(Vector2 pos, Vector2 size) : base("Input Node", "flow node 2", "flow node 2 on", pos, size)
        {
            _node = new InputNode();
            _out = new UIOutConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), new OutConnection(_node));
            ControlId = GUIUtility.GetControlID(FocusType.Passive, Rect);
        }

        public override void Draw()
        {
            base.Draw();
            _out.Rect = new Rect(Rect.position.x + Rect.width, Rect.position.y, 50, 50);
            _out.Draw();

            GUILayout.BeginArea(Rect);
            _node.Value = EditorGUILayout.IntSlider("Value: ", _node.Value, 0, 10);
            GUILayout.EndArea();
        }
    }
}
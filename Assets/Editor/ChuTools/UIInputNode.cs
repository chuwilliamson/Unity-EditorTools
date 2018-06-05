using Interfaces;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [System.Serializable]
    public class UIInputNode : UIElement
    {
        private readonly INode _node;
        private readonly UIOutConnectionPoint _out;

        public UIInputNode(Vector2 pos, Vector2 size) : base("Input Node: ", pos, size)
        {
            _node = new InputNode();
            _out = new UIOutConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), new OutConnection(_node));
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
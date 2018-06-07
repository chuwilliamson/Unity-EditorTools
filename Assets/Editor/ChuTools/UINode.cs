using System;
using Interfaces;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChuTools
{
    /// <summary>
    ///     This node will represent a pass through node
    ///     It will receive data from it's in connection
    ///     That data will then be changed by the node implementation
    /// </summary>
    [Serializable]
    public class UINode : UIElement
    {
        [SerializeField]
        public UIInConnectionPoint _in;
        [SerializeField]
        public UIOutConnectionPoint _out;

        public INode _input { get; set; }

        public INode _transformation { get; set; }

        public INode _display { get; set; }

        public UINode(Rect rect)

        {
            _display = new DisplayNode(null);
            _in = new UIInConnectionPoint(new Rect(base.rect.position, new Vector2(50, 50)), Connect);
            _transformation = new InputNode();
            _input = new InputNode();
            _out = new UIOutConnectionPoint(new Rect(base.rect.position, new Vector2(50, 50)), new OutConnection(_transformation));
            Base("Transformation", "flow node 3", "flow node 3 on", rect);
        }

        private bool Connect(IConnectionOut outConnection)
        {
            if (outConnection == null) return false;
            _display = new DisplayNode(new InConnection(outConnection));
            return true;
        }

        public override void Draw()
        {
            base.Draw();
            _in.rect = new Rect(base.rect.position.x - 55, base.rect.position.y, 50, 50);
            _in?.Draw();
            _out.rect = new Rect(base.rect.position.x + base.rect.width, base.rect.position.y, 50, 50);
            _out?.Draw();

            GUILayout.BeginArea(base.rect);

            _input.Value = EditorGUILayout.IntSlider("Modifier: ", _input.Value, 0, 10);

            _transformation.Value = _display.Value + _input.Value;
            GUILayout.Label("Input: " + _input?.Value);
            GUILayout.Label("Display: " + _display?.Value);
            GUILayout.Label("Output: " + _transformation.Value);
            GUILayout.EndArea();
            var rect = new Rect(base.rect.x - 5 + base.rect.width / 2, base.rect.y - 5 + base.rect.height / 2, base.rect.width / 2,
                base.rect.height / 2);
            GUI.Box(rect, GUIContent.none);
            GUI.Label(rect, "ADD", new GUIStyle(Style) { fontSize = 55, alignment = TextAnchor.MiddleCenter });
        }
    }
}
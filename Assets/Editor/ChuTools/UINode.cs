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
        private UIInConnectionPoint _in;
        private INode _input;
        private UIOutConnectionPoint _out;
        private INode _transformation;
        private INode _display;

        public UINode(Rect rect)

        {
            _display = new DisplayNode(null);
            _in = new UIInConnectionPoint(new Rect(uRect.position, new Vector2(50, 50)), Connect);
            _transformation = new InputNode();
            _input = new InputNode();
            _out = new UIOutConnectionPoint(new Rect(uRect.position, new Vector2(50, 50)), new OutConnection(_transformation));
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
            _in.uRect = new Rect(uRect.position.x - 55, uRect.position.y, 50, 50);
            _in?.Draw();
            _out.uRect = new Rect(uRect.position.x + uRect.width, uRect.position.y, 50, 50);
            _out?.Draw();

            GUILayout.BeginArea(uRect);

            _input.Value = EditorGUILayout.IntSlider("Modifier: ", _input.Value, 0, 10);

            _transformation.Value = _display.Value + _input.Value;
            GUILayout.Label("Input: " + _input?.Value);
            GUILayout.Label("Display: " + _display?.Value);
            GUILayout.Label("Output: " + _transformation.Value);
            GUILayout.EndArea();
            var rect = new Rect(uRect.x - 5 + uRect.width / 2, uRect.y - 5 + uRect.height / 2, uRect.width / 2,
                uRect.height / 2);
            GUI.Box(rect, GUIContent.none);
            GUI.Label(rect, "ADD", new GUIStyle(Style) { fontSize = 55, alignment = TextAnchor.MiddleCenter });
        }
    }
}
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
        private readonly UIInConnectionPoint _in;
        private readonly INode _input;
        private readonly UIOutConnectionPoint _out;
        private readonly INode _transformation;
        private INode _display;

        private int slidervalue;

        public UINode(Vector2 pos, Vector2 size) : base("Transformation: ", pos, size)
        {
            _display = new DisplayNode(null);
            _in = new UIInConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), Connect);

            _transformation = new InputNode();
            _input = new InputNode();
            _out = new UIOutConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)),
                new OutConnection(_transformation));
        }

        public Object Prefab { get; set; }

        public bool Connect(IConnectionOut outConnection)
        {
            if (outConnection == null) return false;
            _display = new DisplayNode(new InConnection(outConnection));
            return true;
        }

        public override void Draw()
        {
            base.Draw();
            _in.Rect = new Rect(Rect.position.x - 55, Rect.position.y, 50, 50);
            _in?.Draw();
            _out.Rect = new Rect(Rect.position.x + Rect.width, Rect.position.y, 50, 50);
            _out?.Draw();

            GUILayout.BeginArea(Rect);
            Prefab = EditorGUILayout.ObjectField(Prefab, typeof(GameObject), true);
            if (GUILayout.Button("Spawn)"))
            {
                var go = Object.Instantiate(Prefab) as GameObject;
            }

            _input.Value = EditorGUILayout.IntSlider("Modifier: ", _input.Value, 0, 10);

            _transformation.Value = _display.Value + _input.Value;
            GUILayout.Label("Input: " + _input?.Value);
            GUILayout.Label("Display: " + _display?.Value);
            GUILayout.Label("Output: " + _transformation.Value);
            GUILayout.EndArea();
            var rect = new Rect(Rect.x - 5 + Rect.width / 2, Rect.y - 5 + Rect.height / 2, Rect.width / 2,
                Rect.height / 2);
            GUI.Box(rect, GUIContent.none);
            GUI.Label(rect, "ADD", new GUIStyle(Style) {fontSize = 55, alignment = TextAnchor.MiddleCenter});
        }
    }
}
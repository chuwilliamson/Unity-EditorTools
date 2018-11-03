using ChuTools.NodeEditor.Interfaces;
using ChuTools.NodeEditor.Model;
using Newtonsoft.Json;
using System;
using UnityEditor;
using UnityEngine;

namespace ChuTools.NodeEditor.Controller
{
    /// <summary>
    ///     This node will represent a pass through node
    ///     It will receive data from it's in connection
    ///     That data will then be changed by the node implementation
    /// </summary>
    [Serializable]
    public class UITransformationNode : UIElement
    {
        [JsonConstructor]
        public UITransformationNode(Rect rect)
        {
            Display = new DisplayNode(null);
            In = new UIInConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), Connect,
                DisconnectResponse);
            Transformation = new InputNode { Value = 0 };
            Input = new InputNode { Value = 0 };
            Out = new UIOutConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)),
                new OutConnection(Transformation));
            In.Rect = new Rect(Rect.position.x - 55, Rect.position.y, 50, 50);
            Out.Rect = new Rect(Rect.position.x + Rect.width, Rect.position.y, 50, 50);

            Base(name: "Transformation", normalStyleName: "flow node 3", selectedStyleName: "flow node 3 on",
                rect: rect, resize: true);
        }

        public UIInConnectionPoint In { get; set; }

        public UIOutConnectionPoint Out { get; set; }

        public INode Input { get; set; }

        public INode Transformation { get; set; }

        public INode Display { get; set; }

        private bool DisconnectResponse(UIInConnectionPoint point)
        {
            throw new NotImplementedException();
        }

        public bool Connect(IConnectionOut outConnection, UIInConnectionPoint connectionPoint)
        {
            return Connect(outConnection);
        }

        private bool Connect(IConnectionOut outConnection)
        {
            if (outConnection == null) return false;
            Display = new DisplayNode(new InConnection(outConnection));
            return true;
        }

        public override void Draw()
        {
            base.Draw();
            In.Rect = new Rect(Rect.position.x - 55, Rect.position.y, 50, 50);
            Out.Rect = new Rect(Rect.position.x + Rect.width, Rect.position.y, 50, 50);

            In?.Draw();
            Out?.Draw();

            GUILayout.BeginArea(Rect);

            Input.Value = EditorGUILayout.IntSlider("Modifier: ", Convert.ToInt32(Input.Value), 0, 10);

            Transformation.Value = Convert.ToInt32(Display.Value) + Convert.ToInt32(Input.Value);
            GUILayout.Label("Input: " + Input?.Value);
            GUILayout.Label("Display: " + Display?.Value);
            GUILayout.Label("Output: " + Transformation.Value);
            GUILayout.EndArea();
            var rect = new Rect(Rect.x - 5 + Rect.width / 2, Rect.y - 5 + Rect.height / 2,
                Rect.width / 2, Rect.height / 2);
            GUI.Box(rect, GUIContent.none);
            GUI.Label(rect, "ADD", new GUIStyle(Style) { fontSize = 55, alignment = TextAnchor.MiddleCenter });
        }
    }
}
using ChuTools.Model;
using Interfaces;
using Newtonsoft.Json;
using System;
using UnityEditor;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public sealed class UIInputNode : UIElement
    {
        [JsonConstructor]
        public UIInputNode(Rect rect)
        {
            Out = new UIOutConnectionPoint(new Rect(this.rect.position, new Vector2(50, 50)), new OutConnection(Node));
            ControlId = GUIUtility.GetControlID(FocusType.Passive, this.rect);
            Base(name: "Input Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on", rect: rect, resize: true);
        }

        public override void Draw()
        {
            base.Draw();
            Out.rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50);
            Out.Draw();

            GUILayout.BeginArea(rect);

            Node.Value = EditorGUILayout.IntSlider("Value: ", Convert.ToInt32(Node.Value), 0, 10);

            GUILayout.EndArea();
        }

        public INode Node { get; set; } = new InputNode { Value = 0 };
        public UIOutConnectionPoint Out { get; set; }
    }
}
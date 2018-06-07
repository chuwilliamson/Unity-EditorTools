using System;
using Interfaces;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIInputNode : UIElement
    {
        public INode Node { get; set; }
        public UIOutConnectionPoint Out;


        public UIInputNode(Rect rect)
        {
            Node = new InputNode();
            Out = new UIOutConnectionPoint(new Rect(base.rect.position, new Vector2(50, 50)), new OutConnection(Node));
            ControlId = GUIUtility.GetControlID(FocusType.Passive, base.rect);
            Base(name: "Input Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on", rect: rect);
        }

        public override void Draw()
        {
            base.Draw();
            Out.rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50);
            Out.Draw();

            GUILayout.BeginArea(rect);
            Node.Value = EditorGUILayout.IntSlider("Value: ", Node.Value, 0, 10);
            GUILayout.EndArea();
        }
    }
}
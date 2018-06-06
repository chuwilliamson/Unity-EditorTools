using System;
using Interfaces;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIInputNode : UIElement
    {
        public  INode Node { get; set; }
        public UIOutConnectionPoint Out { get; set; }

        
        public UIInputNode(Rect rect)
        {
            Node = new InputNode();
            Out = new UIOutConnectionPoint(new Rect(uRect.position, new Vector2(50, 50)), new OutConnection(Node));
            ControlId = GUIUtility.GetControlID(FocusType.Passive, uRect);
            Base("Input Node", "flow node 2", "flow node 2 on", rect);
        }

        public override void Draw()
        {
            base.Draw();
            Out.uRect = new Rect(uRect.position.x + uRect.width, uRect.position.y, 50, 50);
            Out.Draw();

            GUILayout.BeginArea(uRect);
            Node.Value = EditorGUILayout.IntSlider("Value: ", Node.Value, 0, 10);
            GUILayout.EndArea();
        }
    }
}
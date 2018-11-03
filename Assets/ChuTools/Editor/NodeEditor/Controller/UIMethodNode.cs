using ChuTools.NodeEditor.Interfaces;
using ChuTools.NodeEditor.Model;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ChuTools.NodeEditor.Controller
{
    [Serializable]
    public class UIMethodNode : UIElement
    {
        public INode Node;

        public UIOutConnectionPoint Out;

        [JsonConstructor]
        public UIMethodNode()
        {
            Out = new UIOutConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), new OutConnection(Node))
            {
                Rect = new Rect(Rect.position.x + Rect.width, Rect.position.y, 50, 50)
            };

            Node = new MethodNode(new MethodObject
            {
                Target = this,
                Type = typeof(UIMethodNode),
                MethodName = "TestMethod"
            });

            Base(Rect, "Method Node", resize: true);
        }

        public UIMethodNode(Rect rect)
        {
            Node = new MethodNode(new MethodObject
            {
                Target = this,
                Type = typeof(UIMethodNode),
                MethodName = "TestMethod"
            });

            Out = new UIOutConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), new OutConnection(Node))
            {
                Rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50)
            };

            Base(rect, "Method Node", resize: true);
        }

        public void TestMethod()
        {
            Debug.Log("im from the methodnode " + ControlId);
        }

        public override void Draw()
        {
            base.Draw();

            Out.Rect = new Rect(Rect.position.x + Rect.width, Rect.position.y, 50, 50);
            Out?.Draw();
            GUILayout.BeginArea(Rect);
            if (GUILayout.Button("DynamicInvoke"))
            {
                var obj = Node.Value as MethodObject;
                obj?.DynamicInvoke();
            }

            GUILayout.EndArea();
        }
    }
}
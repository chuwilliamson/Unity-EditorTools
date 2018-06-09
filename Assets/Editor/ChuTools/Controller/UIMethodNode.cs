using System;
using ChuTools.Model;
using Interfaces;
using JeremyTools;
using Newtonsoft.Json;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public class UIMethodNode : UIElement
    {
        [JsonConstructor]
        public UIMethodNode()
        {
            Out = new UIOutConnectionPoint(new Rect(rect.position, new Vector2(50, 50)), new OutConnection(Node));

            Out.rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50);

            Node = new MethodNode(new MethodObject
            {
                Target = this,
                Type = typeof(UIMethodNode),
                MethodName = "TestMethod"
            });

            Base(rect, "Method Node", resize: true);
        }

        public UIMethodNode(Rect rect)
        {
            Node = new MethodNode(new MethodObject
            {
                Target = this,
                Type = typeof(UIMethodNode),
                MethodName = "TestMethod"
            });

            Out = new UIOutConnectionPoint(new Rect(this.rect.position, new Vector2(50, 50)), new OutConnection(Node));

            Out.rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50);
            Base(rect, "Method Node", resize: true);
        }

        public void TestMethod()
        {
            Debug.Log("im from the methodnode " + ControlId);
        }

        public override void Draw()
        {
            base.Draw();

            Out.rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50);
            Out?.Draw();
            GUILayout.BeginArea(rect);
            if(GUILayout.Button("DynamicInvoke"))
            {
                var obj = Node.Value as MethodObject;
                obj?.DynamicInvoke();
            }
            GUILayout.EndArea();
        }

        public INode Node;

        public UIOutConnectionPoint Out;
    }
}
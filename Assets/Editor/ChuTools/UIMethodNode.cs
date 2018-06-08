using System.Reflection;
using Interfaces;
using JeremyTools;
using UnityEngine;

namespace ChuTools
{
    [System.Serializable]
    public class UIMethodNode : UIElement
    {
        public INode Node { get; set; }

        public UIOutConnectionPoint Out { get; set; }

        public UIMethodNode()
        {
            Node = new MethodNode(new MethodObject
            {
                Target = this,
                Type = typeof(UIMethodNode),
                MethodName = "TestMethod"
            });
            Out = new UIOutConnectionPoint(new Rect(this.rect.position, new Vector2(50, 50)), new OutConnection(Node));
            Base(rect, "Method Node");
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
            Base(rect, "Method Node");
        }

        public void TestMethod()
        {
            Debug.Log("im from the methodnode " + ControlId);
        }

        public override void Draw()
        {
            base.Draw();

            Out.rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50);
            Out.Draw();
            GUILayout.BeginArea(rect);
            if (GUILayout.Button("DynamicInvoke"))
            {
                var obj = Node.Value as MethodObject;
                obj?.DynamicInvoke();
            }
            GUILayout.EndArea();
        }
    }
}
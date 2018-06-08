using System.Reflection;
using Interfaces;
using JeremyTools;
using UnityEngine;

namespace ChuTools
{
    public class UIMethodNode : UIElement
    {
        public virtual INode Node { get; set; }

        private MethodInfo _methodInfo;
        public UIOutConnectionPoint Out;
        private object _sender;

        public UIMethodNode(Rect rect)
        {
            var t = GetType();
            var mName = "TestMethod";
            var mInfo = t.GetMethod(mName);
            var methodObject = new MethodObject {Target = this, MethodName = mName, Info = mInfo};

            Node = new MethodNode(methodObject);
            Out = new UIOutConnectionPoint(new Rect(this.rect.position, new Vector2(50, 50)), new OutConnection(Node));
            Base(rect, "Method Node", "flow node 0", "flow node 0 on");
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
        }
    }
}
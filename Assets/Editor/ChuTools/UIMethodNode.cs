using System.Collections.Generic;
using System.Reflection;
using JeremyTools;
using UnityEditor;
using UnityEngine;
using UIDelegateNode = JeremyTools.UIDelegateNode;

namespace ChuTools
{
    public class UIMethodNode : UIElement
    {
        private MethodInfo methodInfo;
        private object sender;

        public UIMethodNode(Rect rect)
        {
            Base(rect: rect, name: "Method Node", normalStyleName: "flow node 0", selectedStyleName: "flow node 0 on");
            var t = GetType();
            methodInfo = t.GetMethod("TestMethod");
            sender = this;
        }

        public void TestMethod()
        {
            Debug.Log("im from the methodnode " + ControlId.ToString());
        }

        public override void Draw()
        {
            base.Draw();
            GUILayout.BeginArea(Rect);

            if (GUILayout.Button("Add method to Delegate Node"))
            {
                UIDelegateNode.AddMethod(sender,methodInfo);
            }
            
            GUILayout.EndArea();
        }
    }
}
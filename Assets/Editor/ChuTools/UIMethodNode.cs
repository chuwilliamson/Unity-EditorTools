using System.Collections.Generic;
using System.Reflection;
using JeremyTools;
using UnityEditor;
using UnityEngine;
using UIDelegateNode = JeremyTools.UIDelegateNode;

namespace ChuTools
{
    /// <summary>
    /// ToDo: create an INode implementation of the data this node holds
    /// you 
    /// </summary>
    public class UIMethodNode : UIInputNode
    {
        private MethodInfo methodInfo;
        private object sender;

        public UIMethodNode(Rect rect) : base(rect)
        {
            var t = GetType();
            methodInfo = t.GetMethod("TestMethod");
            sender = this;
            Base(rect: rect, name: "Method Node", normalStyleName: "flow node 0", selectedStyleName: "flow node 0 on");
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
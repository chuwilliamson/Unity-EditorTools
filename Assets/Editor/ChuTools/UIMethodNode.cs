using System.Collections.Generic;
using System.Reflection;
using JeremyTools;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class UIMethodNode : UIElement
    {
        private UIDelegateNode a;
        private JeremyTools.CustomCallback cb;
        public List<string> names = new List<string> { "trent", "jeremy", "matthew" };
        private MethodInfo methodInfo;
        public UIMethodNode(Rect rect)
        {
            Base(rect: rect, name: "Method Node", normalStyleName: "flow node 0", selectedStyleName: "flow node 0 on");
            var t = GetType();
            methodInfo = t.GetMethod("CallBack_impl");
            cb = () => { methodInfo.Invoke(this, new object[] { }); };
        }

        public void CallBack_impl()
        {
            Debug.Log("im from the methodnode " + ControlId.ToString());
        }

        public override void Draw()
        {
            base.Draw();
            GUILayout.BeginArea(Rect);
            if (GUILayout.Button("Add to Delegate Node"))
            {
                UIDelegateNode.CallbackReceiver += cb;
            }
            
            
            GUILayout.EndArea();
        }
    }
}
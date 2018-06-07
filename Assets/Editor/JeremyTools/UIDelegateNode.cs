using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ChuTools;
using UnityEngine;

namespace JeremyTools
{
    public delegate void CustomCallback();

    public class UIDelegateNode : UIElement
    {
        public static CustomCallback CallbackReceiver;
        public UIDelegateNode(Rect rect)
        {
            Base(rect: rect, name: "UIMethodNode");
        }

        public override void Draw()
        {
            base.Draw();
            GUILayout.BeginArea(rect);
            foreach (var m in CallbackReceiver.GetInvocationList())
            {
                if (GUILayout.Button(m.Method.Name))
                {
                    m.Method.Invoke(this, new object[] { });
                }
            }
        }


        public void AddDelegate(MethodInfo callbackSender)
        {
            CallbackReceiver += () =>
            {
                callbackSender.Invoke(this, new object[] { });
            };
        }
    }
}
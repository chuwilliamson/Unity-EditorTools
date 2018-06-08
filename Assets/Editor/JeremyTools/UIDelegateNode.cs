using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ChuTools;
using UnityEditor;
using UnityEngine;

namespace JeremyTools
{
    public class UIDelegateNode : UIElement
    {
        public static List<MethodObject> MethodObjects = new List<MethodObject>();

        public UIDelegateNode(Rect rect)
        {
            MethodObjects = new List<MethodObject>();
            Base(rect: rect, name: "Delegate Node");
        }

        public override void Draw()
        {
            base.Draw();
            if (MethodObjects.Count < 1)
                return;
            EditorGUILayout.LabelField("no methods");

            GUILayout.BeginArea(rect);
            EditorGUILayout.BeginVertical();
            MethodObjects.ForEach(n => EditorGUILayout.LabelField(n.MethodName));
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Invoke"))
            {
                foreach (var mo in MethodObjects)
                {
                    mo.Invoke();
                }
            }

            GUILayout.EndArea();
        }

        public static void AddMethod(object sender, MethodInfo method)
        {
            var id = sender as UIElement;
            
            MethodObjects.Add(new MethodObject { Info = method, MethodName = id.ControlId + "::" + method.Name, Target = sender });
        }

        
    }
}

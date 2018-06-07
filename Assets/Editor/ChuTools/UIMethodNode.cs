using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class UIMethodNode : UIElement
    {
        public List<string> names = new List<string> {"trent", "jeremy", "matthew"};

        public UIMethodNode(Rect rect)
        {
            Base(rect: rect, name: "Method Node", normalStyleName: "flow node 0", selectedStyleName: "flow node 0 on");
        }

        public override void Draw()
        {
            base.Draw();
            GUILayout.BeginArea(Rect);
            names.ForEach(name =>
            {
                EditorGUILayout.LabelField(name);
                if (GUILayout.Button(name)) Debug.Log(name);
            });
            GUILayout.EndArea();
        }
    }
}
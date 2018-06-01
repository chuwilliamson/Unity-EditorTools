using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public partial class NodeEditorWindow
    {
        public void DrawMenu()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35))
            ) Save();

            GUILayout.Space(5);
            if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35))
            ) Load();
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            wantsMouseMove = EditorGUILayout.Toggle(wantsMouseMove);
            if (GUILayout.Button("Reset", GUILayout.Width(150)))
                Nodes.ForEach(n => n._Rect.center = CenterWindow);
            EditorGUILayout.LabelField("width", Screen.width.ToString());
            EditorGUILayout.LabelField("height", Screen.height.ToString());
            EditorGUILayout.LabelField("HotControl", GUIUtility.hotControl.ToString());
            EditorGUILayout.LabelField("Path", _path);

            var value1 = "null";
            var value2 = "null";

            EditorGUILayout.LabelField("EventSystem Selected", value1);
            EditorGUILayout.LabelField("EventSystem Will Selected   ", value2);
            EditorGUILayout.EndVertical();
        }
    }
}
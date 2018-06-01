using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public partial class NodeEditorWindow
    {
        public void DrawMenu()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35))) Save();

            GUILayout.Space(5);
            if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35))) Load();
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            wantsMouseMove = EditorGUILayout.Toggle(wantsMouseMove);
            EditorGUILayout.LabelField("Width", Screen.width.ToString());
            EditorGUILayout.LabelField("Height", Screen.height.ToString());
            EditorGUILayout.LabelField("HotControl: ", GUIUtility.hotControl.ToString());
            EditorGUILayout.LabelField("Control Name: ", GUI.GetNameOfFocusedControl());
            EditorGUILayout.LabelField("Path", _path);
            EditorGUILayout.LabelField("Current Event", NodeEvents.Current.ToString());
            Event popEvent = null;
            var nextevent = "";
            if (Event.GetEventCount() > 0)
            {
                if (Event.PopEvent(outEvent: popEvent))
                {
                    nextevent = popEvent.ToString();
                }
            }

            EditorGUILayout.LabelField("Next Event ", nextevent);
            EditorGUILayout.LabelField("Event count ", Event.GetEventCount().ToString());

            var value1 = "null";
            var value2 = "null";

            EditorGUILayout.LabelField("EventSystem Selected", value1);
            EditorGUILayout.LabelField("EventSystem Will Selected   ", value2);
            EditorGUILayout.EndVertical();
        }
    }
}
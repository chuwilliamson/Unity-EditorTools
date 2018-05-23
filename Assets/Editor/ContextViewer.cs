using UnityEditor;
using UnityEngine;
using System.Linq;
using States;

namespace _Editor
{
    public class ContextViewer : EditorWindow
    {
        Object obj;
        [MenuItem("Tools/StackViewer")]
        static void Init()
        {
            var w = CreateInstance<ContextViewer>();
            w.Show();
        }

        void OnGUI()
        {
            var fsm = Selection.activeGameObject.GetComponent<StackFSMBehaviour>();
            if (fsm == null) return;
            var states = fsm.AntContext?.Stack?.ToList();

            if (states?.Count <= 0)
                return;

            EditorGUILayout.LabelField(fsm.name);
            
            for (int i = 0; i < states?.Count; i++)
            {
                var rect = new Rect(new Vector2(10, Screen.height), new Vector2(150, 50));
                rect.y = rect.y + (i * rect.height);
                rect.y += i * 25;
                var state = states[i].GetType().Name;
                GUI.Box(position: rect, content: new GUIContent(text: state));
            }

            Repaint();
        }
    }
}
using UnityEditor;
using UnityEngine;
namespace Editor
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
            var states = fsm.AntContext.Stack.ToArray();
            int i = 0;
            foreach (var s in states)
            {
                //GUI.Box()
                i++;
            }

        }
    }
}
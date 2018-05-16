using UnityEditor;
using UnityEngine;
namespace Editor
{
    public class ContextViewer : EditorWindow
    {
    
        [MenuItem("Tools/StackViewer")]
        static void Init()
        {
            var w = CreateInstance<ContextViewer>();
            w.Show();
        }
        
        void OnGUI()
        {
            var rect1 = new UnityEngine.Rect();
            var activeObject = Selection.activeObject;
            var so = new SerializedObject(activeObject);
            //EditorGUI.ObjectField(position: rect1, );
        }
    }
}
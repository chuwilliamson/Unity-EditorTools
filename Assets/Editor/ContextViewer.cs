using UnityEditor;

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

        }
    }
}
using UnityEditor;
namespace ChuTools
{
    public abstract class CustomEditorWindow : EditorWindow
    {
        public readonly IEventSystem _nodeEventSystem = new EditorEventSystem();
    }
}
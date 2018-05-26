using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public interface IDrawable
    {
        void Draw(Event e);
    }

    public class NodeWindow : CustomEditorWindow
    {
        public static List<IDrawable> Drawables = new List<IDrawable>();

        [MenuItem("Tools/NodeWindow")]
        static void Init()
        {
            var window = GetWindow<NodeWindow>();
            window.Show();
        }

        void OnEnable()
        { 
            Drawables = new List<IDrawable>();
            MyEventSystem.OnContextClick += CreateContextMenu; 
        }

        void OnGUI()
        { 
            MyEventSystem.PollEvents(e: Event.current);
            EditorGUILayout.LabelField("width", Screen.width.ToString());
            EditorGUILayout.LabelField("height", Screen.height.ToString());
            var value = "null";
            var value2 = "null";
            if (MyEventSystem.Selected != null)
            {
                value = MyEventSystem.Selected.ToString();
                value2 = MyEventSystem.WillSelect.ToString();
            }
                
            EditorGUILayout.LabelField("EventSystem Selected", label2: value);
            EditorGUILayout.LabelField("EventSystem Will Selected   ", label2: value2);

            Drawables.ForEach(n=>n.Draw(e: Event.current)); 
            Repaint();
        }

        void CreateContextMenu(Event e)
        {
            var pos = e.mousePosition;
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Create Node"), false, () => { CreateNode(pos: pos); });
            gm.AddItem(new GUIContent("Clear Nodes"), false, func: ClearNodes);
            gm.ShowAsContext();
            e.Use();
        }

        void CreateNode(Vector2 pos)
        {
            Drawables.Add(new Node(position: pos, id: Drawables.Count, eventSystem: MyEventSystem));
        }

        void ClearNodes()
        {
            Drawables = new List<IDrawable>();
        }
    }
}
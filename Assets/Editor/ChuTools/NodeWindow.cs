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
        private static void Init()
        {
            var window = GetWindow<NodeWindow>();
            window.Show();
        }

        private void OnEnable()
        { 
            Drawables = new List<IDrawable>();
            MyEventSystem.OnContextClick += CreateContextMenu; 
        }

        private void OnGUI()
        { 
            MyEventSystem.PollEvents(Event.current);
            EditorGUILayout.LabelField("width", label2: Screen.width.ToString());
            EditorGUILayout.LabelField("height", label2: Screen.height.ToString());
            var value = "null";
            if (MyEventSystem.Selected != null)
                value = MyEventSystem.Selected.ToString();
            EditorGUILayout.LabelField("EventSystem Selected", label2: value);
            
            Drawables.ForEach(n=>n.Draw(Event.current)); 
            Repaint();
        }

        private void CreateContextMenu(Event e)
        {
            var pos = e.mousePosition;
            var gm = new GenericMenu();
            gm.AddItem(content: new GUIContent("Create Node"), on: false, func: () => { CreateNode(pos); });
            gm.AddItem(content: new GUIContent("Clear Nodes"), on: false, func: ClearNodes);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateNode(Vector2 pos) => Drawables.Add(item: new Node(pos, Drawables.Count) { NodeEventSystem = MyEventSystem });

        private void ClearNodes() => Drawables = new List<IDrawable>();

       
    }
}
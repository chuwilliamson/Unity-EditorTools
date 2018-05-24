using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class NodeWindow : CustomEditorWindow
    {
        private List<Node> _nodes;

        [MenuItem("Tools/NodeWindow")]
        private static void Init()
        {
            var window = GetWindow<NodeWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            _nodes = new List<Node>();
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

        private void CreateNode(Vector2 pos) => _nodes.Add(item: new Node(pos, _nodes.Count) { NodeEventSystem = MyEventSystem });

        private void ClearNodes() => _nodes = new List<Node>();

       
    }
}
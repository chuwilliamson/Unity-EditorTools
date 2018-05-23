using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
namespace ChuTools
{
    public class NodeWindow : CustomEditorWindow
    {        
        private List<Node> nodes;

        [MenuItem("Tools/NodeWindow")]
        private static void Init()
        {
            var window = GetWindow<NodeWindow>();
            window.Show();
        }
        void OnEnable()
        {
            nodes = new List<Node>();
            onContextClick += CreateContextMenu;
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("width", Screen.width.ToString());
            EditorGUILayout.LabelField("height", Screen.height.ToString());
            base.PollEvents(Event.current);
            Repaint(); 
        }

        void CreateContextMenu(Event e)
        {
            var position = e.mousePosition;
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Create Node"), false, () => { CreateNode(position); });
            gm.AddItem(new GUIContent("Clear Nodes"), false, ClearNodes);
            gm.ShowAsContext();
        }

        void CreateNode(Vector2 position)
        {
            nodes.Add(new Node(position));
        }

        void ClearNodes()
        {
            nodes = new List<Node>();
        }
    }

}
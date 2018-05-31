using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class NodeEditorWindow : EditorWindow
    {
        public static IEventSystem NodeEvents { get; private set; }

        public Vector2 CenterWindow => new Vector2(x: Screen.width / 2.0f, y: Screen.height / 2.0f);
        
        public class NodeList //just for saving
        {
            public List<Node> Nodes;
        }
         
        public List<Node> Nodes;
        public List<ConnectionPoint> ConnectionPoints;


        private string _path => Application.dataPath + "/Dialogue/nodes.json";

        [MenuItem("Tools/NodeWindow")]
        private static void Init()
        {
            var window = GetWindow<NodeEditorWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            Nodes = new List<Node>();
            ConnectionPoints = new List<ConnectionPoint>();
            NodeEvents = new NodeWindowEventSystem();
            NodeEvents.OnContextClick += CreateContextMenu;
        }


        private void OnGUI()
        {
            NodeEvents.PollEvents(Event.current);

            DrawMenu();
            Nodes.ForEach(n => n.Draw());

            if (GUI.changed)
                Repaint();
        }
        

        private void CreateContextMenu(Event e)
        {
            var pos = e.mousePosition;
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Create Node"), false, CreateNode, e);
            gm.AddItem(new GUIContent("Clear Nodes"), false, ClearNodes);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateNode(object e)
        {
            var pos = ((Event) e).mousePosition;
            Nodes.Add(item: new Node(pos, size: new Vector2(150, 50), id: Nodes.Count, onRemoveNode: RemoveNode));
        }

        private void RemoveNode(Node n)
        {
            Nodes.Remove(n);
        }

        private void ClearNodes()
        {
            Nodes = new List<Node>();
        }

        private void Save()
        {
            var n = new NodeList();
            Nodes.ForEach(node => n.Nodes.Add(node));

            var json = JsonUtility.ToJson(n, true);

            File.WriteAllText(_path, json);
        }

        private void Load()
        {
            var json = File.ReadAllText(_path);
            var n = new NodeList();
            JsonUtility.FromJsonOverwrite(json, n);
            Nodes = n.Nodes;
        }
    }
}
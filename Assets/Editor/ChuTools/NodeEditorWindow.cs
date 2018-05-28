using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public partial class NodeEditorWindow : CustomEditorWindow
    {
        [Serializable]
        public class NodeList
        {
            public List<Node> Nodes;
        }

        public List<Node> _nodes = new List<Node>();
        private List<Connection> _connections;
        private NodeInfoMenu _infoMenu;

        private string _path
        {
            get { return Application.dataPath + "/Dialogue/nodes.json"; }
        }

 
        private void Save()
        {
            var n = new NodeList();
            _nodes.ForEach(node => n.Nodes.Add(node));

            var json = JsonUtility.ToJson(n ,true);
            
            System.IO.File.WriteAllText(_path, json);
            Debug.Log("save");
        }

        private void Load()
        {
            var json = System.IO.File.ReadAllText(_path);
            var n = new NodeList();
            JsonUtility.FromJsonOverwrite(json, n);
            _nodes = n.Nodes;
        }
        [MenuItem("Tools/ChuTools/NodeWindow")]
        private static void Init()
        {
            var window = GetWindow<NodeEditorWindow>();

            window.Show();
        }

        void OnEnable()
        {
            _infoMenu =
                 new NodeInfoMenu
                 {
                     DrawElements = () =>
                     {
                         EditorGUILayout.BeginVertical();
                         wantsMouseMove = EditorGUILayout.Toggle(wantsMouseMove);

                         EditorGUILayout.LabelField("width", Screen.width.ToString());
                         EditorGUILayout.LabelField("height", Screen.height.ToString());
                         EditorGUILayout.LabelField("HotControl", GUIUtility.hotControl.ToString());
                         EditorGUILayout.LabelField("Path", _path);

                         var value1 = _nodeEventSystem.Selected?.ToString() ?? "null";
                         var value2 = _nodeEventSystem.WillSelect?.ToString() ?? "null";

                         EditorGUILayout.LabelField("EventSystem Selected", value1);
                         EditorGUILayout.LabelField("EventSystem Will Selected   ", value2);
                         EditorGUILayout.EndVertical();
                     }
                 };

            _nodes = new List<Node>();
            wantsMouseMove = true;
            _nodeEventSystem.Selected = this;
            _nodeEventSystem.WillSelect = this;
            _nodeEventSystem.OnContextClick += CreateContextMenu;
        }

        void OnGUI()
        {
            if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35)))
                Save();

            GUILayout.Space(5);

            if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35)))
                Load();

            _nodeEventSystem.PollEvents(e: Event.current);
            _infoMenu.Draw();
            _nodes.ForEach(n => n.PollEvents(Event.current));
            _nodes.ForEach(n => n.Draw());

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

        private void OnConnectionCreated(Connection connection)
        {

        }

        private void CreateNode(Vector2 pos)
        {
            _nodes.Add(new Node(pos, new Vector2(250, 200), _nodes.Count, RemoveNode));

        }

        private void RemoveNode(Node n)
        {
            _nodes.Remove(n);
        }

        private void ClearNodes()
        {
            _nodes = new List<Node>();
        }
    }
}
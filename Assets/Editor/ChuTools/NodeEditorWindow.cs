using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public partial class NodeEditorWindow : EditorWindow
    {
        [Serializable]
        public class NodeList //just for saving
        {
            public List<Node> Nodes;
        }

        public List<Node> Nodes = new List<Node>();
        private List<Connection> _connections;
        private NodeInfoMenu _infoMenu;


        private string _path
        {
            get { return Application.dataPath + "/Dialogue/nodes.json"; }
        }

        [MenuItem("Tools/ChuTools/NodeWindow")]
        private static void Init()
        {
            var window = GetWindow<NodeEditorWindow>();

            window.Show();
        }

        public Vector2 CenterWindow
        {
            get { return new Vector2(Screen.width / 2.0f, Screen.height / 2.0f); }
        }
        void OnEnable()
        {
            _infoMenu =
                 new NodeInfoMenu
                 {
                     DrawElements = () =>
                     {

                         GUILayout.BeginHorizontal();
                         if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35))) Save();

                         GUILayout.Space(5);
                         if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35))) Load();
                         GUILayout.EndHorizontal();

                         EditorGUILayout.BeginVertical();
                         wantsMouseMove = EditorGUILayout.Toggle(wantsMouseMove);
                         if (GUILayout.Button("Reset", GUILayout.Width(150)))
                             Nodes.ForEach(n => n.BackgroundRect.center = CenterWindow);
                         EditorGUILayout.LabelField("width", Screen.width.ToString());
                         EditorGUILayout.LabelField("height", Screen.height.ToString());
                         EditorGUILayout.LabelField("HotControl", GUIUtility.hotControl.ToString());
                         EditorGUILayout.LabelField("Path", _path);

                         var value1 = "null";
                         var value2 = "null";

                         EditorGUILayout.LabelField("EventSystem Selected", value1);
                         EditorGUILayout.LabelField("EventSystem Will Selected   ", value2);
                         EditorGUILayout.EndVertical();
                     }
                 };

            Nodes = new List<Node>();
            wantsMouseMove = true;
        }

        void PollEvents()
        {
            switch (Event.current.type)
            {
                case EventType.ContextClick:
                    CreateContextMenu(Event.current);
                    break;
            }
        }
        void OnGUI()
        {
            PollEvents();
            Nodes.ForEach(n => n.PollEvents());

            _infoMenu.Draw();
            Nodes.ForEach(n => n.Draw()); 

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
            Nodes.Add(new Node(pos, new Vector2(250, 200), Nodes.Count, RemoveNode));
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

            System.IO.File.WriteAllText(_path, json);
            Debug.Log("save");
        }

        private void Load()
        {
            var json = System.IO.File.ReadAllText(_path);
            var n = new NodeList();
            JsonUtility.FromJsonOverwrite(json, n);
            Nodes = n.Nodes;
        }

    }
}
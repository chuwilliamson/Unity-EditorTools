using System;
using System.Collections.Generic;
using System.IO;
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


        private string _path => Application.dataPath + "/Dialogue/nodes.json";

        [MenuItem("Tools/NodeWindow")]
        private static void Init()
        {
            var window = GetWindow<NodeEditorWindow>();
            window.Show();
        }

        public Vector2 CenterWindow => new Vector2(x: Screen.width / 2.0f, y: Screen.height / 2.0f);

        private void OnEnable()
        {
            _infoMenu = new NodeInfoMenu
            {
                DrawElements = () =>
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35))
                    ) Save();

                    GUILayout.Space(5);
                    if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35))
                    ) Load();
                    GUILayout.EndHorizontal();

                    EditorGUILayout.BeginVertical();
                    wantsMouseMove = EditorGUILayout.Toggle(wantsMouseMove);
                    if (GUILayout.Button("Reset", GUILayout.Width(150)))
                        Nodes.ForEach(n => n._Rect.center = CenterWindow);
                    EditorGUILayout.LabelField("width", label2: Screen.width.ToString());
                    EditorGUILayout.LabelField("height", label2: Screen.height.ToString());
                    EditorGUILayout.LabelField("HotControl", label2: GUIUtility.hotControl.ToString());
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


        private void OnGUI()
        {
            PollEvents(Event.current);
            Nodes.ForEach(n => n.PollEvents(Event.current));

            _infoMenu.Draw();
            Nodes.ForEach(n => n.Draw());

            if(GUI.changed)
                Repaint();
        }

        private void PollEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.ContextClick:
                    CreateContextMenu(Event.current);
                    break;
            }
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

        private void CreateNode(Vector2 pos)
        {
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
            Debug.Log("save");
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
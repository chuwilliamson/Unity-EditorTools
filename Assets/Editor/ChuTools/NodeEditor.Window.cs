using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Interfaces;
using JeremyTools;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class NodeEditorWindow : EditorWindow
    {
        public static GUIStyle NodeStyle;
        public static GUIStyle SelectedNodeStyle;
        public static GUIStyle InPointStyle;
        public static GUIStyle OutPointStyle;
        public IConnectionIn TESTIN;
        public IConnectionOut TESTOUT;
        public List<IDrawable> Connections;
        public List<IDrawable> Nodes;
        public static IEventSystem NodeEvents { get; private set; }
        public Vector2 CenterWindow => new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        private string _path => Application.dataPath + "/Dialogue/nodes.json";

        [MenuItem("Tools/ChuTools/NodeWindow")]
        private static void Init()
        {
            var window = GetWindow<NodeEditorWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            wantsMouseMove = true;
            SelectedNodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D
                },
                border = new RectOffset(12, 12, 12, 12)
            };

            InPointStyle = new GUIStyle
            {
                normal
                    = {background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D},
                hover =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D
                },
                border = new RectOffset(4, 4, 12, 12)
            };

            OutPointStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D
                },
                active =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D
                },
                border = new RectOffset(4, 4, 12, 12)
            };


            NodeStyle = new GUIStyle
            {
                normal = {background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D},
                border = new RectOffset(12, 12, 12, 12)
            };
            Nodes = new List<IDrawable>();
            Connections = new List<IDrawable>();
            NodeEvents = new NodeWindowEventSystem();

            NodeEvents.OnContextClick += CreateContextMenu;
            var c = new ConnectionPoint(new Vector2(500, 50), new Vector2(25, 25));
            var d = new ConnectionPoint(new Vector2(600, 50), new Vector2(25, 25));
            Connections.Add(new Connection(c, d));
        }
        
        private void OnGUI()
        {
            NodeEvents.PollEvents(Event.current);
            if (GUI.changed)
                Repaint();

            GUILayout.BeginHorizontal();
            var value1 = "null";
            var value2 = "null";
            if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35))) Save();
            GUILayout.Space(5);
            if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35))) Load();
            GUILayout.EndHorizontal();
            
            var lastrect = GUILayoutUtility.GetLastRect();
            var pos = new Vector2(lastrect.xMin, lastrect.yMax);
            var menurect = new Rect(pos, new Vector2(250, 200));
            

            GUI.BeginGroup(menurect);
            GUI.Box(menurect, GUIContent.none);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Width", Screen.width.ToString());
            wantsMouseMove = EditorGUILayout.Toggle("WantsMouseMove", wantsMouseMove);
            EditorGUILayout.LabelField("Height", Screen.height.ToString());
            EditorGUILayout.LabelField("HotControl: ", GUIUtility.hotControl.ToString());
            EditorGUILayout.LabelField("Control Name: ", GUI.GetNameOfFocusedControl());
            EditorGUILayout.LabelField("Path", _path);
            EditorGUILayout.LabelField("Current Event", NodeEvents.Current.ToString());
            EditorGUILayout.LabelField("Event count ", Event.GetEventCount().ToString());
            EditorGUILayout.LabelField("EventSystem Selected", value1);
            EditorGUILayout.LabelField("EventSystem Will Selected   ", value2);
    
            EditorGUILayout.EndVertical();
            GUI.EndGroup();
            if (TESTOUT != null)
            {
                if (GUILayout.Button("Make Connection?"))
                {
                    _displayNode.Connect(TESTOUT);
                }
            }
            Nodes.ForEach(n => n.Draw());
            Connections.ForEach(c => c.Draw());
        }


        private void CreateContextMenu(Event e)
        {
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Create Node"), false, CreateNode, e);
            gm.AddItem(new GUIContent("Create Input Node"), false, CreateInputNode, e);
            gm.AddItem(new GUIContent("Create Display Node"), false, CreateDisplayNode, e);
            gm.AddItem(new GUIContent("Clear Nodes"), false, ClearNodes);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateDisplayNode(object e)
        {
            var pos = ((Event)e).mousePosition;
            _displayNode = new UIDisplayNode(pos, new Vector2(300, 150), SetConnectionIn);
            Nodes.Add(_displayNode);
        }

        private UIDisplayNode _displayNode;
        private void CreateInputNode(object e)
        {
            var pos = ((Event)e).mousePosition;
            
            Nodes.Add( new UIInputNode(pos, new Vector2(300, 150), SetConnectionOut));
        }

        private void SetConnectionOut(IConnectionOut outConnection)
        {
            TESTOUT = outConnection;
        }

        private void SetConnectionIn(IConnectionIn inConnectionIn)
        {
            TESTIN = inConnectionIn;
        }
        private void CreateNode(object e)
        {
            var pos = ((Event) e).mousePosition;
            Nodes.Add(new Node(pos, new Vector2(150, 50), RemoveNode, CreateConnection));
        }

        private void CreateConnection(Connection connection)
        {
            Connections.Add(connection);
        }


        private void RemoveNode(Node n)
        {
            Nodes.Remove(n);
        }

        private void ClearNodes()
        {
            Nodes = new List<IDrawable>();
        }

        private void Save()
        {
            var n = new NodeList();
            Nodes.ForEach(node => n.Nodes.Add(null));
            var json = JsonUtility.ToJson(n, true);
            File.WriteAllText(_path, json);
        }

        private void Load()
        {
            var json = File.ReadAllText(_path);
            var n = new NodeList();
            JsonUtility.FromJsonOverwrite(json, n);
        }

        public class NodeList //just for saving
        {
            public List<Node> Nodes;
        }
    }
}
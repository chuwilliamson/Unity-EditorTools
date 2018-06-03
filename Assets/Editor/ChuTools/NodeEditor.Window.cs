using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Interfaces;
using JeremyTools;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public delegate void ConnectionCreated(UIOutConnection @out, UIInConnection @in);
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class NodeEditorWindow : EditorWindow
    {
        public static ConnectionCreated ConnectionCreatedEvent;
        public static GUIStyle NodeStyle;
        public static GUIStyle SelectedNodeStyle;
        public static GUIStyle InPointStyle;
        public static GUIStyle OutPointStyle;

        public static IDrawable CurrentDrag;

        private bool _connect;
        private UIDisplayNode _displayNode;
        public List<IDrawable> Connections;
        public List<IDrawable> Nodes;
        public static IConnectionIn TESTIN;
        public static IConnectionOut TESTOUT;
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
                    = { background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D },
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
                normal = { background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D },
                border = new RectOffset(12, 12, 12, 12)
            };
            Nodes = new List<IDrawable>();
            Connections = new List<IDrawable>();
            NodeEvents = new NodeWindowEventSystem();

            NodeEvents.OnContextClick += CreateContextMenu;
            ConnectionCreatedEvent += OnConnectionCreated;
        }

        private void OnGUI()
        {
            NodeEvents.PollEvents(Event.current);
            if (GUI.changed)
                Repaint();

            DrawMenu();
           

            Nodes.ForEach(n => n.Draw());
            Connections.ForEach(c => c.Draw());


            if (CurrentDrag == null) return;
            Chutilities.DrawNodeCurve(CurrentDrag.Rect, new Rect(Event.current.mousePosition, CurrentDrag.Rect.size));
            var endRect = new Rect(Current.mousePosition, Vector2.one * 10);
            Handles.RectangleHandleCap(GUIUtility.GetControlID(FocusType.Passive, endRect), endRect.center,
                Quaternion.identity, 15, EventType.Repaint);
        }

        private void CreateContextMenu(Event e)
        {
            var gm = new GenericMenu();

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

        private void CreateInputNode(object e)
        {
            var pos = ((Event)e).mousePosition;

            Nodes.Add(new UIInputNode(pos, new Vector2(300, 150), SetConnectionOut));
        }

        private void OnConnectionCreated(UIOutConnection @out, UIInConnection @in)
        {
            Connections.Add(new Connection(@out, @in));
        }

        private static void SetConnectionOut(IConnectionOut outConnection)
        {
            TESTOUT = outConnection;
        }

        private static void SetConnectionIn(IConnectionIn inConnectionIn)
        {
            TESTIN = inConnectionIn;
        }

        private void RemoveNode(Node n)
        {
            Nodes.Remove(n);
        }

        private void ClearNodes()
        {
            Nodes = new List<IDrawable>();
            Connections = new List<IDrawable>();
            NodeEvents = new NodeWindowEventSystem();
            NodeEvents.OnContextClick += CreateContextMenu;
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

        public class NodeList//just for saving
        {
            public List<Node> Nodes;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Interfaces;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class NodeEditorWindow : EditorWindow
    {
        public static Action<UIOutConnectionPoint, UIInConnectionPoint> ConnectionCreatedEvent;

        public List<IDrawable> Connections;
        public List<IDrawable> Nodes;
        public static UIOutConnectionPoint CurrentSendingDrag { get; set; }
        public static UIInConnectionPoint CurrentAcceptingDrag { get; set; }
        public int NodeHeight { get; set; }
        public int NodeWidth { get; set; }
        public static IEventSystem NodeEvents { get; private set; }
        public Vector2 CenterWindow => new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        private string _path => Application.dataPath + "/Editor/ChuTools/nodes.json";

        [MenuItem("Tools/ChuTools/NodeWindow")]
        private static void Init()
        {
            var window = GetWindow<NodeEditorWindow>();
            window.Show();
        }

        public static void RequestConnection(UIOutConnectionPoint uiOut, IConnectionOut @out)
        {
            if (CurrentAcceptingDrag.ValidateConnection(@out))
            {
                ConnectionCreatedEvent.Invoke(CurrentSendingDrag, CurrentAcceptingDrag);
            }
            else
            {
                Debug.Log("cancel connection request");
                CurrentAcceptingDrag = null;
                CurrentSendingDrag = null;
            }
        }


        private void OnEnable()
        {
            ClearNodes();
        }

        private void OnGUI()
        {
            NodeEvents.PollEvents(Event.current);
            if (GUI.changed)
                Repaint();

            DrawMenu();
            DrawConnection();

            Nodes.ForEach(n => n.Draw());
            Connections.ForEach(c => c.Draw());
        }

        private void DrawConnection()
        {
            if (CurrentSendingDrag == null) return;
            Chutilities.DrawNodeCurve(CurrentSendingDrag.Rect,
                new Rect(Event.current.mousePosition, CurrentSendingDrag.Rect.size));
            var endRect = new Rect(Current.mousePosition, Vector2.one * 10);
            Handles.RectangleHandleCap(GUIUtility.GetControlID(FocusType.Passive, endRect), endRect.center,
                Quaternion.identity, 15, EventType.Repaint);
            GUI.changed = true;
        }

        private void CreateContextMenu(Event e)
        {
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Create Input-Output Node"), false, CreateNode, e);
            gm.AddItem(new GUIContent("Create Old Node"), false, CreateOldNode, e);
            gm.AddItem(new GUIContent("Create Input Node"), false, CreateInputNode, e);
            gm.AddItem(new GUIContent("Create Display Node"), false, CreateDisplayNode, e);
            gm.AddItem(new GUIContent("Clear Nodes"), false, ClearNodes);
            gm.ShowAsContext();
            e.Use();
        }
        private void CreateOldNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            Nodes.Add(new Node(pos, new Vector2(NodeWidth, NodeHeight), RemoveNode));
        }

        private void CreateNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            Nodes.Add(new UINode(pos, new Vector2(NodeWidth, NodeHeight)));
        }

        private void CreateDisplayNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            Nodes.Add(new UIDisplayNode(pos, new Vector2(NodeWidth, NodeHeight)));
        }

        private void CreateInputNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            Nodes.Add(new UIInputNode(pos, new Vector2(NodeWidth, NodeHeight)));
        }

        /// <summary>
        ///     when a connection is created add it to the connections list to draw
        /// </summary>
        /// <param name="out">the ui element associated with the out connection</param>
        /// <param name="in">the ui element associated with the in connection</param>
        private void OnConnectionCreated(UIOutConnectionPoint @out, UIInConnectionPoint @in)
        {
            if (@out != null && @in != null) Connections.Add(new UIBezierConnection(@out, @in));

            CurrentSendingDrag = null;
            CurrentAcceptingDrag = null;
        }


        private void RemoveNode(Node n)
        {
            Nodes.Remove(n);
        }

        private void ClearNodes()
        {
            NodeWidth = 300;
            NodeHeight = 150;
            wantsMouseMove = true;
            Nodes = new List<IDrawable>();
            Connections = new List<IDrawable>();
            NodeEvents = new NodeWindowEventSystem();

            NodeEvents.OnContextClick += CreateContextMenu;
            ConnectionCreatedEvent += OnConnectionCreated;
            NodeEvents.OnMouseUp += e =>
            {
                if (CurrentAcceptingDrag != null) return;
                CurrentAcceptingDrag = null;
                CurrentSendingDrag = null;
            };
        }

        private void Save()
        {
            var n = new NodeEditorWindowSaveLoad();
            Nodes.ForEach(node => n.Nodes.Add(node));
            Connections.ForEach(connection => n.Connections.Add(connection));

            var json = JsonConvert.SerializeObject(n,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });

            File.WriteAllText(_path, json);
        }

        private void Load()
        {
            var json = File.ReadAllText(_path);

            var n = JsonConvert.DeserializeObject<NodeEditorWindowSaveLoad>(json,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
            Nodes = n.Nodes;
            Connections = n.Connections;
        }

        [Serializable]
        public class NodeEditorWindowSaveLoad //just for saving
        {
            public List<IDrawable> Connections = new List<IDrawable>();
            public List<IDrawable> Nodes = new List<IDrawable>();
        }
    }
}
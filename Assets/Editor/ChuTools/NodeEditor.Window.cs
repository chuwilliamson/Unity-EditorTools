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

        public List<UIBezierConnection> Connections = new List<UIBezierConnection>();
        public List<UIElement> Nodes = new List<UIElement>();
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
            gm.AddItem(new GUIContent("Create Method Node"), false, CreateMethodNode, e);
            gm.AddItem(new GUIContent("Create Delegate Node"), false, CreateDelegateNode, e);
            gm.AddItem(new GUIContent("Create Input-Output Node"), false, CreateNode, e);
            gm.AddItem(new GUIContent("Create Input Node"), false, CreateInputNode, e);
            gm.AddItem(new GUIContent("Create Display Node"), false, CreateDisplayNode, e);
            gm.AddItem(new GUIContent("Clear Nodes"), false, ClearNodes);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateMethodNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth, NodeHeight));
            Nodes.Add(new ChuTools.UIMethodNode(rect));
        }

        private void CreateDelegateNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth, NodeHeight));
            Nodes.Add(new JeremyTools.UIDelegateNode(rect));
        }

        private void CreateNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth, NodeHeight));
            Nodes.Add(new UINode(rect));
        }

        private void CreateDisplayNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth, NodeHeight));
            Nodes.Add(new UIDisplayNode(rect));
        }

        private void CreateInputNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth, NodeHeight));
            Nodes.Add(new UIInputNode(rect));
        }

        /// <summary>
        ///     when a connection is created add it to the connections list to draw
        /// </summary>
        /// <param name="out">the ui element associated with the out connection</param>
        /// <param name="in">the ui element associated with the in connection</param>
        private void OnConnectionCreated(UIOutConnectionPoint @out, UIInConnectionPoint @in)
        {

            if (@out != null && @in != null)
            {
                Debug.Log("connection created");
                Connections.Add(new UIBezierConnection(@out, @in));
            }

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
            CurrentAcceptingDrag = null;
            CurrentSendingDrag = null;

            Nodes = new List<UIElement>();
            Connections = new List<UIBezierConnection>();
            NodeEvents = new NodeWindowEventSystem();
            
            NodeEvents.OnContextClick += CreateContextMenu;
            ConnectionCreatedEvent = null;
            ConnectionCreatedEvent += OnConnectionCreated;
            NodeEvents.OnMouseUp += e =>
            {
                if (CurrentAcceptingDrag != null) return;
                CurrentAcceptingDrag = null;
                CurrentSendingDrag = null;
            };
        }

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            Formatting = Formatting.Indented
        };

        private void Save()
        {
            var n = new NodeEditorWindowSaveLoad
            {
                Nodes = Nodes,
                Connections = Connections
            };

            var json = JsonConvert.SerializeObject(n, _settings);
            File.WriteAllText(_path, json);
        }

        private void Load()
        {
            ClearNodes();
            var json = File.ReadAllText(_path);
            var n = JsonConvert.DeserializeObject<NodeEditorWindowSaveLoad>(json, _settings);
            Nodes = n.Nodes;
            Connections = n.Connections;
        }
    }
    [Serializable]
    public class NodeEditorWindowSaveLoad //just for saving
    {
        public List<UIBezierConnection> Connections { get; set; }
        public List<UIElement> Nodes { get; set; }
    }
}

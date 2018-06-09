using ChuTools.Controller;
using Interfaces;
using JeremyTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using TrentTools;
using UnityEditor;
using UnityEngine;

namespace ChuTools.View
{
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class NodeEditorWindow : EditorWindow
    {
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
            gm.AddItem(new GUIContent("Create Roslyn Node"), false, CreateRoslynNode, e);
            gm.AddItem(new GUIContent("Create Method Node"), false, CreateMethodNode, e);
            gm.AddItem(new GUIContent("Create Delegate Node"), false, CreateDelegateNode, e);
            gm.AddItem(new GUIContent("Create Input-Output Node"), false, CreateNode, e);
            gm.AddItem(new GUIContent("Create Input Node"), false, CreateInputNode, e);
            gm.AddItem(new GUIContent("Create Display Node"), false, CreateDisplayNode, e);
            gm.AddItem(new GUIContent("Clear Nodes"), false, ClearNodes);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateRoslynNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth + 25, NodeHeight + 25));
            Nodes.Add(new RoslynNode(rect));
        }

        private void CreateMethodNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth, NodeHeight));
            Nodes.Add(new UIMethodNode(rect));
        }

        private void CreateDelegateNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth, NodeHeight));
            Nodes.Add(new UIDelegateNode(rect));
        }

        private void CreateNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth, NodeHeight));
            Nodes.Add(new UITransformationNode(rect));
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

        private void RemoveNode(IDrawable n)
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

            Nodes = new List<IDrawable>();
            Connections = new List<IDrawable>();
            NodeEvents = new NodeWindowEventSystem();

            NodeEvents.OnContextClick += CreateContextMenu;
            ConnectionCreatedEvent = null;
            ConnectionCreatedEvent = OnConnectionCreated;
            NodeEvents.OnMouseUp += e =>
            {
                if (CurrentAcceptingDrag != null) return;
                CurrentAcceptingDrag = null;
                CurrentSendingDrag = null;
            };
        }

        private void Save()
        {
            var n = new NodeEditorWindowSaveLoad { Nodes = Nodes, Connections = Connections };

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

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            Formatting = Formatting.Indented
        };

        public List<IDrawable> Connections = new List<IDrawable>();
        public List<IDrawable> Nodes = new List<IDrawable>();
        public static UIOutConnectionPoint CurrentSendingDrag { get; set; }
        public static UIInConnectionPoint CurrentAcceptingDrag { get; set; }
        public int NodeHeight { get; set; }
        public int NodeWidth { get; set; }
        public static IEventSystem NodeEvents { get; private set; }
        public Vector2 CenterWindow => new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        private string _path => Application.dataPath + "/Editor/ChuTools/nodes.json";
        public static Action<UIOutConnectionPoint, UIInConnectionPoint> ConnectionCreatedEvent;
    }

    [Serializable]
    public class NodeEditorWindowSaveLoad//just for saving
    {
        public List<IDrawable> Connections { get; set; }
        public List<IDrawable> Nodes { get; set; }
    }
}
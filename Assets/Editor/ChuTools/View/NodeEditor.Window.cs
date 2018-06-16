using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using ChuTools.Controller;
using Interfaces;
using JeremyTools;
using Newtonsoft.Json;
using TrentTools;
using UnityEditor;
using UnityEngine;

namespace ChuTools.View
{
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class NodeEditorWindow : EditorWindow
    {
        public static Action<UIInConnectionPoint> OnConnectionCancelRequest;

        public static Vector2 Drag;
        public static Action<UIOutConnectionPoint, UIInConnectionPoint> ConnectionCreatedEvent;

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ObjectCreationHandling = ObjectCreationHandling.Reuse,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            Formatting = Formatting.Indented,
            DefaultValueHandling = DefaultValueHandling.Populate,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        private Vector2 _offset;

        public List<IDrawable> Connections = new List<IDrawable>();
        public List<IDrawable> Nodes = new List<IDrawable>();
        public static UIOutConnectionPoint CurrentSendingDrag { get; set; }
        public static UIInConnectionPoint CurrentAcceptingDrag { get; set; }
        public int NodeHeight { get; set; }
        public int NodeWidth { get; set; }
        public static IEventSystem NodeEventSystem { get; private set; }
        public Vector2 CenterWindow => new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        private string _path => Application.dataPath + "/Editor/ChuTools/nodes.json";

        [MenuItem("Tools/ChuTools/NodeWindow %g")]
        private static void Init()
        {
            var window = GetWindow<NodeEditorWindow>();
            window.InitializeComponents();
            window.ShowTab();
        }

        private void RequestCancelConnection(UIInConnectionPoint uiIn)
        {
            IDrawable flagForRemove = null;
            foreach (var connection in Connections)
                flagForRemove = (connection as UIBezierConnection).@in == uiIn ? connection : null;

            Connections.Remove(flagForRemove);
        }

        public static void RequestConnection(UIOutConnectionPoint uiOut, IConnectionOut @out)
        {
            if (CurrentAcceptingDrag.ValidateConnection(@out))
            {
                ConnectionCreatedEvent(CurrentSendingDrag, CurrentAcceptingDrag);
            }
            else
            {
                Debug.Log("cancel connection request");
                CurrentAcceptingDrag = null;
                CurrentSendingDrag = null;
            }
        }

        private void OnDisable()
        {
            try
            {
                Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void OnGUI()
        {
            if (NodeEventSystem == null)
            {
                GUILayout.Label(new GUIContent("NO EVENT SYSTEM PLEASE RELOAD"), EditorStyles.largeLabel);
                if (GUILayout.Button("Reload"))
                    InitializeComponents();
                return;
            }

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawMenu();
            if (new UITypesDropdown().Button(GUILayoutUtility.GetLastRect()))
            {

            }
            DrawConnection();
            var f = Time.deltaTime;
            Nodes.ForEach(n => n.Draw());
            Connections.ForEach(c => c.Draw());

            NodeEventSystem?.PollEvents(Event.current);

            if (GUI.changed)
                Repaint();
        }

        private static void DrawConnection()
        {
            if (CurrentSendingDrag == null) return;

            var endRect = new Rect(Current.mousePosition, new Vector2(10, 10));
            Handles.DrawSolidRectangleWithOutline(endRect, Color.cyan, Color.black);
            Chutilities.DrawNodeCurve(CurrentSendingDrag.rect, endRect);
            GUI.changed = true;
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            var widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            var heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            _offset += Drag * .5f;
            var newOffset = new Vector3(_offset.x % gridSpacing, _offset.y % gridSpacing, 0);

            for (var i = 0; i < widthDivs; i++)
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,
                    new Vector3(gridSpacing * i, position.height, 0f) + newOffset);

            for (var j = 0; j < heightDivs; j++)
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,
                    new Vector3(position.width, gridSpacing * j, 0f) + newOffset);

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void CreateContextMenu(Event e)
        {
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Create UIMultiDelegate Node"), false, CreateNode<UIMultiDelegateNode>, e);
            gm.AddItem(new GUIContent("Create Roslyn Node"), false, CreateNode<UIRoslynNode>, e);
            gm.AddItem(new GUIContent("Create Method Node"), false, CreateNode<UIMethodNode>, e);
            gm.AddItem(new GUIContent("Create Delegate Node"), false, CreateNode<UIDelegateNode>, e);
            gm.AddItem(new GUIContent("Create Input-Output Node"), false, CreateNode<UITransformationNode>, e);
            gm.AddItem(new GUIContent("Create Input Node"), false, CreateNode<UIInputNode>, e);
            gm.AddItem(new GUIContent("Create Display Node"), false, CreateNode<UIDisplayNode>, e);
            gm.AddItem(new GUIContent("Clear Nodes"), false, InitializeComponents);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateNode<T>(object userdata) where T : IDrawable
        {
            var pos = ((Event) userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth, NodeHeight));
            Nodes.Add((T) Activator.CreateInstance(typeof(T), rect));
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
                Connections.Add(new UIBezierConnection(@in, @out));
            }

            CurrentSendingDrag = null;
            CurrentAcceptingDrag = null;
        }

        private void RemoveNode(IDrawable n)
        {
            Nodes.Remove(n);
        }

        private void InitializeComponents()
        {
            NodeWidth = 300;
            NodeHeight = 150;
            wantsMouseMove = true;
            CurrentAcceptingDrag = null;
            CurrentSendingDrag = null;
            Drag = Vector2.zero;
            Nodes = new List<IDrawable>();
            Connections = new List<IDrawable>();
            NodeEventSystem = new NodeWindowEventSystem();

            NodeEventSystem.OnContextClick += CreateContextMenu;
            ConnectionCreatedEvent = null;
            ConnectionCreatedEvent = OnConnectionCreated;
            OnConnectionCancelRequest = null;
            OnConnectionCancelRequest = RequestCancelConnection;
            NodeEventSystem.OnMouseUp += ClearDrag;
            NodeEventSystem.OnMouseDrag += OnDrag;
            typeof(EditorBaseWindow).GetMethod("ClearConsole", BindingFlags.Static | BindingFlags.NonPublic)
                ?.Invoke(null, null);

            NodeEventSystem.OnScrollWheel += OnScroll;
        }

        private void OnScroll(Event e)
        {
            Nodes?.ForEach(c => (c as UIElement).rect.size += Vector2.one * e.delta.y);
            GUI.changed = true;
        }

        private void OnDrag(Event e)
        {
            if (GUIUtility.hotControl != 0)
                return;
            Drag = e.delta;
            Nodes?.ForEach(c => (c as UIElement).rect.position += Drag);
            GUI.changed = true;
        }

        private void ClearDrag(Event e)
        {
            if (CurrentAcceptingDrag != null) return;
            CurrentAcceptingDrag = null;
            CurrentSendingDrag = null;
        }

        private void Save()
        {
            var n = new NodeEditorWindowSaveLoad {Nodes = Nodes, Connections = Connections};

            var json = JsonConvert.SerializeObject(n, _settings);
            File.WriteAllText(_path, json);
        }

        private void Load()
        {
            InitializeComponents();
            var json = File.ReadAllText(_path);
            var n = JsonConvert.DeserializeObject<NodeEditorWindowSaveLoad>(json, _settings);
            Nodes = n.Nodes;
            Connections = n.Connections;
        }
    }

    [Serializable]
    public class NodeEditorWindowSaveLoad //just for saving
    {
        public List<IDrawable> Nodes { get; set; }
        public List<IDrawable> Connections { get; set; }
    }
}
using ChuTools.Extensions;
using ChuTools.NodeEditor.Controller;
using ChuTools.NodeEditor.EventSystems;
using ChuTools.NodeEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

namespace ChuTools.NodeEditor.View
{
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class NodeEditor : EditorWindow
    {
        public static Vector2 Drag;

        private Vector2 _offset;

        public List<IDrawable> Connections = new List<IDrawable>();
        public List<IDrawable> Nodes = new List<IDrawable>();

        public UIOutConnectionPoint CurrentSendingDrag =>
            DragAndDrop.GetGenericData("UIOutConnectionPoint") as UIOutConnectionPoint;

        public UIInConnectionPoint CurrentAcceptingDrag =>
            DragAndDrop.GetGenericData("UIInConnectionPoint") as UIInConnectionPoint;

        public int NodeHeight { get; set; }
        public int NodeWidth { get; set; }
        public static IEventSystem NodeEventSystem { get; private set; }
        public Vector2 CenterWindow => new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        private string _path => Application.dataPath + "ChuTools/Editor/nodes.json";

        [MenuItem("Tools/ChuTools/NodeWindow %g")]
        private static void Init()
        {
            var window = GetWindow<NodeEditor>();
            window.InitializeComponents();
            window.ShowTab();
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

            NodeEventSystem?.PollEvents(Event.current);

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawMenu();

            DrawConnection();
            Nodes.ForEach(n => n.Draw());
            Connections.ForEach(c => c.Draw());

            if (GUI.changed)
                Repaint();
        }

        private void DrawConnection()
        {
            if (CurrentSendingDrag == null) return;

            var endRect = new Rect(Current.mousePosition, new Vector2(10, 10));
            Handles.DrawSolidRectangleWithOutline(endRect, Color.cyan, Color.black);
            Chutilities.DrawNodeCurve(CurrentSendingDrag.Rect, endRect);
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
            gm.AddItem(new GUIContent("Connections/Create InConnection"), false,
                CreateUIConnection<UIInConnectionPoint>, e);
            gm.AddItem(new GUIContent("Connections/Create OutConnection"), false,
                CreateUIConnection<UIOutConnectionPoint>, e);
            gm.AddItem(new GUIContent("Nodes/Create MultiDelegate Node"), false, CreateNode<UIMultiDelegateNode>, e);
            //gm.AddItem(new GUIContent("Nodes/Create Roslyn Node"), false, CreateNode<UIRoslynNode>, e);
            gm.AddItem(new GUIContent("Nodes/Create Method Node"), false, CreateNode<UIMethodNode>, e);
            gm.AddItem(new GUIContent("Nodes/Create Delegate Node"), false, CreateNode<UIDelegateNode>, e);
            gm.AddItem(new GUIContent("Nodes/Create Input-Output Node"), false, CreateNode<UITransformationNode>, e);
            gm.AddItem(new GUIContent("Nodes/Create Input Node"), false, CreateNode<UIInputNode>, e);
            gm.AddItem(new GUIContent("Nodes/Create Display Node"), false, CreateNode<UIDisplayNode>, e);
            gm.AddItem(new GUIContent("Clear Nodes"), false, InitializeComponents);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateUIConnection<T>(object userdata) where T : IDrawable
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(15, 15));
            Nodes.Add((T)Activator.CreateInstance(typeof(T), rect));
        }

        private void CreateNode<T>(object userdata) where T : IDrawable
        {
            var pos = ((Event)userdata).mousePosition;
            var rect = new Rect(pos, new Vector2(NodeWidth, NodeHeight));
            Nodes.Add((T)Activator.CreateInstance(typeof(T), rect));
        }

        /// <summary>
        ///     when a connection is created add it to the connections list to draw
        /// </summary>
        /// <param name="out">the ui element associated with the out connection</param>
        /// <param name="in">the ui element associated with the in connection</param>
        private void OnConnectionCreated(Event e)
        {
            var outdrop = DragAndDrop.GetGenericData("UIOutConnectionPoint") as UIOutConnectionPoint;
            var indrop = DragAndDrop.GetGenericData("UIInConnectionPoint") as UIInConnectionPoint;
            if (indrop == null || outdrop == null)
            {
                DragAndDrop.PrepareStartDrag();
                return;
            }

            Connections.Add(new UIBezierConnection(outdrop, indrop));
            DragAndDrop.PrepareStartDrag();
        }

        private void OnConnectionRemove(UIInConnectionPoint @in)
        {
            if (Connections.Contains(@in))
                Connections.Remove(@in);
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
            DragAndDrop.PrepareStartDrag();
            Drag = Vector2.zero;
            Nodes = new List<IDrawable>();
            Connections = new List<IDrawable>();
            NodeEventSystem = new NodeWindowEventSystem();
            NodeEventSystem.OnContextClick += CreateContextMenu;
            NodeEventSystem.OnMouseDrag += OnDrag;
            NodeEventSystem.OnDragExited += OnConnectionCreated;
            NodeEventSystem.OnScrollWheel += OnScroll;
        }

        private void OnScroll(Event e)
        {
            Nodes?.ForEach(c => (c as UIElement).Rect.size += Vector2.one * e.delta.y);
            GUI.changed = true;
        }

        private void OnDrag(Event e)
        {
            if (GUIUtility.hotControl != 0)
                return;
            Drag = e.delta;
            Nodes?.ForEach(c => (c as UIElement).Rect.position += Drag);
            GUI.changed = true;
        }
    }
}
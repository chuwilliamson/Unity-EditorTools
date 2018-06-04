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
        public static System.Action<UIOutConnectionPoint, UIInConnectionPoint> ConnectionCreatedEvent;
        public static UIOutConnectionPoint CurrentSendingDrag { get; set; }
        public static UIInConnectionPoint CurrentAcceptingDrag { get; set; }

        public static void RequestConnection(UIOutConnectionPoint @uiOut, IConnectionOut @out)
        {
            if (CurrentAcceptingDrag.ValidateConnection(@out))
                ConnectionCreatedEvent.Invoke(CurrentSendingDrag, CurrentAcceptingDrag);
            else
            {
                Debug.Log("cancel connection request");
            }
        }

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
            Nodes = new List<IDrawable>();
            Connections = new List<IDrawable>();
            NodeEvents = new NodeWindowEventSystem();

            NodeEvents.OnContextClick += CreateContextMenu;
            ConnectionCreatedEvent += OnConnectionCreated;
            NodeEvents.OnMouseUp += e =>
            {
                if(CurrentAcceptingDrag != null) return;
                CurrentAcceptingDrag = null;
                CurrentSendingDrag = null;
            };
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

        private static void DrawConnection()
        {
            if (CurrentSendingDrag == null) return;
            Chutilities.DrawNodeCurve(CurrentSendingDrag.Rect, new Rect(Event.current.mousePosition, CurrentSendingDrag.Rect.size));
            var endRect = new Rect(Current.mousePosition, Vector2.one * 10);
            Handles.RectangleHandleCap(GUIUtility.GetControlID(FocusType.Passive, endRect), endRect.center,
                Quaternion.identity, 15, EventType.Repaint);
            GUI.changed = true;
        }

        private void CreateContextMenu(Event e)
        {
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Create Input-Output Node"), false, CreateNode, e);
            gm.AddItem(new GUIContent("Create Input Node"), false, CreateInputNode, e);
            gm.AddItem(new GUIContent("Create Display Node"), false, CreateDisplayNode, e);
            gm.AddItem(new GUIContent("Clear Nodes"), false, ClearNodes);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            Nodes.Add(new UINode(pos, new Vector2(300, 150)));
        }

        private void CreateDisplayNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            Nodes.Add(new UIDisplayNode(pos, new Vector2(300, 150)));
        }

        private void CreateInputNode(object userdata)
        {
            var pos = ((Event)userdata).mousePosition;
            Nodes.Add(new UIInputNode(pos, new Vector2(300, 150)));
        }

        private void OnConnectionCreated(UIOutConnectionPoint @out, UIInConnectionPoint @in)
        {
            CurrentSendingDrag = null;
            CurrentAcceptingDrag = null;
            if (@out != null && @in != null)
            {
                Connections.Add(new UIBezierConnection(@out, @in));
            }
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
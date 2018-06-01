using System;
using Interfaces;
using JeremyTools;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public partial class Node 
    {
        private readonly ConnectionPoint _in;
        private readonly Action<Connection> _onConnectionMade;
        private readonly ConnectionPoint _out;

        private int _dragcounter;


        public Node()
        {
            Rect = new Rect(new Vector2(0, 0), new Vector2(25, 25));
         
            Content = new GUIContent(ControlId.ToString());
            Style = NodeEditorWindow.NodeStyle;

            _in = new ConnectionPoint(new Vector2(Rect.x - 50, Rect.y), new Vector2(50, 50), ConnectionMade);
            Content = new GUIContent("in");

            _out = new ConnectionPoint(new Vector2(Rect.x + Rect.width + 5, Rect.y), new Vector2(50, 50),
                ConnectionMade) {Content = new GUIContent("out")};


            NodeEditorWindow.NodeEvents.OnMouseDown += OnMouseDown;
            NodeEditorWindow.NodeEvents.OnMouseUp += OnMouseUp;
            NodeEditorWindow.NodeEvents.OnMouseDrag += OnMouseDrag;
        }

        public Node(Vector2 position, Vector2 size, Action<Node> onRemoveNode, Action<Connection> onConnectionMade) :
            this(position, size, onRemoveNode)
        {
            _onConnectionMade = onConnectionMade;
        }

        public Node(Vector2 position, Vector2 size, Action<Node> onRemoveNode)
        {
            Rect = new Rect(position, size);
           
            Content = new GUIContent(ControlId.ToString());
            Style = NodeEditorWindow.NodeStyle;

            _in = new ConnectionPoint(new Vector2(Rect.x - 50, Rect.y), new Vector2(50, 50), null);
            _out = new ConnectionPoint(new Vector2(Rect.x + Rect.width + 5, Rect.y), new Vector2(50, 50), null);

            _onRemoveNodeAction = onRemoveNode;

            NodeEditorWindow.NodeEvents.OnMouseDown += OnMouseDown;
            NodeEditorWindow.NodeEvents.OnMouseUp += OnMouseUp;
            NodeEditorWindow.NodeEvents.OnMouseDrag += OnMouseDrag;
        }

        public Vector2 OutCenter => Rect.center;

        public Vector2 InCenter => _in.Rect.center;

        private void ConnectionMade(ConnectionPoint outPoint)
        {
            //if (_in.Rect.Overlaps(outPoint.Rect))
            //{
            //    Debug.Log("connection made");
            //    _onConnectionMade(new Connection(_in, outPoint));
            //}
        }

        public void OnMouseDown(Event e)
        {
            switch (e.button)
            {
                case 0:
                    if (Rect.Contains(e.mousePosition))
                    {
                        GUIUtility.hotControl = ControlId;
                        Style = NodeEditorWindow.SelectedNodeStyle;
                        GUI.changed = true;
                    }
                    break;
                case 1:
                    if (!Rect.Contains(e.mousePosition)) return;
                    var gm = new GenericMenu();
                    gm.AddItem(new GUIContent("Nodes/Remove"), false, OnRemoveNode, this);
                    gm.ShowAsContext();
                    GUI.changed = true;
                    break;
                case 2:
                    break;
            }
        }

        public void OnMouseUp(Event e)
        {
            if (GUIUtility.hotControl == ControlId)
                GUIUtility.hotControl = 0;
            GUI.changed = true;
        }

        public void OnMouseDrag(Event e)
        {
            if (GUIUtility.hotControl == ControlId)
            {
                _dragcounter++;
                Position += e.delta;
                _in.Position += e.delta;
                _out.Position += e.delta;
                GUI.changed = true;
                e.Use();
            }

        }

        public override void Draw()
        { 
            base.Draw();
            GUILayout.BeginArea(Rect);
            ControlId = GUIUtility.GetControlID(FocusType.Passive, Rect);
            GUILayout.Label(_dragcounter.ToString());
            GUILayout.EndArea();
            _in.Draw();
            _out.Draw();

        }

        private void OnRemoveNode(object n)
        {
            _onRemoveNodeAction?.Invoke(this);
        }

        public override string ToString()
        {
            return $"Node {ControlId}";
        }

        public int Value { get; set; }
    }
}
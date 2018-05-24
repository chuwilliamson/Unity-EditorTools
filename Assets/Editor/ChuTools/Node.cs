using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class Node : IDrawable
    {
        private readonly GUIStyle _normal = new GUIStyle("flow node 0");
        private readonly GUIStyle _selected = new GUIStyle("flow node 0 on");
        private GUIStyle _currentStyle;
        private IEventSystem _nodeEventSystem;

        public int Id;
        public Connection left;
        public Rect LeftRect;
        public int mousedowns = 0;

        public Rect Rect;
        public Connection right;
        public Rect RightRect;

        public Node()
        {
            LeftRect = new Rect(Rect.position, size: new Vector2(25, 25))
            {
                center = new Vector2(Rect.xMin, y: Rect.yMax / 2)
            };
            RightRect = new Rect(Rect.position, size: new Vector2(25, 25))
            {
                center = new Vector2(Rect.xMax, y: Rect.yMax / 2)
            };
        
            _currentStyle = _normal;
        }

        public Node(Vector2 position, int id) : this()
        {
            Rect = new Rect(position, size: new Vector2(150, 50));
            Id = id;
        }

        public IEventSystem NodeEventSystem
        {
            get { return _nodeEventSystem; }
            set
            {
                _nodeEventSystem = value;
                _nodeEventSystem.OnMouseUp += OnMouseUp;
                _nodeEventSystem.OnMouseDown += OnMouseDown;
                _nodeEventSystem.OnMouseDrag += OnMouseDrag;
                _nodeEventSystem.OnContextClick += OnContextClick;
            }
        }

        public void Draw(Event e)
        {
            var guistyle = new GUIStyle { normal = { textColor = Color.white } };
            var botrect = Rect;
            botrect.position = new Vector2(Rect.position.x, y: Rect.position.y + Rect.height);
            GUI.Box(botrect, text: Rect.position.ToString(), style: guistyle);

            LeftRect = new Rect(Rect.position, size: new Vector2(25, 25))
            {
                center = new Vector2(Rect.xMin, y: Rect.yMax - Rect.height / 2)
            };

            RightRect = new Rect(Rect.position, size: new Vector2(25, 25))
            {
                center = new Vector2(Rect.xMax, y: Rect.yMax - Rect.height / 2)
            };

            GUI.Box(Rect, content: new GUIContent { text = Rect.position.ToString() }, style: _currentStyle);
            GUI.Box(LeftRect, "l", _currentStyle);
            GUI.Box(RightRect, "r", _currentStyle);
            right?.Draw(Event.current);
        }


        public void OnMouseDrag(Event e)
        {
            var newposition = Rect.position + e.delta;

            if (newposition.x < 0) //left
                return;
            if (newposition.y < 0) //top
                return;
            if (newposition.x > Screen.width - Rect.width) //right
                return;
            if (newposition.y > Screen.height - Rect.height) //bottom
                return;

            if (NodeEventSystem.Selected == this)
            {
                Rect.position = newposition;
                e.Use();
            }
        }

        public void OnMouseDown(Event e)
        {
            if (e.button == 0)
            {
                if (Rect.Contains(e.mousePosition))
                    if (NodeEventSystem.Selected == null)
                    {
                        NodeEventSystem.Selected = this;
                        e.Use();
                    }
                    else if (NodeEventSystem.Selected == this)
                    {
                    }
                if (!Rect.Contains(e.mousePosition) && NodeEventSystem.Selected == this)
                    NodeEventSystem.Selected = null;
            }
            if (e.button == 1)
            {
            }
            _currentStyle = NodeEventSystem.Selected == this ? _selected : _normal;
        }

        public void OnMouseUp(Event e)
        {
        }

        private void OnContextClick(Event e)
        {
            if (!Rect.Contains(e.mousePosition))
                return;

            var pos = e.mousePosition;
            var gm = new GenericMenu();
            gm.AddItem(content: new GUIContent("Create Connection"), on: false, func: () => { CreateConnection(pos); });
            gm.AddItem(content: new GUIContent("Clear Connections"), on: false, func: ClearConnection);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateConnection(Vector2 pos)
        {
            right = new Connection(ref RightRect, NodeEventSystem);
            ClearConnection();

        }

        private void ClearConnection()
        {
            left = null;
            right = null;
        }

        public override string ToString()
        {
            return string.Format("Node {0}", Id);
        }
    }
}
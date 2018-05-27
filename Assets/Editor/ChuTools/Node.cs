using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class Node : IDrawable
    {
        private readonly GUIStyle _normal = new GUIStyle("flow node 0") { normal = { textColor = Color.white } };
        private readonly GUIStyle _selectedStyle = new GUIStyle("flow node 0 on") { normal = { textColor = Color.green } };
        private GUIStyle _currentStyle;
        private readonly IEventSystem _nodeEventSystem;

        public int Id;
        private Node RightNode;
        public Rect CenterRect;
        public Rect RightRect;
        public Connection RightConnection;

        public Node()
        {
            RightRect = new Rect(CenterRect.position, size: new Vector2(25, 25))
            {
                center = new Vector2(CenterRect.xMax, y: CenterRect.yMax / 2)
            };

            _currentStyle = _normal;
        }

        public Node(Vector2 position, int id, IEventSystem eventSystem) : this()
        {
            CenterRect = new Rect(position, size: new Vector2(150, 150));
            Id = id;
            _nodeEventSystem = eventSystem;
            _nodeEventSystem.OnMouseDown += OnMouseDown;
            _nodeEventSystem.OnContextClick += OnContextClick;
            _nodeEventSystem.OnMouseMove += OnMouseMove;
            _nodeEventSystem.OnMouseDrag += OnMouseDrag;
        }

        public void Draw(Event e)
        {
            RightRect.center = new Vector2(CenterRect.xMax, y: CenterRect.yMax - CenterRect.height / 2);
            GUI.Box(CenterRect, content: new GUIContent { text = CenterRect.position.ToString() }, style: _currentStyle);
            GUI.Box(RightRect, content: new GUIContent { text = "r" }, style: _currentStyle);
            if (RightConnection != null)
                RightConnection.Draw(Event.current);
        }

        private void OnMouseMove(Event e)
        {
            if (OutsideRect) return;
            _nodeEventSystem.WillSelect = this;
            GUI.changed = true;
        }


        private void OnMouseDown(Event e)
        {
            if (InsideRect && _nodeEventSystem.WillSelect == this)
            {
                _nodeEventSystem.Selected = this;
                _currentStyle = _selectedStyle;
                GUI.changed = true;
                e.Use();
            }
        }

        private void OnMouseDrag(Event e)
        {
            if (_nodeEventSystem.Selected != this) return;
            var newposition = CenterRect.position + e.delta;
            if (newposition.x < 0 && newposition.y < 0) //left && top
                return;
            if (newposition.x > Screen.width - CenterRect.width) //right
                return;
            if (newposition.y > Screen.height - CenterRect.height) //bottom
                return;

            CenterRect.position = newposition;
            e.Use();
        }

        private bool InsideRect
        {
            get { return CenterRect.Contains(Event.current.mousePosition); }
        }

        private bool OutsideRect
        {
            get { return !InsideRect; }
        }

        private void OnContextClick(Event e)
        {
            if (OutsideRect)
                return;

            var pos = Event.current.mousePosition;
            var gm = new GenericMenu();
            gm.AddItem(content: new GUIContent("Create Connection"), on: false, func: CreateConnection);
            gm.AddItem(content: new GUIContent("Clear Connections"), on: false, func: ClearConnection);
            gm.ShowAsContext();
        }

        private void CreateConnection()
        {
            //delegate (Node node) { RightNode = node; }
            RightConnection = new Connection(node: this, eventSystem: _nodeEventSystem, onConnectionMade: SetConnection);
        }

        private void SetConnection(Node node)
        {
            RightNode = node;
        }
        private void ClearConnection()
        {
            RightConnection = null;
        }

        public override string ToString()
        {
            return string.Format("Node {0}", Id);
        }
    }
}
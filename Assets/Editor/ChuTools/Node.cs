using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class Node : IDrawable
    {
        private readonly GUIStyle _normal = new GUIStyle("flow node 0") {normal = {textColor = Color.white}};
        private readonly GUIStyle _selectedStyle = new GUIStyle("flow node 0 on") {normal = {textColor = Color.green}};
        private GUIStyle _currentStyle;
        private readonly IEventSystem _nodeEventSystem;

        public int Id;

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
            CenterRect = new Rect(position, size: new Vector2(150, 50));
            Id = id;

            _nodeEventSystem = eventSystem;
            _nodeEventSystem.OnMouseDown += OnMouseDown;
            _nodeEventSystem.OnMouseDrag += OnMouseDrag;
            _nodeEventSystem.OnContextClick += OnContextClick;
            _nodeEventSystem.OnMouseMove += OnMouseMove;
        }

        private void DrawNodeCurve(Rect start, Rect end)
        {
            var startPos = new Vector3(x: start.x + start.width, y: start.y + start.height / 2, z: 0);
            var endPos = new Vector3(end.x, y: end.y + end.height / 2, z: 0);
            var startTan = startPos + Vector3.right * 50;
            var endTan = endPos + Vector3.left * 50;
            var shadowCol = new Color(0, 0, 0, 0.06f);
            for (var i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, width: (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        }

        public void OnMouseMove(Event e)
        {
            if (InsideRect)
                _nodeEventSystem.WillSelect = this;
        }

        public void Draw(Event e)
        {
            RightRect.center = new Vector2(CenterRect.xMax, y: CenterRect.yMax - CenterRect.height / 2);
            CenterRect=GUI.Window(Id, CenterRect, OnNodeGUI, title: new GUIContent(text: "Node " + Id), style: _currentStyle);
        }

        public void OnNodeGUI(int id)
        {
            GUI.DragWindow();
            GUI.Box(CenterRect, content: new GUIContent {text = CenterRect.position.ToString()}, style: _currentStyle);
            GUI.Box(RightRect, content: new GUIContent {text = "r"}, style: _currentStyle);
            RightConnection?.Draw(Event.current);
        }

        public void OnMouseDrag(Event e)
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

        public void OnMouseDown(Event e)
        {
            if (InsideRect && _nodeEventSystem.WillSelect == this)
            {
                _nodeEventSystem.Selected = this;
                _currentStyle = _selectedStyle;
                e.Use();
            }

            if (OutsideRect && _nodeEventSystem.Selected == this)
            {
                _nodeEventSystem.Selected = null;
                _currentStyle = _normal;
            }
        }

        private bool InsideRect => CenterRect.Contains(Event.current.mousePosition);

        private bool OutsideRect => !InsideRect;

        private void OnContextClick(Event e)
        {
            if (OutsideRect)
                return;

            var pos = e.mousePosition;
            var gm = new GenericMenu();
            gm.AddItem(content: new GUIContent("Create Connection"), on: false, func: CreateConnection);
            gm.AddItem(content: new GUIContent("Clear Connections"), on: false, func: ClearConnection);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateConnection()
        {
            RightConnection = new Connection(node: this, eventSystem: _nodeEventSystem);
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
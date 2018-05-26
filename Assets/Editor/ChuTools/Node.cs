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

        public Rect NodeRect; 
        public Rect RightRect;

        public Connection RightConnection; 

        public Node()
        {
 
            RightRect = new Rect(position: NodeRect.position, size: new Vector2(25, 25))
            {
                center = new Vector2(x: NodeRect.xMax, y: NodeRect.yMax / 2)
            };

            _currentStyle = _normal;
        }

        public Node(Vector2 position, int id, IEventSystem eventSystem) : this()
        {
            NodeRect = new Rect(position: position, size: new Vector2(150, 50));
            Id = id;

            _nodeEventSystem = eventSystem;
            _nodeEventSystem.OnMouseUp += OnMouseUp;
            _nodeEventSystem.OnMouseDown += OnMouseDown;
            _nodeEventSystem.OnMouseDrag += OnMouseDrag;
            _nodeEventSystem.OnContextClick += OnContextClick;
            _nodeEventSystem.OnMouseMove += OnMouseMove;
            _nodeEventSystem.OnUsed += OnUsed;
        }

        public void OnMouseMove(Event e)
        {
            UnityEngine.Debug.Log("movve");
            if (NodeRect.Contains(point: e.mousePosition))
            {
                _nodeEventSystem.WillSelect = this;
            }
        }

        public void Draw(Event e)
        {
            RightRect.center = new Vector2(x: NodeRect.xMax, y: NodeRect.yMax - NodeRect.height / 2);

            GUI.Box(NodeRect, new GUIContent { text = NodeRect.position.ToString() }, style: _currentStyle);
            GUI.Box(RightRect, new GUIContent { text = "r" }, style: _currentStyle);

            
            RightConnection?.Draw(e: Event.current);
        }

        public void OnMouseDrag(Event e)
        {
            if (_nodeEventSystem.Selected != this) return;
            var newposition = NodeRect.position + e.delta;
            if (newposition.x < 0 && newposition.y < 0) //left && top
                return;
            if (newposition.x > Screen.width - NodeRect.width) //right
                return;
            if (newposition.y > Screen.height - NodeRect.height) //bottom
                return;

            NodeRect.position = newposition;
            e.Use();
        }

        public void OnMouseDown(Event e)
        {
            if (!NodeRect.Contains(point: e.mousePosition) && _nodeEventSystem.Selected == this)
                _nodeEventSystem.Selected = null;

            if (NodeRect.Contains(point: e.mousePosition))
            {
                _nodeEventSystem.Selected = this;
                _currentStyle = _selectedStyle;
                e.Use();
            }
        }

        public void OnUsed(Event e)
        {
        }

        public void OnMouseUp(Event e)
        {
        }

        void OnContextClick(Event e)
        {
            if (!NodeRect.Contains(point: e.mousePosition))
                return;

            var pos = e.mousePosition;
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Create Connection"), false, () => { CreateConnection(pos: pos); });
            gm.AddItem(new GUIContent("Clear Connections"), false, func: ClearConnection);
            gm.ShowAsContext();
            e.Use();
        }

        void CreateConnection(Vector2 pos)
        {
            RightConnection = new Connection(this, rect: RightRect, eventSystem: _nodeEventSystem);
        }

        void ClearConnection()
        {
            RightConnection = null;
        }

        public override string ToString()
        {
            return string.Format("Node {0}", arg0: Id);
        }
    }
}
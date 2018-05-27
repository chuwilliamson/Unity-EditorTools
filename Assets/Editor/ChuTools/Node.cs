using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class Node : IDrawable
    {
        private readonly GUIStyle _normal = new GUIStyle("flow node 0")
        {
            normal = { textColor = Color.white },
            margin = new RectOffset { left = 15, right = 15, bottom = 15, top = 1 }

        };
        private readonly GUIStyle _selectedStyle = new GUIStyle("flow node 0 on") { normal = { textColor = Color.green } };
        private GUIStyle _currentStyle;
        private Vector2 _scrollPosition;
        private readonly IEventSystem _nodeEventSystem;

        public int Id;
        public Object Data { get; set; }

        public Node RightNode { get; set; }
        public Connection RightConnection { get; set; }
        public Connection LeftConnection { get; set; }

        public Rect CenterRect;

        public Node()
        {
            _currentStyle = _normal;

        }

        public Node(Vector2 position, int id, IEventSystem eventSystem) : this()
        {
            CenterRect = new Rect(position, size: new Vector2(250, 250));
            Id = id;
            _nodeEventSystem = eventSystem;
            _nodeEventSystem.OnMouseDown += OnMouseDown;
            _nodeEventSystem.OnContextClick += OnContextClick;
            _nodeEventSystem.OnMouseMove += OnMouseMove;
            _nodeEventSystem.OnMouseDrag += OnMouseDrag;
        }

        public Rect GetRectOffset(Rect r, Vector2 width, Vector2 height)
        {
            var offrect = new Rect(r);
            offrect.xMin += width.x;
            offrect.xMax -= width.y;
            offrect.yMin += height.x;
            offrect.yMax -= height.y;
            return offrect;
        }

        public void Draw(Event e)
        {
         
            //draw the boxes
            var toprect =
                new Rect(CenterRect)
                {
                    position = new Vector2(CenterRect.position.x, CenterRect.position.y - 30),
                    size =  new Vector2(CenterRect.width, 25)
                };

            GUI.Button(toprect, text: "ID:" + Id, style: _currentStyle);
            GUI.Box(CenterRect, content: GUIContent.none, style: _currentStyle);



            //draw inside the boxes
            var padrect = GetRectOffset(CenterRect, width: new Vector2(5, 5), height: new Vector2(25, 5));

            GUILayout.BeginArea(padrect);
            Data = EditorGUILayout.ObjectField(Data as DialogueRootObject, typeof(DialogueRootObject), false);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);



            if (Data != null)
            {
                var so = new SerializedObject(Data);
                var sp = so.FindProperty("Conversation");
                var rp = sp.FindPropertyRelative("DialogueNodes");
                EditorGUILayout.PropertyField(property: rp, includeChildren: true);
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();


            if (RightConnection != null)
                RightConnection.Draw(Event.current);
        }

        #region callbacks

        private void OnMouseMove(Event e)
        {
            if (InsideRect)
                _nodeEventSystem.WillSelect = this;

            if (OutsideRect && _nodeEventSystem.WillSelect == this)
                _nodeEventSystem.WillSelect = null;

        }


        private void OnMouseDown(Event e)
        {
            if (OutsideRect)
            {
                _nodeEventSystem.Release(obj: this);
                e.Use();

            }
            else
            {
                _nodeEventSystem.SetSelected(this);
                e.Use();
            }
            _currentStyle = _nodeEventSystem.Selected == this ? _selectedStyle : _normal;

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

            CenterRect.position += e.delta;
            e.Use();
        }


        private void OnContextClick(Event e)
        {
            if (OutsideRect)
                return;

            var gm = new GenericMenu();
            gm.AddItem(content: new GUIContent("Create Connection"), on: false, func: CreateConnection);
            gm.AddItem(content: new GUIContent("Clear Connections"), on: false, func: ClearConnection);
            gm.ShowAsContext();
        }

        #endregion

        private void CreateConnection()
        {
            RightConnection =
                new Connection(node: this, eventSystem: _nodeEventSystem, onConnectionMade: SetConnection);
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

        private bool InsideRect
        {
            get { return CenterRect.Contains(Event.current.mousePosition); }
        }

        private bool OutsideRect
        {
            get { return !InsideRect; }
        }
    }
}
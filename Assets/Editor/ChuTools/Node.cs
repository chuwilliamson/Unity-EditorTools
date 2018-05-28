using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChuTools
{
    [System.Serializable]
    public class Node
    {
        private readonly GUIStyle _normalStyle = new GUIStyle("flow node 0")
        {
            normal = { textColor = Color.white },
            margin = new RectOffset { left = 15, right = 15, bottom = 15, top = 1 }
        };

        private readonly GUIStyle _selectedStyle = new GUIStyle("flow node 0 on")
        {
            normal = { textColor = Color.green },
            margin = new RectOffset { left = 15, right = 15, bottom = 15, top = 1 }
        };

        private GUIStyle _currentStyle;
        private Vector2 _scrollPosition;
        private IEventSystem _nodeEventSystem;

        public int Id;
        public Object Data { get; set; }

        public Connection RightConnection { get; set; }
        public Connection LeftConnection { get; set; }

        private readonly Action<Node> _onRemoveNodeAction;

        public Rect CenterRect;

        public Node(Vector2 position, Vector2 size, int id, Action<Node> onRemoveNode)
        {
            Id = id;
            CenterRect = new Rect(position, size);
            _onRemoveNodeAction = onRemoveNode;
            _currentStyle = _normalStyle;
            _nodeEventSystem = new EditorEventSystem();
            _nodeEventSystem.OnMouseDown += OnMouseDown;
            _nodeEventSystem.OnContextClick += OnContextClick;
            _nodeEventSystem.OnMouseMove += OnMouseMove;
            _nodeEventSystem.OnMouseDrag += OnMouseDrag;
        }

        ~Node()
        {
            Debug.Log(message: "deleted" + Id);
        }

        public void PollEvents(Event e)
        {
            _nodeEventSystem.PollEvents(e);
        }

        public void Draw()
        {
            //draw the boxes
            var toprect = new Rect(CenterRect)
            {
                position = new Vector2(CenterRect.position.x, y: CenterRect.position.y - 30),
                size = new Vector2(CenterRect.width, 25)
            };

            GUI.Box(toprect, text: "ID:" + Id, style: _currentStyle);
            GUI.Box(CenterRect, GUIContent.none, _currentStyle);


            //draw inside the boxes
            var padrect = Chutilities.GetRectOffset(CenterRect, width: new Vector2(5, 5), height: new Vector2(25, 5));

            GUILayout.BeginArea(padrect);
            Data = EditorGUILayout.ObjectField(obj: Data as DialogueRootObject, objType: typeof(DialogueRootObject),
                allowSceneObjects: false);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);


            if (Data != null)
            {
                var so = new SerializedObject(Data);
                var sp = so.FindProperty("Conversation");
                var rp = sp.FindPropertyRelative("DialogueNodes");
                EditorGUILayout.PropertyField(rp, true);
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();


            RightConnection?.Draw(Event.current);
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
                _currentStyle = _normalStyle;
                return;
            }
            if (e.button == 1)
            {
                var gm = new GenericMenu();
                gm.AddItem(content: new GUIContent("Create Connection"), on: false, func: CreateConnection);
                gm.AddItem(content: new GUIContent("Clear Connections"), on: false, func: ClearConnections);
                gm.AddItem(content: new GUIContent("Remove Node"), on: false, func: () => { OnRemoveNode(n: this); });
                gm.ShowAsContext();
            }

            _currentStyle = _selectedStyle;
            e.Use();
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
            if (InsideRect)
            {

            }
        }

        #endregion

        private void OnRemoveNode(Node n)
        {
            if (_onRemoveNodeAction != null)
            {
                _onRemoveNodeAction(obj: this);
                RightConnection = null;
                LeftConnection = null;
                _nodeEventSystem = null;
            }
        }

        private void CreateConnection()
        {
            RightConnection = new Connection(node: this, eventSystem: _nodeEventSystem,
                onConnectionComplete: SetConnection);
        }

        private void SetConnection(Node node)
        {
        }

        private void ClearConnections()
        {
            RightConnection = null;
            LeftConnection = null;
        }

        public override string ToString()
        {
            return string.Format("Node {0}", Id);
        }

        private bool InsideRect
        {
            get { return CenterRect.Contains(Event.current.mousePosition, true); }
        }

        private bool OutsideRect
        {
            get { return !InsideRect; }
        }
    }
}
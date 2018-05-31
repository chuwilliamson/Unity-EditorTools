using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


namespace ChuTools
{
    [System.Serializable]
    public class Node
    {
        private readonly List<bool> _toggles = new List<bool>() { false, false, false };
        public List<string> names = new List<string>() { "bob", "two", "80" };

        public bool IsSelected { get; set; }
        public bool IsHovered { get; set; }

        private readonly GUIStyle _normalStyle = new GUIStyle("flow node 0")
        {
            normal = { textColor = Color.white },
            padding = new RectOffset(5, 5, 5, 5)

        };

        private readonly GUIStyle _topStyle = new GUIStyle("flow node 0")
        {
            normal = { textColor = Color.white },
            alignment = TextAnchor.UpperLeft
        };

        private readonly GUIStyle _selectedStyle = new GUIStyle("flow node 0 on")
        {
            normal = { textColor = Color.green },
            padding = new RectOffset(5, 5, 5, 5)
        };

        private GUIStyle _currentStyle;
        private Vector2 _scrollPosition;

        public int Id;
        public Object Data;

        public Connection RightConnection { get; set; }
        public Connection LeftConnection { get; set; }

        private readonly Action<Node> _onRemoveNodeAction;
        public Rect BackgroundRect;
        public Node(Vector2 position, Vector2 size, int id, Action<Node> onRemoveNode)
        {
            Id = id;
            BackgroundRect = new Rect(position, size);
            _onRemoveNodeAction = onRemoveNode;
            _currentStyle = _normalStyle;

        }
        private int controlId;
        private int _propid;
        public void PollEvents()
        {
            controlId = GUIUtility.GetControlID(FocusType.Passive);
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (BackgroundRect.Contains(Event.current.mousePosition) && Event.current.button == 0)
                    {
                        GUIUtility.hotControl = controlId;
                        IsSelected = true;
                        _currentStyle = _selectedStyle;
                    }

                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlId)
                    {
                        GUIUtility.hotControl = 0;
                    }

                    break;
                case EventType.MouseDrag:
                    OnMouseDrag(Event.current);
                    break;
                case EventType.ContextClick:
                    OnContextClick(Event.current);
                    break;

            }
 
        }


        public void Draw()
        {
            //draw the boxes  
            GUI.Box(BackgroundRect, GUIContent.none, _normalStyle);
            GUILayout.BeginArea(BackgroundRect);

            GUILayout.Box("ID:" + Id, _normalStyle, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(15));
            for (int i = 0; i < names.Count; i++)
            {
                _toggles[i] = EditorGUILayout.ToggleLeft(new GUIContent(names[i]), _toggles[i]);
            }
            GUILayout.Label(new GUIContent("control id:: " + controlId));
            GUILayout.Label(new GUIContent("prop id:: " + _propid));
            Data = EditorGUILayout.ObjectField(new GUIContent("data") { tooltip = GUILayoutUtility.GetLastRect().ToString() }, Data, typeof(DialogueRootObject), false,GUILayout.Height(25));
            EditorGUILayout.RectField(GUILayoutUtility.GetLastRect());


            if (Data != null)
            {
                var so = new SerializedObject(Data);
                var sp = so.FindProperty("Conversation");
                var rp = sp.FindPropertyRelative("DialogueNodes");
                
                if (EditorGUILayout.PropertyField(rp, true))
                {
                    
                }
                
                
            }

            GUILayout.EndArea();

        }

        #region callbacks

        private void OnMouseDown(Event e)
        {

        }

        private void OnMouseDrag(Event e)
        {
            if (GUIUtility.hotControl != controlId)
                return;
            var newposition = BackgroundRect.position + e.delta;
            if (newposition.x < 0 || newposition.y < 0) //left && top
                return;
            if (newposition.x + BackgroundRect.width > Screen.width) //right
                return;
            if (newposition.y + BackgroundRect.height > Screen.height) //bottom
                return;

            BackgroundRect.position += e.delta;
            e.Use();
        }

        private void OnContextClick(Event e)
        {
            if (GUIUtility.hotControl != controlId) return;
            var gm = new GenericMenu();
            //gm.AddItem(content: new GUIContent("Create Connection"), on: false, func: CreateConnection);
            gm.AddItem(content: new GUIContent("Clear Connections"), on: false, func: ClearConnections);
            gm.AddItem(content: new GUIContent("Remove Node"), on: false, func: () => { OnRemoveNode(n: this); });
            gm.ShowAsContext();
        }

        #endregion

        private void OnRemoveNode(Node n)
        {
            if (_onRemoveNodeAction == null) return;
            _onRemoveNodeAction(obj: this);
            RightConnection = null;
            LeftConnection = null;
        }

        private void CreateConnection()
        {
            RightConnection = new Connection(this, SetConnection, new NodeWindowEventSystem());
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
    }
}
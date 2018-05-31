using System;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public partial class Node
    {
        private readonly ConnectionPoint _rPoint;
       // private readonly ConnectionPoint _lPoint;

        public Node(Vector2 position, Vector2 size, int id, Action<Node> onRemoveNode)
        {
            Id = id;
            _Rect = new Rect(position, size);
          //  _lPoint = new ConnectionPoint(new Vector2(_Rect.x - 30, _Rect.y), new Vector2(30, 30), OnConnectionComplete, this);
            _rPoint = new ConnectionPoint(new Vector2(_Rect.x + _Rect.width, _Rect.y), new Vector2(30, 30), OnConnectionComplete);

            _onRemoveNodeAction = onRemoveNode;
            _currentStyle = NormalStyle;
            _currentStyle.fontSize = NormalStyle.fontSize;
        }


        public override void PollEvents(Event e)
        {
            _rPoint.PollEvents(e);
         //   _lPoint.PollEvents(e);

            _controlId = GUIUtility.GetControlID(FocusType.Passive);

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (_Rect.Contains(e.mousePosition))
                        {
                            GUIUtility.hotControl = _controlId;
                            IsSelected = true;
                            _currentStyle = SelectedStyle;
                            e.Use();
                        }
                    }
                    else if (e.button == 1)
                    {
                        if (!_Rect.Contains(e.mousePosition)) return;
                        var gm = new GenericMenu();
                        gm.AddItem(content: new GUIContent("Remove Node"), on: false, func: () => { OnRemoveNode(n: this); });
                        gm.ShowAsContext();
                        e.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (IsSelected) IsSelected = false;
                    break;

                case EventType.MouseDrag:
                    if (!IsSelected) return;
                    _Rect.position += e.delta;
                    _rPoint.Position += e.delta;
                 //   _lPoint.Position += e.delta;
                    e.Use();
                    break;
            }
        }

        public override void Draw()
        {
            _rPoint.Draw();
          //  _lPoint.Draw();
            GUI.Box(_Rect, GUIContent.none, NormalStyle);
        }

        private void OnRemoveNode(Node n)
        {
            _onRemoveNodeAction?.Invoke(obj: this);
        }

        public override string ToString()
        {
            return string.Format("Node {0}", Id);
        }

        public void OnConnectionComplete(ConnectionPoint connectionPoint)
        {
            if(connectionPoint == _rPoint)
                return;
            if(connectionPoint.CreateConnection(_rPoint))
            {
                Debug.Log(string.Format("Node {0} connected {1} with {2}", Id, _rPoint, connectionPoint));
            }
        }
    }

}
using System;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public partial class Node
    {
        private readonly ConnectionPoint _rPoint;

        public Node(Vector2 position, Vector2 size, int id, Action<Node> onRemoveNode)
        {
            Id = id;
            _Rect = new Rect(position, size);
            _rPoint = new ConnectionPoint(new Vector2(_Rect.x + _Rect.width, _Rect.y), new Vector2(30, 30), OnConnectionComplete);

            _onRemoveNodeAction = onRemoveNode;
            _currentStyle = NormalStyle;
            _currentStyle.fontSize = NormalStyle.fontSize;
            NodeEditorWindow.NodeEvents.OnMouseDown += OnMouseDown;
            NodeEditorWindow.NodeEvents.OnMouseUp += OnMouseUp;
            NodeEditorWindow.NodeEvents.OnMouseDrag += OnMouseDrag;
        }

        public void OnMouseDown(Event e)
        {
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
                gm.AddItem(new GUIContent("Nodes/Remove"), false, OnRemoveNode, this);
                gm.ShowAsContext();
                e.Use();
            }
        }

        public void OnMouseUp(Event e)
        {
            if (!IsSelected) return;
                IsSelected = false;
        }

        public void OnMouseDrag(Event e)
        {
            if (!IsSelected)
                return;
            _Rect.position += e.delta;
            _rPoint.Position += e.delta;

            e.Use();
        }
 
        public override void Draw()
        {
            _rPoint.Draw();
            GUI.Box(_Rect, GUIContent.none, NormalStyle);
        }

        private void OnRemoveNode(object n)
        {
            _onRemoveNodeAction?.Invoke(this);
        }

        public override string ToString()
        {
            return $"Node {Id}";
        }

        public void OnConnectionComplete(ConnectionPoint connectionPoint)
        {
            if (connectionPoint == _rPoint)
                return;
            if (connectionPoint.CreateConnection(_rPoint))
            {
                Debug.Log($"Node {Id} connected {_rPoint} with {connectionPoint}");
            }
        }
    }

}
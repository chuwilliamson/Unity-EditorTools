using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class Connection : IDrawable
    {
        bool _active;
        Vector3 _end;
        IEventSystem _eventSystem;
        Node _inNode;
        Node _outNode;
        Rect _rect;
        Vector3 _start;

        public Connection(Node node, Rect rect, IEventSystem eventSystem)
        {
            _inNode = node;
            _rect = rect;
            _start = _rect.center;
            _end = _rect.center;
            _active = true;
            _eventSystem = eventSystem;
            _eventSystem.OnMouseDown += OnMouseDown;
        }

        public void Draw(Event e)
        {
            if (_active)
                _end = e.mousePosition;
            if (_outNode != null)
                _end = _outNode.NodeRect.position;
            
            Handles.DrawLine(p1: _inNode.RightRect.center, p2: _end);
        }

        void OnMouseDown(Event e)
        {
            _active = false;
            if (_eventSystem.WillSelect != null)
            {
                if (_eventSystem.WillSelect == typeof(Node))
                {
                    _outNode = _eventSystem.WillSelect as Node;

                }
            }
        }
    }
}
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
        public Rect _rect;

        Vector3 _start;

        public Connection(ref Rect rect, IEventSystem eventSystem)
        {
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
            Handles.DrawLine(p1: _rect.center, p2: _end);
        }

        void OnMouseDown(Event e)
        {
            _active = false;
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class Connection : IDrawable
    {
        private bool _active;
        
        private readonly IEventSystem _eventSystem;

        private Node _in;
        private Node _out;
        
        private Vector3 _start;
        private Vector3 _end;

        public Connection(Node node, IEventSystem eventSystem)
        {
            _in = node;
            _out = null;

            _start = node.RightRect.position;
            _end = _start;

            _active = true;
            _eventSystem = eventSystem;
            _eventSystem.OnMouseDown += OnMouseDown;
        }

        public void Draw(Event e)
        {
            Handles.DrawLine(_in.RightRect.center, _end);
        }

        private void OnMouseDown(Event e)
        {
            _active = false;
            if (_eventSystem.Selected != null && _eventSystem.Selected != _in)
            {
                _out = _eventSystem.Selected as Node;
                _end = _out.CenterRect.position;
            } 
        }
    }
}
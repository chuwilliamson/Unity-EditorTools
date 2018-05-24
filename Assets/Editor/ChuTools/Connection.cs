using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class Connection : IDrawable
    {
        private Vector3 _start;
        private Vector3 _end;

        private IEventSystem _eventSystem;
        private bool _active = false;
        private Rect _rect;
        
        public Connection(ref Rect rect,IEventSystem eventSystem)
        {
            _rect = rect;
            _start = _rect.center;
            _end = _rect.center;  
            _active = true;
            _eventSystem = eventSystem;
            _eventSystem.OnMouseDown += OnMouseUp;
        }
         
        private void OnMouseUp(Event e)
        {
            _active = false;
        }

        public void Draw(Event e)
        {
            if (_active)
                _end = e.mousePosition;
            Handles.DrawLine(_rect.center, _end);
        }
    }
}
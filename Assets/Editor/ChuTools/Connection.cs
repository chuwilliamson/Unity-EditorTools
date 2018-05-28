using System;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class Connection : IDrawable
    {
        private readonly IEventSystem _eventSystem;
        [NonSerialized]
        private readonly Node _in;
        [NonSerialized]
        private Node _out;
        private readonly Action<Node> _onConnectionComplete;
        private bool _connecting;
        private bool _complete;

        public Connection(Node node, Action<Node> onConnectionComplete, IEventSystem eventSystem)
        {
            _in = node;
            _out = null;
            _connecting = true;
            _complete = false;
            
            _onConnectionComplete = onConnectionComplete;
            _eventSystem = eventSystem;
            _eventSystem.OnMouseDown += OnMouseDown;
        }

        public void Draw(Event e)
        {
            if (!_connecting && !_complete)return;
            var rect = new Rect(e.mousePosition, Vector3.one);
            Chutilities.DrawNodeCurve(_in.BackgroundRect, end: _connecting ? rect:_out.BackgroundRect);
        }

        private void OnMouseDown(Event e)
        {
            if (_complete)//we already completed so we don't need to set values again
                return;

            var hovered = _eventSystem.WillSelect as Node;

            if (hovered == null)//if nothing is hovered or already completed
            {
                _connecting = false;
                return;
            }

            if (hovered == _in)
                return;

            _out = hovered;
            _connecting = false;
            _complete = true;

            _onConnectionComplete(_out);
        }
    }
}
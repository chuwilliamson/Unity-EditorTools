using System;
using UnityEngine;

namespace ChuTools
{
    public class Connection : IDrawable
    {
        private readonly IEventSystem _eventSystem;
        private readonly Node _in;
        private Node _out;
        private readonly Action<Node> _onConnectionComplete;
        private bool _connecting;
        private bool _complete;

        public Connection(Node node, IEventSystem eventSystem, Action<Node> onConnectionComplete)
        {
            _in = node;
            _out = null;
            _connecting = true;
            _complete = false;
            _eventSystem = eventSystem;
            _eventSystem.OnMouseDown += OnMouseDown;
            _onConnectionComplete = onConnectionComplete;
        }

        public void Draw(Event e)
        {
            var rect = new Rect(e.mousePosition, Vector3.one);
            Chutilities.DrawNodeCurve(_in.CenterRect, end: _connecting ? rect:_out.CenterRect);
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
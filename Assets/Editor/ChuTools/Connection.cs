using System;
using BehaviourMachine;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class Connection : IDrawable
    {
        private readonly IEventSystem _eventSystem;

        private readonly Node _in;
        private bool _active;
        private readonly Action<Node> _onConnectionMade;
        private Node _out;

        public Connection(Node node, IEventSystem eventSystem, Action<Node> onConnectionMade) 
        {
            _in = node;
            _out = null;
            _active = true;
            _eventSystem = eventSystem;
            _eventSystem.OnMouseDown += OnMouseDown;
            _onConnectionMade = onConnectionMade;
        }

        public void Draw(Event e)
        {
            if (!_active && _out == null) return;
            var rect = new Rect(Event.current.mousePosition, Vector3.one);
            DrawNodeCurve(_in.CenterRect, end: _out == null ? rect : _out.CenterRect);
        }

        private static void DrawNodeCurve(Rect start, Rect end)
        {
            var startPos = new Vector3(x: start.x + start.width, y: start.y + start.height / 2, z: 0);
            var endPos = new Vector3(end.x, y: end.y + end.height / 2, z: 0);
            var startTan = startPos + Vector3.right * 50;
            var endTan = endPos + Vector3.left * 50;
            var shadowCol = new Color(0, 0, 0, 0.06f);
            for (var i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, width: (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
            GUI.changed = true;

        }

        private void OnMouseDown(Event e)
        {
            _active = false;
            if (_eventSystem.WillSelect == null)
                return;

            if (_eventSystem.WillSelect != _in)
            {
                _out = _eventSystem.WillSelect as Node;
                _onConnectionMade(_out); 
            }
                
        }
    }
}
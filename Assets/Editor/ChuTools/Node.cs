using System;
using NUnit.Framework.Internal.Commands;
using UnityEngine;

namespace ChuTools
{
    public class Node
    {

        public int Id;
        private IEventSystem _nodeEventSystem;
        public IEventSystem NodeEventSystem
        {
            get { return _nodeEventSystem; }
            set
            {
                _nodeEventSystem = value;
                _nodeEventSystem.OnMouseUp += OnMouseUp;
                _nodeEventSystem.OnMouseDown += OnMouseDown;
              //  _nodeEventSystem.OnMouseDrag += OnMouseDrag;
                _nodeEventSystem.OnRepaint += Draw;
            }
        }

        public Rect Rect;

        private readonly GUIStyle _normal = new GUIStyle("flow node 0");
        private readonly GUIStyle _selected = new GUIStyle("flow node 0 on");
        private GUIStyle _currentStyle;

        public Node()
        {
            _currentStyle = _normal;
        }

        public Node(Vector2 position, int id) : this()
        {
            Rect = new Rect(position, size: new Vector2(150, 50));
            Id = id;
        }

        public void Draw(Event e)
        {
            var botrect = Rect;
            botrect.position = new Vector2(Rect.position.x, y: Rect.position.y + Rect.height);

            GUI.Box(Rect, content: new GUIContent { text = Rect.position.ToString() }, style: _currentStyle);
            var guistyle = new GUIStyle { normal = { textColor = Color.white } };
            GUI.Box(botrect, text: Rect.position.ToString(), style: guistyle);
        }


        public void OnMouseDrag(Event e)
        {
            var newposition = Rect.position + e.delta;

            if (newposition.x < 0) //left
                return;
            if (newposition.y < 0) //top
                return;
            if (newposition.x > Screen.width - Rect.width) //right
                return;
            if (newposition.y > Screen.height - Rect.height) //bottom
                return;

            if (NodeEventSystem.Selected == this)
            {
                Rect.position = newposition;
                e.Use();
            }

        }

        public void OnMouseDown(Event e)
        {
            if (Rect.Contains(e.mousePosition))
            {
                if (NodeEventSystem.Selected == null)
                {
                    NodeEventSystem.Selected = this;
                    e.Use();
                }
                else if (NodeEventSystem.Selected == this)
                {

                }
            }
            if (!Rect.Contains(e.mousePosition) && NodeEventSystem.Selected == this)
            {
                NodeEventSystem.Selected = null;
            }

            _currentStyle = NodeEventSystem.Selected == this ? _selected : _normal;
        }

        public void OnMouseUp(Event e)
        {

        }

        public override string ToString()
        {
            return string.Format("Node {0}", Id);
        }
    }
}
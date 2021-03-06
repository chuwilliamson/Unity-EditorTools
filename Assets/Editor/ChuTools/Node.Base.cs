﻿using System;
using System.Collections.Generic;
using JeremyTools;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public partial class Node : UIElement
    {
        private readonly Action<Connection> _onConnectionMade;

        private int _dragcounter;

        public List<ConnectionPoint> points;

        public Node(Vector2 position, Vector2 size, Action<Node> onRemoveNode) : base("OldNode", position, size)
        {
            points = new List<ConnectionPoint>();
            _onRemoveNodeAction = onRemoveNode;
        }


        public override void OnMouseDown(Event e)
        {
            base.OnMouseDown(e);
            switch (e.button)
            {
                case 1:
                    if (!Rect.Contains(e.mousePosition)) return;
                    var gm = new GenericMenu();
                    gm.AddItem(new GUIContent("Remove"), false, OnRemoveNode, this);
                    gm.ShowAsContext();
                    GUI.changed = true;
                    e.Use();
                    break;
            }
        }

        public override void Draw()
        {
            base.Draw();
            GUILayout.BeginArea(Rect);
            GUILayout.Label(_dragcounter.ToString());
            GUILayout.EndArea();
        }

        private void OnRemoveNode(object n)
        {
            _onRemoveNodeAction?.Invoke(this);
        }

        public override string ToString()
        {
            return $"Node {ControlId}";
        }
    }
}
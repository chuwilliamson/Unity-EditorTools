using System;
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

        public Node(Vector2 position, Vector2 size, Action<Node> onRemoveNode)
        {
            points = new List<ConnectionPoint>();
            _onRemoveNodeAction = onRemoveNode;
            Base("OldNode", "flow node 0", "flow node on 0", new Rect(position, size));
        }


        public override void OnMouseDown(Event e)
        {
            base.OnMouseDown(e);
            if (e.button == 1)
            {
                if (!rect.Contains(e.mousePosition)) return;
                var gm = new GenericMenu();
                gm.AddItem(new GUIContent("Remove"), false, OnRemoveNode, this);
                gm.ShowAsContext();
                GUI.changed = true;
                e.Use();
            }
        }

        public override void Draw()
        {
            base.Draw();
            GUILayout.BeginArea(rect);
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
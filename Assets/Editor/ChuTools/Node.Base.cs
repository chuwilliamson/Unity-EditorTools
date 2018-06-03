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
            Rect = new Rect(position, size);

            Content = new GUIContent(ControlId.ToString());
            Style = NodeEditorWindow.NodeStyle;
            points = new List<ConnectionPoint>();

            _onRemoveNodeAction = onRemoveNode;


        }

        public Vector2 OutCenter => Rect.center;

        public Vector2 InCenter => Rect.center;

        public int Value { get; set; }

 
        public override void OnMouseDown(Event e)
        {
            switch (e.button)
            {
                case 0:
                    if(Rect.Contains(e.mousePosition))
                    {
                        GUIUtility.hotControl = ControlId;
                        Style = NodeEditorWindow.SelectedNodeStyle;
                        GUI.changed = true;
                    }

                    break;
                case 1:
                    if(!Rect.Contains(e.mousePosition)) return;
                    var gm = new GenericMenu();
                    gm.AddItem(new GUIContent("Nodes/Remove"), false, OnRemoveNode, this);
                    gm.ShowAsContext();
                    GUI.changed = true;
                    e.Use();
                    break;
                case 2:
                    break;
            }
        }
         

        public virtual void OnMouseDrag(Event e)
        {
            if(GUIUtility.hotControl == ControlId)
            {
                _dragcounter++;
                Position += e.delta; 
                GUI.changed = true;
                e.Use();
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
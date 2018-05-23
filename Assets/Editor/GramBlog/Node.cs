using System;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace _Editor.GramBlog
{
    public class Node
    {
        [XmlIgnore] public GUIStyle defaultNodeStyle;

        public ConnectionPoint inPoint;

        [XmlIgnore] public bool isDragged;

        [XmlIgnore] public bool isSelected;

        [XmlIgnore] public Action<Node> OnRemoveNode;

        public ConnectionPoint outPoint;
        public Rect rect;

        [XmlIgnore] public GUIStyle selectedNodeStyle;

        [XmlIgnore] public GUIStyle style;

        [XmlIgnore] public string title;

        public Node()
        {
        }

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint,
            Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
        {
            rect = new Rect(x: position.x, y: position.y, width: width, height: height);
            style = nodeStyle;
            inPoint = new ConnectionPoint(this, type: ConnectionPointType.In, style: inPointStyle,
                OnClickConnectionPoint: OnClickInPoint);
            outPoint = new ConnectionPoint(this, type: ConnectionPointType.Out, style: outPointStyle,
                OnClickConnectionPoint: OnClickOutPoint);
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
            OnRemoveNode = OnClickRemoveNode;
        }

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint,
            Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, string inPointID, string outPointID)
        {
            rect = new Rect(x: position.x, y: position.y, width: width, height: height);
            style = nodeStyle;
            inPoint = new ConnectionPoint(this, type: ConnectionPointType.In, style: inPointStyle,
                OnClickConnectionPoint: OnClickInPoint, id: inPointID);
            outPoint = new ConnectionPoint(this, type: ConnectionPointType.Out, style: outPointStyle,
                OnClickConnectionPoint: OnClickOutPoint, id: outPointID);
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
            OnRemoveNode = OnClickRemoveNode;
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public void Draw()
        {
            inPoint.Draw();
            outPoint.Draw();
            GUI.Box(position: rect, text: title, style: style);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                        if (rect.Contains(point: e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                            style = selectedNodeStyle;
                        }
                        else
                        {
                            GUI.changed = true;
                            isSelected = false;
                            style = defaultNodeStyle;
                        }

                    if (e.button == 1 && isSelected && rect.Contains(point: e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(delta: e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }

            return false;
        }

        void ProcessContextMenu()
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, func: OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        void OnClickRemoveNode()
        {
            if (OnRemoveNode != null)
                OnRemoveNode(this);
        }
    }
}
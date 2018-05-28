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

        public Node(Vector2 position, float width, float height,
            GUIStyle nodeStyle,
            GUIStyle selectedStyle,
            GUIStyle inPointStyle,
            GUIStyle outPointStyle,
            Action<ConnectionPoint> onClickInPoint,
            Action<ConnectionPoint> onClickOutPoint,
            Action<Node> onClickRemoveNode)
        {
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
            inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, onClickInPoint);
            outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, onClickOutPoint);
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
            OnRemoveNode = onClickRemoveNode;
        }

        public Node(Vector2 position, float width, float height,
            GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle,
            Action<ConnectionPoint> onClickInPoint,
            Action<ConnectionPoint> onClickOutPoint,
            Action<Node> onClickRemoveNode,
            string inPointId,
            string outPointId)
        {
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
            inPoint = new ConnectionPoint(node: this, type: ConnectionPointType.In, style: inPointStyle,
                OnClickConnectionPoint: onClickInPoint, id: inPointId);
            outPoint = new ConnectionPoint(node: this, type: ConnectionPointType.Out, style: outPointStyle,
                OnClickConnectionPoint: onClickOutPoint, id: outPointId);
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
            OnRemoveNode = onClickRemoveNode;
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public void Draw()
        {
            inPoint.Draw();
            outPoint.Draw();
            GUI.Box(rect, title, style);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    switch (e.button)
                    {
                        case 0:
                            if (rect.Contains(e.mousePosition))
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
                            break;
                        case 1:
                            if (isSelected && rect.Contains(e.mousePosition))
                            {
                                ProcessContextMenu();
                                e.Use();
                            }
                            break;
                    }

                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }

            return false;
        }

        private void ProcessContextMenu()
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(content: new GUIContent("Remove node"), on: false, func: OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            OnRemoveNode?.Invoke(obj: this);
        }
    }
}
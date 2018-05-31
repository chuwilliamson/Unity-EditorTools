using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;


namespace ChuTools
{

    public class ConnectionPoint : UIElement
    {

        private Rect _endRect;
        private bool _connected;
        private bool isActive = false;
        private bool isDragging = false;
        private Vector2 currentMouse = Vector2.zero;
        private readonly Action<ConnectionPoint> _onConnectionComplete;

        private GUIStyle _normalStyle = new GUIStyle("U2D.pivotDot");
        
        public int Guid;

        public ConnectionPoint(Vector2 position, Vector2 size, Action<ConnectionPoint> onConnectionComplete) : this(position, size)
        {
            Guid = GetHashCode();
            
            _onConnectionComplete = onConnectionComplete;
        }

        public ConnectionPoint(Vector2 position, Vector2 size)
        {
            _Rect = new Rect(position, size);
            _endRect = new Rect(_Rect);
            NodeEditorWindow.NodeEvents.OnMouseDrag += OnPointerDrag;
            NodeEditorWindow.NodeEvents.OnMouseDown += OnPointerDown;
            NodeEditorWindow.NodeEvents.OnMouseUp += OnPointerUp;

        }

        public override void Draw()
        {
            GUI.Box(_Rect, GUIContent.none, Node.NormalStyle);
            GUI.Label(_Rect, Guid.ToString());
            
            if (isDragging)
            {
                _endRect = new Rect(currentMouse, _Rect.size);
                Chutilities.DrawNodeCurve(_Rect, _endRect);
                Handles.RectangleHandleCap(GUIUtility.GetControlID(FocusType.Passive), _endRect.position, Quaternion.identity, 15, EventType.Repaint);
            }
        }

        public Vector2 Position
        {
            get { return _Rect.position; }
            set { _Rect.position = value; }
        }

        public void OnPointerUp(Event e)
        {
            if (!isActive) return;

            isActive = false;
            isDragging = false;
            _onConnectionComplete(this);
            GUI.changed = true;
        }
        public void OnPointerDown(Event e)
        {
            if (_Rect.Contains(e.mousePosition))
            {
                isActive = true;
                isDragging = true;
                GUI.changed = true;
                currentMouse = e.mousePosition;
                e.Use();
            }
        }

        public void OnPointerDrag(Event e)
        {
            if (isActive)
            {
                isDragging = true;
                currentMouse = e.mousePosition;
                GUI.changed = true;

            }
        }


        public bool CreateConnection(ConnectionPoint connectionPoint)
        {
            if (_endRect.Overlaps(connectionPoint._Rect))
            {
                Debug.Log(string.Format("connecting {0} with {1}", Guid, connectionPoint.Guid));
                _endRect = connectionPoint._Rect;
                connectionPoint._endRect = _Rect;
                return _endRect.Overlaps(connectionPoint._Rect);
            }
            return _endRect.Overlaps(connectionPoint._Rect);

        }
    }
}

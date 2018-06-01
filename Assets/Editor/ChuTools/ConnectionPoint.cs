using System;
using TrentTools;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming


namespace ChuTools
{
    public class ConnectionPoint : UIElement, IConnection, IMouseDownHandler, IMouseUpHandler, IMouseDragHandler 
    {
        public ConnectionPoint()
        {
            Style = new GUIStyle("flow node 6");
        }
        private readonly GUIStyle _normalStyle;
        private readonly Action<ConnectionPoint> _onConnectionComplete;

        private Vector2 _currentMouse = Vector2.zero;

        private int _dragcounter;
        private Rect _endRect;
        private bool _isActive = false;

        public ConnectionPoint(Vector2 position, Vector2 size, Action<ConnectionPoint> onConnectionComplete) : this(
            position, size)
        {
            _onConnectionComplete = onConnectionComplete;
                
         
            Content = GUIContent.none;
        }

        public override string ToString()
        {
            return base.ToString() + "::" + ControlId.ToString();
        }

        public ConnectionPoint(Vector2 position, Vector2 size) : this()
        {
            Rect = new Rect(position, size);
            _endRect = new Rect(Rect);
            NodeEditorWindow.NodeEvents.OnMouseDrag += OnMouseDrag;
            NodeEditorWindow.NodeEvents.OnMouseDown += OnMouseDown;
            NodeEditorWindow.NodeEvents.OnMouseUp += OnMouseUp;
            NodeEditorWindow.NodeEvents.OnMouseMove += OnMouseMove;
        }


        private ButtonState _cstate { get; set; }

        public void OnMouseDown(Event e)
        {
            if (Rect.Contains(e.mousePosition))
                if (e.button == 0)
                {
                    _cstate = ButtonState.Selected;
                    _currentMouse = e.mousePosition;
                    GUI.changed = true;
                }
        }

        public void OnMouseDrag(Event e)
        {
            if (_cstate == ButtonState.Selected)
            {
                _dragcounter++;
                _currentMouse = e.mousePosition;
                GUI.changed = true;
                e.Use();
            }
        }

        public void OnMouseUp(Event e)
        { 
            _cstate = ButtonState.Normal;

            GUI.changed = true;

        }

        private void OnMouseMove(Event e)
        {
            if (Rect.Contains(e.mousePosition))
            {
                switch (_cstate)
                {
                    case ButtonState.Normal:
                        _cstate = ButtonState.Hovered;
                        GUIUtility.hotControl = ControlId;
                        GUI.changed = true;
                        break;
                    case ButtonState.Active:
                        break;
                    case ButtonState.Selected:
                        break;
                }
            }
            else
            {
                if (GUIUtility.hotControl == ControlId)
                {
                    _cstate = ButtonState.Normal;
                    GUIUtility.hotControl = 0;
                    GUI.changed = true;
                }
            }
        }

        public override void Draw()
        {
            base.Draw();
            ControlId = GUIUtility.GetControlID(FocusType.Passive, Rect);
            GUILayout.BeginArea(Rect);
            GUILayout.Label("id:" + ControlId);
            GUILayout.EndArea();

            switch (_cstate)
            {
                case ButtonState.Selected:
                    _endRect = new Rect(_currentMouse, Rect.size);
                    Chutilities.DrawNodeCurve(Rect, _endRect);
                    Handles.RectangleHandleCap(GUIUtility.GetControlID(FocusType.Passive, _endRect), _endRect.center,
                        Quaternion.identity, 15, EventType.Repaint);
                    break;
            }
        }

        private enum ButtonState
        {
            Normal = 0,
            Active = 1,
            Hovered = 1,
            Selected = 2
        }

        public Vector2 OutCenter => _endRect.center;
        public Vector2 InCenter => Rect.center;
    }
}
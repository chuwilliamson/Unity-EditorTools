using System;
using TrentTools;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming


namespace ChuTools
{
    public class ConnectionPoint : UIElement, IConnection, IMouseDownHandler, IMouseUpHandler, IMouseDragHandler
    {
        private readonly GUIStyle _normalStyle;
        private readonly Action<ConnectionPoint> _onConnectionComplete;

        private Vector2 _currentMouse = Vector2.zero;

        private int _dragcounter;
        private Rect _endRect;
        private bool _isActive = false;

        public ConnectionPoint()
        {
            Content = GUIContent.none;
            Style = new GUIStyle("flow node 6");
        }

        public ConnectionPoint(Vector2 position, Vector2 size, Action<ConnectionPoint> onConnectionComplete) : this(
            position, size)
        {
            _onConnectionComplete = onConnectionComplete;


            Content = GUIContent.none;
        }

        public ConnectionPoint(Vector2 position, Vector2 size) : this()
        {
            Rect = new Rect(position, size); 
            _endRect = new Rect(Rect);
 
        }

        private ButtonState _cstate { get; set; }

        public Vector2 OutCenter => _endRect.center;
        public Vector2 InCenter => Rect.center;
         
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

            GUILayout.BeginArea(Rect);
            GUILayout.Label("id:" + ControlId);
            GUILayout.EndArea();

            switch (_cstate)
            {
                case ButtonState.Selected:
                    
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
    }
}
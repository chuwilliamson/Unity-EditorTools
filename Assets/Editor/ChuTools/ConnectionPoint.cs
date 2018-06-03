using System;
using TrentTools;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming


namespace ChuTools
{
    public class ConnectionPoint : UIElement, IConnection 
    {
        private readonly GUIStyle _normalStyle;
 
        private int _dragcounter;
        private Rect _endRect;
 
        public ConnectionPoint()
        {
            Content = GUIContent.none;
            Style = new GUIStyle("flow node 6");
        }

        public ConnectionPoint(Vector2 position, Vector2 size, Action<ConnectionPoint> onConnectionComplete) : this(
            position, size)
        {
  
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
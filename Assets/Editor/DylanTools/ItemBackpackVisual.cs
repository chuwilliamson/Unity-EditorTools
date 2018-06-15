using System.Collections;
using System.Collections.Generic;
using EditorInterfaces;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace BackpacViewerWindow
{
    public class ItemBackpackVisual : IDrawable, IDraggable
    {
        private Rect Rect;
        public Rect _Rect
        {
            get { return Rect; }
        }

        private bool isDraggable;
        public bool IsDraggable
        {
            get { return isDraggable; }
        }

        private Vector2 CurrentPosition = new Vector2(0,0);
        public Vector2 Positon
        {
            get { return CurrentPosition; }
            set { CurrentPosition = value; }
        }

        public ItemScriptable Data;

        public void Draw()
        {
            if(Data == null)
                return;
            Rect = new Rect(CurrentPosition, new Vector2(22,22));
            GUI.Box(Rect, "");
            if(Data.UIImage != null)
                GUI.DrawTexture(Rect, Data.UIImage);
        }

        public void EnableDragging()
        {
            if (Event.current.button == 0)
            {
                if (Rect.Contains(Event.current.mousePosition))
                    isDraggable = true;
                Event.current.Use();
            }
        }

        public void Drag()
        {
            var WindowSize = new Vector2();
            if (EditorWindow.focusedWindow != null)
                WindowSize = EditorWindow.focusedWindow.position.size;
            if (isDraggable)
            {
                if (Event.current.delta.x < 0 && Rect.position.x > 0)
                    CurrentPosition.x += Event.current.delta.x;
                if (Event.current.delta.x > 0 && Rect.position.x + Rect.width < WindowSize.x)
                    CurrentPosition.x += Event.current.delta.x;
                if (Event.current.delta.y < 0 && Rect.position.y > 0)
                    CurrentPosition.y += Event.current.delta.y;
                if (Event.current.delta.y > 0 && Rect.position.y + Rect.height < WindowSize.y)
                    CurrentPosition.y += Event.current.delta.y;
                Event.current.Use();
            }
        }

        public void DisableDragging()
        {
            isDraggable = false;
            Event.current.Use();
        }
    }
}

using Interfaces;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public abstract class CustomEditorWindow : EditorWindow, IEventSystem
    {
        public EditorEvent OnScrollWheel { get; set; }
        public Event Current { get; set; }

        public EditorEvent OnDragExited { get; set; }
 
        public EditorEvent OnMouseDown
        {
            get { return EventSystem.OnMouseDown; }
            set { EventSystem.OnMouseDown = value; }
        }

        public EditorEvent OnMouseUp
        {
            get { return EventSystem.OnMouseUp; }
            set { EventSystem.OnMouseUp = value; }
        }

        public EditorEvent OnRepaint
        {
            get { return EventSystem.OnRepaint; }
            set { EventSystem.OnRepaint = value; }
        }

        public EditorEvent OnMouseDrag
        {
            get { return EventSystem.OnMouseDrag; }
            set { EventSystem.OnMouseDrag = value; }
        }

        public EditorEvent OnContextClick
        {
            get { return EventSystem.OnContextClick; }
            set { EventSystem.OnContextClick = value; }
        }

        public EditorEvent OnMouseMove
        {
            get { return EventSystem.OnMouseMove; }
            set { EventSystem.OnMouseMove = value; }
        }

        public EditorEvent OnUsed
        {
            get { return EventSystem.OnUsed; }
            set { EventSystem.OnUsed = value; }
        }

        public void PollEvents(Event e)
        {
            EventSystem.PollEvents(e);
        }

        public abstract IEventSystem EventSystem { get; set; }
    }
}
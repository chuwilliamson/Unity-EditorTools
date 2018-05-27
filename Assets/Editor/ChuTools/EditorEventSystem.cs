using UnityEngine;

namespace ChuTools
{
    public class EditorEventSystem : IEventSystem
    { 
        public object Selected { get; set; }
        public object WillSelect { get; set; }
        public EditorEvent OnMouseDown { get; set; }
        public EditorEvent OnMouseUp { get; set; }
        public EditorEvent OnRepaint { get; set; }
        public EditorEvent OnMouseDrag { get; set; }
        public EditorEvent OnContextClick { get; set; }
        public EditorEvent OnMouseMove { get; set; }
        public EditorEvent OnUsed { get; set; }

        public void SetSelected(object obj)
        {
            if (WillSelect == obj)
                Selected = WillSelect;
        }

        public void Release(object obj)
        {
            if (Selected == null)
                return;

            SetSelected(obj);
        }
        public void Invoke(EditorEvent editorEvent, Event e)
        {
            if (editorEvent == null)
                return;
            editorEvent.Invoke(e);
        }
        public void PollEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDrag:
                    Invoke(OnMouseDrag, e);
                    GUI.changed = true;
                    break;
                case EventType.MouseUp:
                    Invoke(OnMouseUp,e);
                    GUI.changed = true;
                    break;
                case EventType.MouseDown:
                    Invoke(OnMouseDown, e); 
                    break;
                case EventType.Repaint:
                    Invoke(OnRepaint, e);
                    break;
                case EventType.ContextClick:
                    Invoke(OnContextClick, e);
                    GUI.changed = true;
                    break;
                case EventType.MouseMove:
                    Invoke(OnMouseMove, e);
                    break;
                case EventType.Used:
                    Invoke(OnUsed, e);
                    break;
        
            }
        }
    }
}
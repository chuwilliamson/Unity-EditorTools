﻿using UnityEngine;

namespace ChuTools
{
    [System.Serializable]
    public class NodeWindowEventSystem : IEventSystem
    {
        public Event Current { get; set; }
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
            Selected = WillSelect;
        }

        public void Release(object obj)
        {
            if(Selected == null)
                return;

            SetSelected(obj);
        }

        public void PollEvents(Event e)
        {
            Current = e;
            switch (Current.type)
            {
                case EventType.MouseDrag:
                    Invoke(OnMouseDrag, Current);
                    break;
                case EventType.MouseUp:
                    Invoke(OnMouseUp, Current);
                    break;
                case EventType.MouseDown:
                    Invoke(OnMouseDown, Current);
                    break;
                case EventType.Repaint:
                    Invoke(OnRepaint, Current);
                    break;
                case EventType.ContextClick:
                    Invoke(OnContextClick, Current);
                    break;
                case EventType.MouseMove:
                    OnMouseMove?.Invoke(Current);
                    break;
                case EventType.Used:
                    OnUsed?.Invoke(Current);
                    break;

            }
        }

        public void Invoke(EditorEvent cb, Event e)
        {
            cb?.Invoke(e);
        }
    }
}
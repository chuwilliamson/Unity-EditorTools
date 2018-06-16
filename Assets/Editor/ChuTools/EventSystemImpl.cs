using ChuTools.View;
using Interfaces;
using UnityEngine;

namespace ChuTools
{
    public interface IMouseEvent
    {
        EditorEvent OnMouseDown { get; set; }
        EditorEvent OnMouseUp { get; set; }
        EditorEvent OnMouseDrag { get; set; }
    }

    public abstract class EditorEventSystem : IEventSystem
    {
        public virtual void Invoke(EditorEvent cb, Event e)
        {
            cb?.Invoke(e);
        }

        public EditorEvent OnDragPerform { get; set; }
        public Event Current { get; set; }
        public EditorEvent OnMouseDown { get; set; }
        public EditorEvent OnMouseUp { get; set; }
        public EditorEvent OnRepaint { get; set; }
        public EditorEvent OnMouseDrag { get; set; }
        public EditorEvent OnContextClick { get; set; }
        public EditorEvent OnMouseMove { get; set; }
        public EditorEvent OnUsed { get; set; }
        public EditorEvent OnDragExited { get; set; }
        public EditorEvent OnScrollWheel { get; set; }
        public EditorEvent OnDragUpdated { get; set; }

        public virtual void PollEvents(Event e)
        {
            Current = e;
            switch (Current.type)
            {
                case EventType.ScrollWheel:
                    OnScrollWheel?.Invoke(Current);
                    break;

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
                case EventType.DragExited:
                    OnDragExited?.Invoke(Current);
                    break;
                case EventType.DragUpdated:
                    OnDragUpdated?.Invoke(Current);
                    break;
                case EventType.DragPerform:
                    OnDragPerform?.Invoke(Current);
                    break;
            }
        }
    }

    public class GridWindowEventSystem : EditorEventSystem
    {
    }

    public class NodeWindowEventSystem : EditorEventSystem
    {
        public override void PollEvents(Event e)
        {
            NodeEditorWindow.Drag = Vector2.zero;
            base.PollEvents(e);
        }
    }
}
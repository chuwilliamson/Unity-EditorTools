using ChuTools;
using UnityEngine;

namespace Interfaces
{
    public interface IMouseDragHandler
    {
        void OnMouseDrag(Event e);
    }

    public interface IMouseDownHandler
    {
        void OnMouseDown(Event e);
    }

    public interface IMouseUpHandler
    {
        void OnMouseUp(Event e);
    }

    public interface IMouseMoveHandler
    {
        void OnMouseMoveHandler(Event e);
    }

    public interface IEventSystem
    {
        void SetSelected(object obj);
        void Release(object obj);
        void PollEvents(Event e);
        object Selected { get; set; }
        object WillSelect { get; set; }
        EditorEvent OnMouseDown { get; set; }
        EditorEvent OnMouseUp { get; set; }
        EditorEvent OnRepaint { get; set; }
        EditorEvent OnMouseDrag { get; set; }
        EditorEvent OnContextClick { get; set; }
        EditorEvent OnMouseMove { get; set; }
        EditorEvent OnUsed { get; set; }
        Event Current { get; set; }
    }
}
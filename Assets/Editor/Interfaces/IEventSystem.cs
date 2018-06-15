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

        void PollEvents(Event e);
        EditorEvent OnDragExited { get; set; }
        EditorEvent OnMouseDown { get; set; }
        EditorEvent OnMouseUp { get; set; }
        EditorEvent OnRepaint { get; set; }
        EditorEvent OnMouseDrag { get; set; }
        EditorEvent OnContextClick { get; set; }
        EditorEvent OnMouseMove { get; set; }
        EditorEvent OnUsed { get; set; }
        EditorEvent OnScrollWheel { get; set; }
        Event Current { get; set; }
    }
}
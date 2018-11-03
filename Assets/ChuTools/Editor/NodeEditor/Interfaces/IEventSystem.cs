using ChuTools.NodeEditor.EventSystems;
using UnityEngine;

namespace ChuTools.NodeEditor.Interfaces
{
    public interface IEventSystem
    {
        EditorEvent OnDragExited { get; set; }
        EditorEvent OnMouseDown { get; set; }
        EditorEvent OnMouseUp { get; set; }
        EditorEvent OnRepaint { get; set; }
        EditorEvent OnMouseDrag { get; set; }
        EditorEvent OnContextClick { get; set; }
        EditorEvent OnMouseMove { get; set; }
        EditorEvent OnUsed { get; set; }
        EditorEvent OnScrollWheel { get; set; }
        EditorEvent OnDragUpdated { get; set; }
        EditorEvent OnDragPerform { get; set; }
        Event Current { get; set; }

        void PollEvents(Event e);
    }
}
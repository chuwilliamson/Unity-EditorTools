using UnityEngine;

namespace ChuTools
{
    public interface IEventSystem
    {
        void SetSelected(object obj);
        void Release(object obj);
        object Selected { get; set; }
        object WillSelect { get; set; }
        EditorEvent OnMouseDown { get; set; }
        EditorEvent OnMouseUp { get; set; }
        EditorEvent OnRepaint { get; set; }
        EditorEvent OnMouseDrag { get; set; }
        EditorEvent OnContextClick { get; set; }
        EditorEvent OnMouseMove { get; set; }
        EditorEvent OnUsed { get; set; }
        void PollEvents(Event e);
    }
}
using ChuTools.NodeEditor.EventSystems;

namespace ChuTools.NodeEditor.Interfaces
{
    public interface IMouseEvent
    {
        EditorEvent OnMouseDown { get; set; }
        EditorEvent OnMouseUp { get; set; }
        EditorEvent OnMouseDrag { get; set; }
    }
}
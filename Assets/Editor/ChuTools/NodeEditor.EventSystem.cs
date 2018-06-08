using UnityEngine;

namespace ChuTools
{
    public partial class NodeEditorWindow
    {
        public static Event Current
        {
            get { return NodeEvents.Current; }
            set { NodeEvents.Selected = value; }
        }

        public static object Selected
        {
            get { return NodeEvents.Selected; }
            set { NodeEvents.Selected = value; }
        }

        public static object WillSelect
        {
            get { return NodeEvents.WillSelect; }
            set { NodeEvents.WillSelect = value; }
        }

        public static EditorEvent OnMouseDown
        {
            get { return NodeEvents.OnMouseDown; }
            set { NodeEvents.OnMouseDown = value; }
        }

        public static EditorEvent OnMouseUp
        {
            get { return NodeEvents.OnMouseUp; }
            set { NodeEvents.OnMouseUp = value; }
        }

        public static EditorEvent OnRepaint
        {
            get { return NodeEvents.OnRepaint; }
            set { NodeEvents.OnRepaint = value; }
        }

        public static EditorEvent OnMouseDrag
        {
            get { return NodeEvents.OnMouseDrag; }
            set { NodeEvents.OnMouseDrag = value; }
        }

        public static EditorEvent OnContextClick
        {
            get { return NodeEvents.OnContextClick; }
            set { NodeEvents.OnContextClick = value; }
        }

        public static EditorEvent OnMouseMove
        {
            get { return NodeEvents.OnMouseMove; }
            set { NodeEvents.OnMouseMove = value; }
        }

        public static EditorEvent OnUsed
        {
            get { return NodeEvents.OnUsed; }
            set { NodeEvents.OnUsed = value; }
        }

        public static void SetSelected(object obj)
        {
            NodeEvents.SetSelected(obj);
        }

        public static void Release(object obj)
        {
            NodeEvents.Release(obj);
        }

        public static void PollEvents(Event e)
        {
            NodeEvents.PollEvents(e);
        }
    }
}
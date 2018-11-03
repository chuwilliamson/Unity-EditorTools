using ChuTools.NodeEditor.EventSystems;
using UnityEngine;

namespace ChuTools.NodeEditor.View
{
    public partial class NodeEditor
    {
        public static Event Current
        {
            get { return NodeEventSystem.Current; }
            set { NodeEventSystem.Current = value; }
        }

        public static EditorEvent OnMouseDown
        {
            get { return NodeEventSystem.OnMouseDown; }
            set { NodeEventSystem.OnMouseDown = value; }
        }

        public static EditorEvent OnMouseUp
        {
            get { return NodeEventSystem.OnMouseUp; }
            set { NodeEventSystem.OnMouseUp = value; }
        }

        public static EditorEvent OnRepaint
        {
            get { return NodeEventSystem.OnRepaint; }
            set { NodeEventSystem.OnRepaint = value; }
        }

        public static EditorEvent OnMouseDrag
        {
            get { return NodeEventSystem.OnMouseDrag; }
            set { NodeEventSystem.OnMouseDrag = value; }
        }

        public static EditorEvent OnDragExited
        {
            get { return NodeEventSystem.OnDragExited; }
            set { NodeEventSystem.OnDragExited = value; }
        }

        public static EditorEvent OnContextClick
        {
            get { return NodeEventSystem.OnContextClick; }
            set { NodeEventSystem.OnContextClick = value; }
        }

        public static EditorEvent OnMouseMove
        {
            get { return NodeEventSystem.OnMouseMove; }
            set { NodeEventSystem.OnMouseMove = value; }
        }

        public static EditorEvent OnScrollWheel
        {
            get { return NodeEventSystem.OnScrollWheel; }
            set { NodeEventSystem.OnScrollWheel = value; }
        }

        public static EditorEvent OnUsed
        {
            get { return NodeEventSystem.OnUsed; }
            set { NodeEventSystem.OnUsed = value; }
        }

        public static void PollEvents(Event e)
        {
            NodeEventSystem.PollEvents(e);
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public abstract class CustomEditorWindow : EditorWindow, IEventSystem
    {
        public abstract IEventSystem NodeEventSystem { get; set; }
        public Event Current { get; set; }

        public void SetSelected(object obj)
        {
            NodeEventSystem.SetSelected(obj);
        }

        public void Release(object obj)
        {
            NodeEventSystem.Release(obj);
        }

        public object Selected
        {
            get { return NodeEventSystem.Selected; }
            set { NodeEventSystem.Selected = value; }
        }

        public object WillSelect
        {
            get { return NodeEventSystem.WillSelect; }
            set { NodeEventSystem.WillSelect = value; }
        }

        public EditorEvent OnMouseDown
        {
            get { return NodeEventSystem.OnMouseDown; }
            set { NodeEventSystem.OnMouseDown = value; }
        }

        public EditorEvent OnMouseUp
        {
            get { return NodeEventSystem.OnMouseUp; }
            set { NodeEventSystem.OnMouseUp = value; }
        }

        public EditorEvent OnRepaint
        {
            get { return NodeEventSystem.OnRepaint; }
            set { NodeEventSystem.OnRepaint = value; }
        }

        public EditorEvent OnMouseDrag
        {
            get { return NodeEventSystem.OnMouseDrag; }
            set { NodeEventSystem.OnMouseDrag = value; }
        }

        public EditorEvent OnContextClick
        {
            get { return NodeEventSystem.OnContextClick; }
            set { NodeEventSystem.OnContextClick = value; }
        }

        public EditorEvent OnMouseMove
        {
            get { return NodeEventSystem.OnMouseMove; }
            set { NodeEventSystem.OnMouseMove = value; }
        }

        public EditorEvent OnUsed
        {
            get { return NodeEventSystem.OnUsed; }
            set { NodeEventSystem.OnUsed = value; }
        }

        public void PollEvents(Event e)
        {
            NodeEventSystem.PollEvents(e);
        }
    }
}
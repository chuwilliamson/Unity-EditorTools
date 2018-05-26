using System;
using UnityEngine;
using UnityEditor;
namespace ChuTools
{
    public interface IEventSystem
    {
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

    public class MyEventSystem : IEventSystem
    { 
        public object Selected { get; set; }
        public object WillSelect { get; set; }
        public EditorEvent OnMouseDown { get; set; }
        public EditorEvent OnMouseUp { get; set; }
        public EditorEvent OnRepaint { get; set; }
        public EditorEvent OnMouseDrag { get; set; }
        public EditorEvent OnContextClick { get; set; }
        public EditorEvent OnMouseMove { get; set; }
        public EditorEvent OnUsed { get; set; }
        public void PollEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDrag:
                    OnMouseDrag?.Invoke(e);
                    break;
                case EventType.MouseUp:
                    OnMouseUp?.Invoke(e);
                    break;
                case EventType.MouseDown:
                    OnMouseDown?.Invoke(e);
                    break;
                case EventType.Repaint:
                    OnRepaint?.Invoke(e);
                    break;
                case EventType.ContextClick:
                    OnContextClick?.Invoke(e);
                    break;
                case EventType.MouseMove:
                    OnMouseMove?.Invoke(e);
                    break;
                case EventType.Used:
                    OnUsed?.Invoke(e);
                    break;
        
            }
        }
    }

    public abstract class CustomEditorWindow : EditorWindow
    {
        public readonly IEventSystem MyEventSystem = new MyEventSystem();
    }
}
using UnityEngine;
using UnityEditor;
namespace ChuTools
{
    public delegate void EditorEvent(Event e);

    public abstract class CustomEditorWindow :EditorWindow
    { 
        public EditorEvent onMouseDown;
        public EditorEvent onMouseUp;
        public EditorEvent onRepaint;
        public EditorEvent onMouseDrag;
        public EditorEvent onContextClick;
         

        private void ProcessDelegate(EditorEvent editorEvent)
        {
            if (editorEvent == null) return;            
                editorEvent.Invoke(Event.current);
        }

        public void PollEvents(Event e)
        {   
            switch (e.type)
            {
                case EventType.MouseDrag:
                    onMouseDrag?.Invoke(e);
                    ProcessDelegate(onMouseDrag);
                    break;
                case EventType.MouseUp:
                    ProcessDelegate(onMouseUp);
                    break;
                case EventType.MouseDown:
                    ProcessDelegate(onMouseDown);
                    break;
                case EventType.Repaint:
                    ProcessDelegate(onRepaint);
                    break;
                case EventType.ContextClick:
                    ProcessDelegate(onContextClick);
                    break;
            }       
        }
    }
}
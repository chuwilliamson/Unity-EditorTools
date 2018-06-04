using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorGlobals
{
    public delegate void OnMouseDragged();
    public delegate void OnMouseDown();
    public delegate void OnMouseUp();
    public static OnMouseDragged mouseDragEvent;
    public static OnMouseDown mouseDownEvent;
    public static OnMouseUp mouseUpEvent;

    public static void GUIEvents()
    {
        var current = Event.current;
        switch(current.type)
        {
            case EventType.MouseDown:
                if(mouseDownEvent != null)
                    mouseDownEvent.Invoke();                
                break;
            case EventType.MouseDrag:
                if(mouseDragEvent != null)
                    mouseDragEvent.Invoke();                
                break;
            case EventType.MouseUp:
                if(mouseUpEvent != null)
                    mouseUpEvent.Invoke();                
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class EditorEvents
{
    public delegate void OnMouseDown();
    public delegate void OnMouseUP();
    public delegate void OnMouseDrag();

    public OnMouseDrag MouseDragEvent;
    public OnMouseUP MouseUpEvent;
    public OnMouseDown MouseDownEvent;

    public void Update()
    {
        var currentEvent = Event.current;
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                if(MouseDownEvent != null)
                    MouseDownEvent.Invoke();
                break;
            case EventType.MouseDrag:
                if(MouseDragEvent != null)
                    MouseDragEvent.Invoke();
                break;
            case EventType.MouseUp:
                if(MouseUpEvent != null)
                    MouseUpEvent.Invoke();
                break;
        }
    }
}

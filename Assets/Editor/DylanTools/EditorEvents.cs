using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class EditorEvents
{
    public delegate void OnMouseDown();
    public delegate void OnMouseUP();
    public delegate void OnMouseDrag();

    private OnMouseDrag _MouseDragEvent;
    public OnMouseDrag MouseDragEvent
    {
        get { return _MouseDragEvent; }
        set { _MouseDragEvent = value; }
    }
    public OnMouseUP _MouseUpEvent;
    public OnMouseUP MouseUpEvent
    {
        get { return _MouseUpEvent; }
        set { _MouseUpEvent = value; }
    }
    public OnMouseDown _MouseDownEvent;
    public OnMouseDown MouseDownEvent
    {
        get { return _MouseDownEvent; }
        set { _MouseDownEvent = value; }
    }

    public void Update()
    {
        var currentEvent = Event.current;
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                if (MouseDownEvent != null)
                {
                    MouseDownEvent.Invoke();
                }
                break;
            case EventType.MouseDrag:
                if (MouseDragEvent != null)
                {
                    MouseDragEvent.Invoke();
                }
                break;
            case EventType.MouseUp:
                if (MouseUpEvent != null)
                {
                    MouseUpEvent.Invoke();
                }
                break;
        }
    }
}

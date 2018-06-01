using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIElement
{
    public Rect _Rect;
    public virtual void Draw() { }
    public virtual void PollEvents(Event e) { }
}

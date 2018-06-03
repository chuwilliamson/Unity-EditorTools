using ChuTools;
using Interfaces;
using UnityEngine;

public abstract class UIElement : IDrawable, IMouseDownHandler, IMouseUpHandler, IMouseDragHandler
{
    protected UIElement()
    {
        NodeEditorWindow.NodeEvents.OnMouseDown += OnMouseDown;
        NodeEditorWindow.NodeEvents.OnMouseUp += OnMouseUp;
        NodeEditorWindow.NodeEvents.OnMouseDrag += OnMouseDrag;
        ControlId = GUIUtility.GetControlID(FocusType.Passive, Rect);

    }

    public Rect Rect;
    public bool IsSelected { get; private set; }
    public Vector2 Position
    {
        get { return Rect.position; }
        set
        {
            var newp = new Vector2(value.x, value.y);
            Rect.position = newp;
        }
    }

    public GUIStyle SelectedStyle { get; set; }
    public GUIStyle NormalStyle { get; set; }
    public GUIStyle Style { get; set; }
    public GUIContent Content { get; set; }
    public int ControlId;

    Rect IDrawable.Rect => Rect;
    /// <summary>
    /// Draw the default box for this ui element
    /// </summary>
    public virtual void Draw()
    {
        GUI.Box(Rect, Content, Style);
  
    }

    public virtual void OnMouseDown(Event e)
    {
        if (Rect.Contains(e.mousePosition))
        {
            IsSelected = true;
            GUIUtility.hotControl = ControlId;
            Style = SelectedStyle;
            GUI.changed = true;
        }
    }

    public virtual void OnMouseUp(Event e)
    {
        IsSelected = false;

        if (GUIUtility.hotControl == ControlId)
        {
            GUIUtility.hotControl = 0;
            Style = NormalStyle;
            GUI.changed = true;
        }
    }

    public virtual void OnMouseDrag(Event e)
    {
        if (GUIUtility.hotControl == ControlId)
        {
            Rect.position += e.delta;
            GUI.changed = true;
            e.Use();
        } 
    }
}

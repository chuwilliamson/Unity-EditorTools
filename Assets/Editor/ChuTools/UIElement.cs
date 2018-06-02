using Interfaces;
using UnityEngine;

public abstract class UIElement : IDrawable
{
    public Rect Rect;
    public Vector2 Position
    {
        get { return Rect.position; }
        set
        {
            var newp = new Vector2(value.x, value.y);
            Rect.position = newp;
        }
    }

    public GUIStyle Style { get; set; }
    public GUIContent Content { get; set; }
    public int ControlId { get; protected set; }

    Rect IDrawable.Rect => Rect;
    /// <summary>
    /// Draw the default box for this ui element
    /// </summary>
    public virtual void Draw()
    {
        if (Content == null)
            Content = GUIContent.none;

        if (Style == null)
            Style = new GUIStyle();

        GUI.Box(Rect, Content, Style);
    }
}

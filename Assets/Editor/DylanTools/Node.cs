using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    protected Rect VisualRect;
    private Rect ScaleRect;
    private Rect DeleteRect;
    protected Vector2 Position;
    protected Vector2 Scale;
    private Vector2 MinScale;
    protected string Name;
    public delegate void OnNodeDestroyed(Node node);
    public OnNodeDestroyed nodeDestroyedEvent;
    public System.Action<Node> _onDelete;

    public Node(string name, Vector2 position, Vector2 scale, System.Action<Node> onDelete) : this(name, position, scale)
    {
        _onDelete = onDelete;
    }

    public Node(string name, Vector2 position, Vector2 scale)
    {
        Name = name;
        Position = position;
        Scale = scale;
        MinScale = scale;
        VisualRect = new Rect(position, Scale);
        EditorGlobals.mouseDragEvent += ScaleVisual;
    }

    public virtual void Draw()
    {
        GUI.Box(VisualRect, Name);
        ScaleRect = new Rect(VisualRect.position, new Vector2(10, 10));
        ScaleRect.position += new Vector2(VisualRect.width - 10, VisualRect.height - 10);
        GUI.Box(ScaleRect, "");
        DeleteRect = new Rect(VisualRect.position, new Vector2(20, 20));
        if (GUI.Button(DeleteRect, "X"))
        {
            _onDelete?.Invoke(this);
        }
    }

    private void ScaleVisual()
    {
        var current = Event.current;
        if(ScaleRect.Contains(current.mousePosition))
        {
            var newScale = VisualRect.size + current.delta;
            VisualRect.size += current.delta;
            if (VisualRect.size.x <= MinScale.x)
                VisualRect.size = new Vector2(MinScale.x, VisualRect.size.y);
            if (VisualRect.size.y <= MinScale.y)
                VisualRect.size = new Vector2(VisualRect.size.x, MinScale.y);
            current.Use();
        }
    }    
}

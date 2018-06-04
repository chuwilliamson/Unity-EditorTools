using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DraggableNode : Node
{
    private bool IsDraggable;
    
    public DraggableNode(string name, Vector2 position, Vector2 scale, System.Action<Node> onDelete) : this(name, position, scale)
    {
        _onDelete = onDelete;
    }
    public DraggableNode(string name, Vector2 position, Vector2 scale) : base(name, position, scale)
    {
        EditorGlobals.mouseDragEvent += Drag;        
    }

    public override void Draw()
    {
        base.Draw();
        var toggleRect = new Rect(VisualRect.position + new Vector2(VisualRect.width - 20, 0),
            new Vector2(20, 20));
        IsDraggable = GUI.Toggle(toggleRect, IsDraggable, "");
    }

    private void Drag()
    {
        var current = Event.current;
        if (VisualRect.Contains(current.mousePosition) && IsDraggable)
        {
            VisualRect.position += current.delta;
            current.Use();
        }
    }
}

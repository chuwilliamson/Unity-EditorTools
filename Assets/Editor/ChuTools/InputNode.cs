using System;
using ChuTools;
using Interfaces;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

/// <summary>
///     this node will have one out
/// </summary>
public class InputNode : INode
{
    private readonly System.Action<IConnectionOut> _onConnectionOut;
    public InputNode(Action<IConnectionOut> onConnectionOut)
    {
        Value = 0;
        _onConnectionOut = onConnectionOut;
        OutConnection = new OutConnection(this);
        _onConnectionOut?.Invoke(OutConnection);
    }


    private readonly IConnectionOut OutConnection;

    public int Value { get; set; }
}

public class OutConnection : IConnectionOut
{
    public OutConnection(INode inputNode)
    {
        Node = inputNode;
    }

    public int Value => Node?.Value ?? 0;

    public INode Node { get; set; }
}


public class DisplayNode : INode
{
    private readonly IConnectionIn _inConnection;
    private readonly Action<IConnectionIn> _onConnectionIn; 

    public DisplayNode(IConnectionIn inConnection, Action<IConnectionIn> onConnectionIn) : this(inConnection)
    {
        _onConnectionIn = onConnectionIn;
    }

    public DisplayNode(IConnectionIn inConnection)
    {
        _inConnection = inConnection;
    }

    public int Value
    {
        get
        {
            if (_inConnection == null)
                return 0;
            return _inConnection.Value;
        }
        set { Debug.LogWarning("no you shouldn't be setting the inconnection value through the node"); }
    }
}

public class InConnection : IConnectionIn
{
    public InConnection(IConnectionOut outConnection)
    {
        Out = outConnection;
    }

    public IConnectionOut Out { get; set; }

    public int Value
    {
        get { return Out.Value; }
        set { }
    }
}


public class UIDisplayNode : UIElement
{
    private INode _node;

    public UIDisplayNode()
    {
        InitGUI();
    }

    public UIDisplayNode(Vector2 pos, Vector2 size, Action<IConnectionIn> onConnectionIn) : this(pos, size)
    {
        _node = new DisplayNode(null, onConnectionIn);
    }

    public UIDisplayNode(Vector2 pos, Vector2 size) : this()
    {
        Rect = new Rect(pos, size);
    }

    public bool Connect(IConnectionOut outConnection)
    {
        _node = new DisplayNode(new InConnection(outConnection));
        return true;
    }

    private void InitGUI()
    {
        Style = new GUIStyle("CN Box")
        {
            alignment = TextAnchor.LowerLeft,
            fontSize = 25
        };
        Content = new GUIContent("Display Node");

        NodeEditorWindow.OnMouseDrag += e =>
        {
            if (!Rect.Contains(e.mousePosition)) return;
            if (GUIUtility.hotControl == ControlId)
            {
                Rect.position += e.delta;
                GUI.changed = true;
                e.Use();
            }
        };
    }

    public override void Draw()
    {
        base.Draw();

        GUILayout.BeginArea(Rect);
        GUILayout.Label("Value  ::  " + _node.Value);
        GUILayout.EndArea();
    }
}

public class UIInputNode : UIElement
{
    private INode _node;

    public UIInputNode(Vector2 pos, Vector2 size, Action<IConnectionOut> onOutConnection)
    {
        Rect = new Rect(pos, size);
        _node = new InputNode(onOutConnection);
        InitGUI();
    }

    private void InitGUI()
    {
        Style = new GUIStyle("CN Box");
        Content = new GUIContent("Input Node", Resources.Load<Texture2D>("white-square"));
        NodeEditorWindow.OnMouseDrag += e =>
        {
            if (!Rect.Contains(e.mousePosition)) return;
            if (GUIUtility.hotControl == ControlId)
            {
                Rect.position += e.delta;
                GUI.changed = true;
                e.Use();
            }
        };
    }

    public override void Draw()
    {
        base.Draw();

        GUILayout.BeginArea(Rect);
        _node.Value = EditorGUILayout.IntSlider("Value", _node.Value, 0, 10);
        GUILayout.EndArea();
    }
}
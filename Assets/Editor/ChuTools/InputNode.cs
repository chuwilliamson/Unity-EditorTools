using System;
using ChuTools;
using Interfaces;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

/// <summary>
/// this node will have one out
/// </summary>
public class InputNode : INode
{
    public InputNode(Action<IConnectionOut> onConnectionOut)
    {
        Value = 0;
        IConnectionOut outConnection = new OutConnection(this);
        onConnectionOut?.Invoke(outConnection);
    }


    public int Value { get; set; }
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


public class UIInConnection : UIElement
{
    public UIInConnection(Rect rect)
    {
        Rect = rect;
        Content = new GUIContent("Display Node: " + ControlId);
        SelectedStyle = new GUIStyle("CN Box") {alignment = TextAnchor.LowerLeft, fontSize = 25};

        NormalStyle = new GUIStyle("CN Box") {alignment = TextAnchor.LowerLeft, fontSize = 25};

        Style = NormalStyle;
    }

    public override void OnMouseUp(Event e)
    {
        base.OnMouseUp(e);
        if(Rect.Contains(e.mousePosition))
        {
            if(NodeEditorWindow.CurrentDrag == this) return;

            var @out = NodeEditorWindow.CurrentDrag as UIOutConnection;
            if(@out == null) return;
            NodeEditorWindow.ConnectionCreatedEvent?.Invoke(@out, this);
        }

    }

    //drag outconnection onto this
}
public class UIOutConnection : UIElement
{
    public UIOutConnection(Rect rect)
    {
        Rect = rect;
        Content = new GUIContent("Display Node: " + ControlId);
        SelectedStyle = new GUIStyle("CN Box") {alignment = TextAnchor.LowerLeft, fontSize = 25};

        NormalStyle = new GUIStyle("CN Box") {alignment = TextAnchor.LowerLeft, fontSize = 25};

        Style = NormalStyle;
    }


    public override void OnMouseDown(Event e)
    {
        base.OnMouseDown(e);
        if (Rect.Contains(e.mousePosition))
        {
            Debug.Log("set current");
            NodeEditorWindow.CurrentDrag = this;
        }

    }

    public override void OnMouseUp(Event e)
    {
        base.OnMouseUp(e);
        if (NodeEditorWindow.CurrentDrag == this)
            NodeEditorWindow.CurrentDrag = null;
    }
}

public class UIDisplayNode : UIElement
{
    private readonly UIInConnection _in;
    private INode _node;

    public UIDisplayNode(Vector2 pos, Vector2 size, Action<IConnectionIn> onConnectionIn)
    {
        _node = new DisplayNode(null, onConnectionIn);
        Rect = new Rect(pos, size);
        _in = new UIInConnection(new Rect(Rect.position, new Vector2(50, 50)));
        Content = new GUIContent("In" + ControlId);
        SelectedStyle = new GUIStyle("CN Box")
        {
            alignment = TextAnchor.LowerLeft,
            fontSize = 12
        };

        NormalStyle = new GUIStyle("CN Box")
        {
            alignment = TextAnchor.LowerLeft,
            fontSize = 12
        };

        Style = NormalStyle;
    }

    public bool Connect(IConnectionOut outConnection)
    {
        _node = new DisplayNode(new InConnection(outConnection));

        return true;
    }

    public void Disconnect()
    {
        _node = null;
    }


    public override void Draw()
    {
        base.Draw();
        _in.Rect = new Rect(Rect.position.x - 55, Rect.position.y, 50, 50);
        _in?.Draw();
        GUILayout.BeginArea(Rect);
        var value = _node?.Value;

        GUILayout.Label("Value  ::  " + value);

        GUILayout.EndArea();
    }
}

public class UIInputNode : UIElement
{
    private readonly INode _node;
    private readonly UIOutConnection _out;
    public UIInputNode(Vector2 pos, Vector2 size, Action<IConnectionOut> onOutConnection)
    {
        Rect = new Rect(pos, size);
        _node = new InputNode(onOutConnection);
        Content = new GUIContent("Input Node: " + ControlId);
        SelectedStyle = new GUIStyle("CN Box")
        {
            alignment = TextAnchor.LowerRight,
            fontSize = 25,
            normal = new GUIStyleState { textColor = Color.green },

        };

        NormalStyle = new GUIStyle("CN Box")
        {
            alignment = TextAnchor.LowerRight,
            fontSize = 25,
            normal = new GUIStyleState { textColor = Color.cyan }
        };

        Style = NormalStyle;

        _out = new UIOutConnection(new Rect(Rect.position, new Vector2(50, 50)));

    }

    public override void Draw()
    {
        base.Draw();
        _out.Rect = new Rect(Rect.position.x + Rect.width + 55, Rect.position.y, 50, 50);
        _out.Draw();

        GUILayout.BeginArea(Rect);
        _node.Value = EditorGUILayout.IntSlider("Value", _node.Value, 0, 10);
        GUILayout.EndArea();
    }
}
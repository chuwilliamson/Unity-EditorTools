﻿using System;
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
    public InputNode()
    {
        Value = 0;

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

    public DisplayNode(IConnectionIn inConnection)
    {
        _inConnection = inConnection;
    }

    public int Value
    {
        get { return _inConnection?.Value ?? 0; }
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            Debug.LogWarning("no you shouldn't be setting the inconnection value through the node");
        }
    }
}

public class UIBezierConnection : IDrawable
{
    public IDrawable @in;
    public IDrawable @out;

    public UIBezierConnection(IDrawable @in, IDrawable @out)
    {
        this.@in = @in;
        this.@out = @out;
    }

    public Rect Rect => @out.Rect;

    public void Draw()
    {
        Chutilities.DrawNodeCurve(@in.Rect.center, @out.Rect.center);
    }
}

public class UIConnectionPoint : UIElement
{
}


public class UIDisplayNode : UIElement
{
    private readonly UIInConnectionPoint _in;
    private INode _node;

    public UIDisplayNode(Vector2 pos, Vector2 size)
    {
        Rect = new Rect(pos, size);
        _node = new DisplayNode(null);
        _in = new UIInConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), Connect);

        Content = new GUIContent("In" + ControlId);
        SelectedStyle = new GUIStyle("flow node 1 on") { alignment = TextAnchor.LowerLeft, fontSize = 12 };
        NormalStyle = new GUIStyle("flow node 1") { alignment = TextAnchor.LowerLeft, fontSize = 12 };
        Style = NormalStyle;
    }

    public void Connect(IConnectionOut outConnection)
    {
        if (outConnection == null)
            return;
        _node = new DisplayNode(new InConnection(outConnection));
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
public class UIInConnectionPoint : UIConnectionPoint
{
    public IConnectionOut Out;
    //drag outconnection onto this
    public UIInConnectionPoint(Rect rect, System.Action<IConnectionOut> cb)
    {
        Rect = rect;
        ConnectionCallback = cb;
        Content = new GUIContent("IN:  " + ControlId);
        SelectedStyle = new GUIStyle("CN Box") { alignment = TextAnchor.LowerLeft, fontSize = 12 };
        NormalStyle = new GUIStyle("CN Box") { alignment = TextAnchor.LowerLeft, fontSize = 12 };
        Style = NormalStyle;
    }

    public override void OnMouseDrag(Event e)
    {
        if (Rect.Contains(e.mousePosition))
        {
            if (NodeEditorWindow.CurrentSendingDrag == null) return;
            NodeEditorWindow.CurrentAcceptingDrag = this;
            GUI.changed = true;
        }
    }

    public Action<IConnectionOut> ConnectionCallback;
    public override void OnMouseUp(Event e)
    {
        base.OnMouseUp(e);
        var @out = NodeEditorWindow.CurrentSendingDrag;
        if (Rect.Contains(e.mousePosition))
        {
            NodeEditorWindow.ConnectionCreatedEvent(@out, this);
            ConnectionCallback.Invoke(Out);
        }
    }
}

public class UIOutConnectionPoint : UIConnectionPoint
{
    public IConnectionOut _out;

    public UIOutConnectionPoint(Rect rect, IConnectionOut @out) : this(rect)
    {
        _out = @out;
    }
    public UIOutConnectionPoint(Rect rect)
    {
        Rect = rect;
        Content = new GUIContent("Out: " + ControlId);
        SelectedStyle = new GUIStyle("CN Box") { alignment = TextAnchor.LowerLeft, fontSize = 8 };
        NormalStyle = new GUIStyle("CN Box") { alignment = TextAnchor.LowerLeft, fontSize = 8 };
        Style = NormalStyle;
    }

    public override void OnMouseUp(Event e)
    {
        base.OnMouseUp(e);
        NodeEditorWindow.CurrentAcceptingDrag?.ConnectionCallback?.Invoke(_out);

        if (NodeEditorWindow.CurrentAcceptingDrag != null) return;

        NodeEditorWindow.CurrentSendingDrag = null;
    }

    private void ConnectionCallback(IConnectionOut connectionOut)
    {

    }

    public override void OnMouseDown(Event e)
    {
        base.OnMouseDown(e);
        if (!Rect.Contains(e.mousePosition)) return;
        NodeEditorWindow.CurrentSendingDrag = this;
    }
}

public class UINode : UIElement
{
    private INode _input;
    private INode _display;
    private UIInConnectionPoint _in;
    private UIOutConnectionPoint _out;

    public UINode(Vector2 pos, Vector2 size)
    {
        Rect = new Rect(pos, size);
        _input = new InputNode();
        _out = new UIOutConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), new OutConnection(_input));
        _display = null;
        _in = new UIInConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), Connect);


        Content = new GUIContent("Input Node: " + ControlId);
        SelectedStyle = new GUIStyle("flow node 0 on") { alignment = TextAnchor.LowerRight, fontSize = 25 };
        NormalStyle = new GUIStyle("flow node 0") { alignment = TextAnchor.LowerRight, fontSize = 25 };
        Style = NormalStyle;
    }

    public void Connect(IConnectionOut outConnection)
    {
        if (outConnection == null)
            return;
        _display = new DisplayNode(new InConnection(outConnection));
        _out._out = outConnection;
    }

    public override void Draw()
    {
        base.Draw();
        _in.Rect = new Rect(Rect.position.x - 55, Rect.position.y, 50, 50);
        _in?.Draw();
        _out.Rect = new Rect(Rect.position.x + Rect.width, Rect.position.y, 50, 50);
        _out?.Draw();

        GUILayout.BeginArea(Rect);
        if (_display != null)
        {
            _input.Value = _display.Value ;
            GUILayout.Label("In " + _input.Value);
            GUILayout.Label("Out " + _display.Value);
        }

        else
        {
            _input.Value = EditorGUILayout.IntSlider("In ", _input.Value, 0, 10);

        }

        GUILayout.EndArea();

    }
}

public class UIInputNode : UIElement
{
    private readonly INode _node;
    private readonly UIOutConnectionPoint _out;

    public UIInputNode(Vector2 pos, Vector2 size)
    {
        Rect = new Rect(pos, size);
        _node = new InputNode();
        _out = new UIOutConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), new OutConnection(_node));

        Content = new GUIContent("Input Node: " + ControlId);
        SelectedStyle = new GUIStyle("flow node 0 on") { alignment = TextAnchor.LowerRight, fontSize = 25 };
        NormalStyle = new GUIStyle("flow node 0") { alignment = TextAnchor.LowerRight, fontSize = 25 };

        Style = NormalStyle;
    }

    public override void Draw()
    {
        base.Draw();
        _out.Rect = new Rect(Rect.position.x + Rect.width, Rect.position.y, 50, 50);
        _out.Draw();

        GUILayout.BeginArea(Rect);
        _node.Value = EditorGUILayout.IntSlider("Value", _node.Value, 0, 10);
        GUILayout.EndArea();

    }
}
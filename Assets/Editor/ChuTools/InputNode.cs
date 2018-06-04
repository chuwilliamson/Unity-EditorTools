using System;
using ChuTools;
using Interfaces;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming




///Process:::
/// Input node manipulates information
/// OUT connection will carry that data
/// IN connection will be connected to the OUT connection
/// Display node will display the information from the OUT connection
/// 
/// 
/// 
/// <summary>
/// this node will have one out
/// It will manipulate it's data
/// The Out Connection will take this data and transfer it to an inconnection 
/// </summary>
public class InputNode : INode
{
    public InputNode() { Value = 0; }
    public int Value { get; set; }
}
public class OutConnection : IConnectionOut
{
    public OutConnection(INode inputNode) { Node = inputNode; }
    public int Value => Node?.Value ?? 0;
    public INode Node { get; set; }
}

public class InConnection : IConnectionIn
{
    public InConnection(IConnectionOut outConnection) { Out = outConnection; }
    public IConnectionOut Out { get; set; }
    public int Value { get { return Out.Value; } }
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
        set { Debug.LogWarning("no you shouldn't be setting the inconnection value through the node" + value.ToString()); }
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
    private string _name;
}

public delegate bool ConnectionResponse(IConnectionOut co);

public class UIDisplayNode : UIElement
{
    private readonly UIInConnectionPoint _in;
    private INode _node;

    public UIDisplayNode(Vector2 pos, Vector2 size) : base("In", pos, size)
    {
        _node = new DisplayNode(null);
        _in = new UIInConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), Connect);

    }

    public bool Connect(IConnectionOut outConnection)
    {
        if (outConnection == null)
            return false;
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
public class UIInConnectionPoint : UIConnectionPoint
{
    private readonly ConnectionResponse _connectionResponse;

    //drag outconnection onto this
    public UIInConnectionPoint(Rect rect, ConnectionResponse cb)
    {
        Rect = rect;
        _connectionResponse = cb;
        Content = new GUIContent("IN:  " + ControlId);
        SelectedStyle = new GUIStyle("CN Box") { alignment = TextAnchor.LowerLeft, fontSize = 12 };
        NormalStyle = new GUIStyle("CN Box") { alignment = TextAnchor.LowerLeft, fontSize = 12 };
        Style = NormalStyle;
    }

    public bool ValidateConnection(IConnectionOut @out)
    {
        return _connectionResponse.Invoke(@out);
    }

    public override void OnMouseDrag(Event e)
    {
        if (!Rect.Contains(e.mousePosition)) return;
        if (NodeEditorWindow.CurrentSendingDrag == null) return;
        NodeEditorWindow.CurrentAcceptingDrag = this;
        GUI.changed = true;
    }

}

public class UIOutConnectionPoint : UIConnectionPoint
{
    private IConnectionOut _out;

    public IConnectionOut Out
    {
        get { return _out; }
        set { _out = value; }
    }

    public UIOutConnectionPoint(Rect rect, IConnectionOut @out)
    {
        _out = @out;
        Rect = rect;
        Content = new GUIContent("Out: " + ControlId);
        SelectedStyle = new GUIStyle("CN Box") { alignment = TextAnchor.LowerLeft, fontSize = 8 };
        NormalStyle = new GUIStyle("CN Box") { alignment = TextAnchor.LowerLeft, fontSize = 8 };
        Style = NormalStyle;
    }

    public override void OnMouseUp(Event e)
    {
        base.OnMouseUp(e);
        var @in = NodeEditorWindow.CurrentAcceptingDrag;
        var @out = NodeEditorWindow.CurrentSendingDrag;
        if (@in == null) return;
        if (@out != this) return;

        Debug.Log("doit");
        NodeEditorWindow.RequestConnection(this, _out);
    }

    public override void OnMouseDown(Event e)
    {
        base.OnMouseDown(e);
        if (!Rect.Contains(e.mousePosition)) return;
        NodeEditorWindow.CurrentSendingDrag = this;
    }
}


public class UIInputNode : UIElement
{
    private readonly INode _node;
    private readonly UIOutConnectionPoint _out;

    public UIInputNode(Vector2 pos, Vector2 size) : base("Input Node: ", pos, size)
    {
        _node = new InputNode();
        _out = new UIOutConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), new OutConnection(_node));
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

/// <summary>
/// This node will represent a pass through node
/// It will receive data from it's in connection
/// That data will then be changed by the node implementation
/// 
/// </summary>
public class UINode : UIElement
{
    private readonly INode _input;
    private readonly INode _transformation;
    private INode _display;
    private readonly UIInConnectionPoint _in;
    private readonly UIOutConnectionPoint _out;

    public UINode(Vector2 pos, Vector2 size) : base("Transformation Node", pos, size)
    {
        _display = new DisplayNode(null);
        _in = new UIInConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), Connect);

        _transformation = new InputNode();
        _input = new InputNode();
        _out = new UIOutConnectionPoint(new Rect(Rect.position, new Vector2(50, 50)), new OutConnection(_transformation));
    }

    public bool Connect(IConnectionOut outConnection)
    {
        if (outConnection == null) return false;
        _display = new DisplayNode(new InConnection(outConnection));
        return true;
    }

    private int slidervalue;
    public override void Draw()
    {
        base.Draw();
        _in.Rect = new Rect(Rect.position.x - 55, Rect.position.y, 50, 50);
        _in?.Draw();
        _out.Rect = new Rect(Rect.position.x + Rect.width, Rect.position.y, 50, 50);
        _out?.Draw();

        GUILayout.BeginArea(Rect);


        slidervalue = EditorGUILayout.IntSlider("Modifier", slidervalue, 0, 10);
        _input.Value = slidervalue;
        _transformation.Value = _display.Value + _input.Value;
        GUILayout.Label("Input " + _input?.Value);
        GUILayout.Label("Display " + _display?.Value);
        GUILayout.Label("Transformation " + _transformation.Value, new GUIStyle(Style) { fontSize = 15, alignment = TextAnchor.LowerLeft });
        GUILayout.EndArea();
    }
}


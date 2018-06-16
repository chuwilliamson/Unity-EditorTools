using Interfaces;
using System;
using UnityEngine;

public class GridMenu : IDrawable
{
    public GridMenu(Rect rect, IEventSystem eventSystem, System.Action drawCallback)
    {
        _rect = rect;
        DrawCallback = drawCallback;
    }

    public void Draw()
    {
        GUILayout.BeginArea(_rect, new GUIContent(GUIUtility.GetControlID(FocusType.Passive, _rect).ToString()), NormalStyle);
        DrawCallback();
        GUILayout.EndArea();
    }

    Rect IDrawable.Rect => _rect;

    private Rect _rect;

    public Action DrawCallback;

    public GUIStyle NormalStyle => new GUIStyle("CN Box")
    {
        fontSize = 15,
        alignment = TextAnchor.UpperCenter,
        padding = new RectOffset(15, 15, 15, 15)
    };
}

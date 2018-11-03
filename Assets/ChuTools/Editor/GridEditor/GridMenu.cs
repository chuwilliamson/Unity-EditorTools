using System;
using ChuTools.NodeEditor.Interfaces;
using UnityEngine;

namespace ChuTools.GridEditor
{
    public class GridMenu : IDrawable
    {
        private readonly Rect _rect;

        public Action DrawCallback;

        public GridMenu(Rect rect, IEventSystem eventSystem, Action drawCallback)
        {
            _rect = rect;
            eventSystem.OnMouseDown += e => { Debug.Log("mouse down"); };
            DrawCallback = drawCallback;
        }

        public GUIStyle NormalStyle => new GUIStyle("CN Box")
        {
            fontSize = 15,
            alignment = TextAnchor.UpperCenter,
            padding = new RectOffset(15, 15, 15, 15)
        };

        public void Draw()
        {
            GUILayout.BeginArea(_rect, new GUIContent(GUIUtility.GetControlID(FocusType.Passive, _rect).ToString()),
                NormalStyle);
            DrawCallback();
            GUILayout.EndArea();
        }

        Rect IDrawable.Rect => _rect;
    }
}
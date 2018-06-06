using System;
using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIOutConnectionPoint : UIConnectionPoint
    {
        public UIOutConnectionPoint(Rect rect, IConnectionOut @out)
        {
            Out = @out;
            Rect = rect;
            Content = new GUIContent("Out: " + ControlId);
            SelectedStyle = new GUIStyle("CN Box") {alignment = TextAnchor.LowerLeft, fontSize = 8};
            NormalStyle = new GUIStyle("CN Box") {alignment = TextAnchor.LowerLeft, fontSize = 8};
            Style = NormalStyle;
        }

        public IConnectionOut Out { get; set; }

        public override void OnMouseUp(Event e)
        {
            base.OnMouseUp(e);
            var @in = NodeEditorWindow.CurrentAcceptingDrag;
            var @out = NodeEditorWindow.CurrentSendingDrag;
            if (@in == null) return;
            if (@out != this) return;

            Debug.Log("doit");
            NodeEditorWindow.RequestConnection(this, Out);
        }

        public override void OnMouseDown(Event e)
        {
            base.OnMouseDown(e);
            if (!Rect.Contains(e.mousePosition)) return;
            NodeEditorWindow.CurrentSendingDrag = this;
        }
    }
}
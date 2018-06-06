using System;
using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIOutConnectionPoint : UIElement
    {
        public UIOutConnectionPoint(Rect rect, IConnectionOut @out)
        {
            Out = @out;
            Base("Out", "CN Box", "CN Box", rect);
        }

        private IConnectionOut Out { get; set; }

        public override void OnMouseUp(Event e)
        {
            base.OnMouseUp(e);
            var @in = NodeEditorWindow.CurrentAcceptingDrag;
            var @out = NodeEditorWindow.CurrentSendingDrag;
            if (@in == null) return;
            if (@out != this) return;

            NodeEditorWindow.RequestConnection(this, Out);
        }

        public override void OnMouseDown(Event e)
        {
            base.OnMouseDown(e);
            if (!uRect.Contains(e.mousePosition)) return;
            NodeEditorWindow.CurrentSendingDrag = this;
        }
    }
}
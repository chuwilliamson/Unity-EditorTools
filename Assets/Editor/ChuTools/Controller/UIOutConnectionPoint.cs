using ChuTools.View;
using Interfaces;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public class UIOutConnectionPoint : UIElement
    {
        [JsonConstructor]
        public UIOutConnectionPoint(Rect rect, IConnectionOut @out)
        {
            Out = @out;
            Base(name: "Out", normalStyleName: "CN Box", selectedStyleName: "CN Box", rect: rect);
        }

        public override void OnMouseUp(Event e)
        {
            base.OnMouseUp(e);
            if (NodeEditorWindow.CurrentAcceptingDrag == null) return;
            if (NodeEditorWindow.CurrentSendingDrag != this) return;

            NodeEditorWindow.RequestConnection(this, Out);
        }

        public override void OnMouseDown(Event e)
        {
            base.OnMouseDown(e);
            if (!rect.Contains(e.mousePosition)) return;
            NodeEditorWindow.CurrentSendingDrag = this;
        }

        public IConnectionOut Out { get; set; }
    }
}
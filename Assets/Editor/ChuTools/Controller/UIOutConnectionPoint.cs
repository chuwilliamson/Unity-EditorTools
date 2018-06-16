using System;
using Interfaces;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public class UIOutConnectionPoint : UIElement
    {
        public UIOutConnectionPoint()
        {

        }

        public UIOutConnectionPoint(Rect rect)
        {
            Rect = rect;
            Base(name: "Out", normalStyleName: "U2D.pivotDot", selectedStyleName: "U2D.pivotDotActive", rect: rect);

        }

        [JsonConstructor]
        public UIOutConnectionPoint(Rect rect, IConnectionOut @out = null)
        {
            Out = @out;
            Base(name: "Out", normalStyleName: "U2D.pivotDot", selectedStyleName: "U2D.pivotDotActive", rect: rect);
        }

        public override void OnMouseDrag(Event e)
        {
            if(DragAndDrop.activeControlID == ControlId)
                GUI.changed = true;
        }

        protected override void OnDragUpdated(Event e)
        {
            base.OnDragUpdated(e);
            Debug.Log("on drag updated");
        }

        protected override void OnDragPerform(Event e)
        {
            base.OnDragPerform(e);
            Debug.Log("on drag perform");
        }

        protected override void OnDragExited(Event e)
        {
            base.OnDragExited(e);
            Debug.Log("on drag exited");
        }

        public override void OnMouseUp(Event e)
        {
            base.OnMouseUp(e);
            if(DragAndDrop.activeControlID == ControlId)
                DragAndDrop.activeControlID = 0;

            //if (NodeEditorWindow.CurrentAcceptingDrag == null) return;
            //if (NodeEditorWindow.CurrentSendingDrag != this) return;
            //NodeEditorWindow.RequestConnection(this, Out);
        }

        public override void OnMouseDown(Event e)
        {
            base.OnMouseDown(e);
            if(!Rect.Contains(e.mousePosition)) return;
            DragAndDrop.activeControlID = ControlId;
            DragAndDrop.SetGenericData("UIOutConnectionPoint", this);
            GUI.changed = true;

            //NodeEditorWindow.CurrentSendingDrag = this;
        }

        public IConnectionOut Out { get; set; }
    }
}
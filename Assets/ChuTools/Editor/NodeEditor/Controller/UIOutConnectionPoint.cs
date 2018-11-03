using ChuTools.NodeEditor.Interfaces;
using Newtonsoft.Json;
using System;
using UnityEditor;
using UnityEngine;

namespace ChuTools.NodeEditor.Controller
{
    [Serializable]
    public class UIOutConnectionPoint : UIElement 
    {
        public UIOutConnectionPoint(Rect rect)
        {
            Rect = rect;
            Base(name: "Out", normalStyleName: "U2D.pivotDot", selectedStyleName: "U2D.pivotDotActive", rect: rect);
            NormalStyle.imagePosition = ImagePosition.ImageOnly;
            SelectedStyle.imagePosition = ImagePosition.ImageOnly;
        }

        [JsonConstructor]
        public UIOutConnectionPoint(Rect rect, IConnectionOut @out = null)
        {
            Out = @out;
            Base(name: "Out", normalStyleName: "U2D.pivotDot", selectedStyleName: "U2D.pivotDotActive", rect: rect);
            NormalStyle.imagePosition = ImagePosition.ImageOnly;
            SelectedStyle.imagePosition = ImagePosition.ImageOnly;
        }

        public IConnectionOut Out { get; set; }

        public override void Draw()
        {
            var width = 0f;
            var height = 0f;

            NormalStyle.CalcMinMaxWidth(Content, out width, out height);
            Rect.height = height;
            Rect.width = width;
            GUI.Box(Rect, GUIContent.none, EditorStyles.miniButton);
            base.Draw();
        }

        public override void OnMouseDrag(Event e)
        {
            if (DragAndDrop.activeControlID == ControlId) GUI.changed = true;
        }

        public override void OnMouseDown(Event e)
        {
            if (!Rect.Contains(e.mousePosition)) return;
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.activeControlID = ControlId;
            DragAndDrop.StartDrag("Connecting...");
            DragAndDrop.SetGenericData("UIOutConnectionPoint", this);
            DragAndDrop.SetGenericData("OutData", Out);
            GUI.changed = true;
        }
    }
}
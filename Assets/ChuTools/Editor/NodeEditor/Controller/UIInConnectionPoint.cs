using Newtonsoft.Json;
using System;
using UnityEditor;
using UnityEngine;

namespace ChuTools.NodeEditor.Controller
{
    [Serializable]
    public class UIInConnectionPoint : UIElement
    {
        [NonSerialized] private readonly ConnectionResponse m_connectionResponse;

        [NonSerialized] private readonly DisconnectResponse m_disconnectResponse;

        public UIInConnectionPoint(Rect rect)
        {
            Base(name: "In", normalStyleName: "U2D.pivotDot", selectedStyleName: "U2D.pivotDotActive", rect: rect);
            NormalStyle.imagePosition = ImagePosition.ImageOnly;
            SelectedStyle.imagePosition = ImagePosition.ImageOnly;
        }

        [JsonConstructor]
        public UIInConnectionPoint(Rect rect, ConnectionResponse cb, DisconnectResponse disconnectResponse)
        {
            m_connectionResponse = cb;
            m_disconnectResponse = disconnectResponse;
            Base(name: "In", normalStyleName: "U2D.pivotDot", selectedStyleName: "U2D.pivotDotActive", rect: rect);
            NormalStyle.imagePosition = ImagePosition.ImageOnly;
            SelectedStyle.imagePosition = ImagePosition.ImageOnly;
        }

        protected override void OnContextClick(Event e)
        {
            if (!Rect.Contains(e.mousePosition))
                return;
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Disconnect"), false, () => { Debug.Log("nope"); });
            gm.ShowAsContext();
            e.Use();
        }

        protected override void OnDragUpdated(Event e)
        {
            if (Rect.Contains(e.mousePosition)) DragAndDrop.visualMode = DragAndDropVisualMode.Link;
        }

        protected override void OnDragPerform(Event e)
        {
            if (!Rect.Contains(e.mousePosition)) return;
            Debug.Log($"on drag perform {Name} {ControlId}");
            DragAndDrop.SetGenericData("UIInConnectionPoint", this);
            DragAndDrop.AcceptDrag();
        }

        public override void Draw()
        {
            GUI.Box(Rect, GUIContent.none, EditorStyles.miniButton);
            base.Draw();
        }

        public override void OnMouseUp(Event e)
        {
            if (!Rect.Contains(e.mousePosition)) return;
            Debug.Log($"on mouse up {Name} {ControlId}");
        }

        public override void OnMouseDrag(Event e)
        {
            if (!Rect.Contains(e.mousePosition))
                return;

            GUI.changed = true;
        }
    }
}
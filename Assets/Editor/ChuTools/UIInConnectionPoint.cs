using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [System.Serializable]
    public class UIInConnectionPoint : UIConnectionPoint
    {
        private readonly ConnectionResponse _connectionResponse;

        private bool connectionState;

        //drag outconnection onto this
        public UIInConnectionPoint(Rect rect, ConnectionResponse cb)
        {
            Rect = rect;
            _connectionResponse = cb;
            Content = new GUIContent("IN:  " + ControlId);
            SelectedStyle = new GUIStyle("CN Box") { alignment = TextAnchor.LowerLeft, fontSize = 8 };
            NormalStyle = new GUIStyle("CN Box") { alignment = TextAnchor.LowerLeft, fontSize = 8 };
            Style = NormalStyle;
        }

        public bool ValidateConnection(IConnectionOut @out)
        {
            if (connectionState) return false;
            return connectionState = _connectionResponse.Invoke(@out);
        }

        public override void OnMouseDrag(Event e)
        {
            if (!Rect.Contains(e.mousePosition)) return;
            if (NodeEditorWindow.CurrentSendingDrag == null) return;
            NodeEditorWindow.CurrentAcceptingDrag = this;
            GUI.changed = true;
        }
    }
}
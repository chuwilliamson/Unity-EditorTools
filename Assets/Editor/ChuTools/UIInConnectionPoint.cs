using System;
using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIInConnectionPoint : UIElement
    {
        private readonly ConnectionResponse _connectionResponse;

        private bool _connectionState;

        //drag outconnection onto this
        public UIInConnectionPoint(Rect rect, ConnectionResponse cb)
        {
            _connectionResponse = cb;
            Base("In", "CN Box", "CN Box", rect);
        }

        public bool ValidateConnection(IConnectionOut @out)
        {
            if (_connectionState) return false;
            return _connectionState = _connectionResponse.Invoke(@out);
        }

        public override void OnMouseDrag(Event e)
        {
            if (!rect.Contains(e.mousePosition)) return;
            if (NodeEditorWindow.CurrentSendingDrag == null) return;
            NodeEditorWindow.CurrentAcceptingDrag = this;
            GUI.changed = true;
        }
    }
}
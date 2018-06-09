using ChuTools.View;
using Interfaces;
using Newtonsoft.Json;
using System;
using JeremyTools;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public class UIInConnectionPoint : UIElement
    {
        [JsonConstructor]
        public UIInConnectionPoint(Rect rect, ConnectionResponse cb)
        {
            ConnectionState = false;
            _connectionResponse = cb;
            Base(name: "In", normalStyleName: "CN Box", selectedStyleName: "CN Box", rect: rect);
        }

        public bool ValidateConnection(IConnectionOut @out)
        {
            if (ConnectionState) return false;
            return ConnectionState = _connectionResponse.Invoke(@out);
        }

        public override void OnMouseDrag(Event e)
        {
            if (!rect.Contains(e.mousePosition))//this fixes the dragging
                                                //this is bad because we dont want the nodes affecting the
                                                //window state
            {
                if (NodeEditorWindow.CurrentAcceptingDrag == this)
                    NodeEditorWindow.CurrentAcceptingDrag = null;
                return;
            }

            if (NodeEditorWindow.CurrentSendingDrag == null) return;
            NodeEditorWindow.CurrentAcceptingDrag = this;
            GUI.changed = true;
        }

        [NonSerialized]
        private ConnectionResponse _connectionResponse;
        public bool ConnectionState { get; set; }
    }
}
using System;
using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIInConnectionPoint : UIElement
    {
        protected ConnectionResponse ConnectionResponse { get; set; }
        public  bool ConnectionState { get; set; }

        //drag outconnection onto this
        public UIInConnectionPoint(Rect rect, ConnectionResponse cb)
        {
            ConnectionState = false;
            ConnectionResponse = cb;
            Base(name: "In", normalStyleName: "CN Box", selectedStyleName: "CN Box", rect: rect);
        }

        public bool ValidateConnection(IConnectionOut @out)
        {
            if (ConnectionState) return false;
            return ConnectionState = ConnectionResponse.Invoke(@out);
        }

        public override void OnMouseDrag(Event e)
        {
            if (!rect.Contains(e.mousePosition)) //this fixes the dragging
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
    }
}
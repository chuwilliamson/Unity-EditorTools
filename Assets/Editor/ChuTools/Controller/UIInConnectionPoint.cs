using System;
using ChuTools.View;
using Interfaces;
using JeremyTools;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public class UIInConnectionPoint : UIElement
    {

        [JsonConstructor]
        public UIInConnectionPoint(Rect rect, ConnectionResponse cb, DisconnectResponse disconnectResponse)
        {
            ConnectionState = false;
            _connectionResponse = cb;
            _disconnectResponse = disconnectResponse;
            Base(name: "In", normalStyleName: "CN Box", selectedStyleName: "CN Box", rect: rect);
        }

        public UIInConnectionPoint(string name, string normalStyle, string selectedStyle, Rect rect,
            ConnectionResponse cb, DisconnectResponse disconnectResponse)
        {
            ConnectionState = false;
            _connectionResponse = cb;
            _disconnectResponse = disconnectResponse;
            Base(rect, name, normalStyle, selectedStyle);
            NormalStyle.imagePosition = ImagePosition.ImageOnly;
            SelectedStyle.imagePosition = ImagePosition.ImageOnly;
        }

        public bool ValidateConnection(IConnectionOut @out)
        {

            if (ConnectionState)
            {
                return false;
            }

            ConnectionState = _connectionResponse.Invoke(@out, this);

            return ConnectionState;
        }


        protected override void OnContextClick(Event e)
        {
            if (!rect.Contains(e.mousePosition))
                return;
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Disconnect"), false, Disconnect);
            gm.ShowAsContext();
            e.Use();
        }

        public void Disconnect()
        {
            if (_disconnectResponse.Invoke(this))
            {
                Debug.Log("successful disconnect!");
                NodeEditorWindow.OnConnectionCancelRequest(this);
                ConnectionState = false;
            }
            else
            {
                Debug.Log("can not disconnect from this node");
            }
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

        [NonSerialized] private readonly ConnectionResponse _connectionResponse;

        [NonSerialized] private readonly DisconnectResponse _disconnectResponse;

        public bool ConnectionState { get; set; }
    }
}
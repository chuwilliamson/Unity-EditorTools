using System;
using ChuTools.View;
using Interfaces;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public class UIInConnectionPoint : UIElement
    {
        public UIInConnectionPoint()
        {

        }

        public UIInConnectionPoint(Rect rect)
        {
            Base(name: "In", normalStyleName: "U2D.pivotDot", selectedStyleName: "U2D.pivotDotActive", rect: rect);
            NormalStyle.imagePosition = ImagePosition.ImageOnly;
            SelectedStyle.imagePosition = ImagePosition.ImageOnly;
        }

        [JsonConstructor]
        public UIInConnectionPoint(Rect rect, ConnectionResponse cb, DisconnectResponse disconnectResponse)
        {
            ConnectionState = false;
            _connectionResponse = cb;
            _disconnectResponse = disconnectResponse;
            Base(name: "In", normalStyleName: "U2D.pivotDot", selectedStyleName: "U2D.pivotDotActive", rect: rect);
            NormalStyle.imagePosition = ImagePosition.ImageOnly;
            SelectedStyle.imagePosition = ImagePosition.ImageOnly;
        }


        public bool ValidateConnection(IConnectionOut @out)
        {

            if(ConnectionState)
                return false;

            ConnectionState = _connectionResponse.Invoke(@out, this);

            return ConnectionState;
        }


        protected override void OnContextClick(Event e)
        {
            if(!Rect.Contains(e.mousePosition))
                return;
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Disconnect"), false, Disconnect);
            gm.ShowAsContext();
            e.Use();
        }

        public void Disconnect()
        {
            if(_disconnectResponse.Invoke(this))
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
            if(!Rect.Contains(e.mousePosition))//this fixes the dragging
                //this is bad because we dont want the nodes affecting the
                //window state
                return;

            //if (NodeEditorWindow.CurrentSendingDrag == null) return;
            //NodeEditorWindow.CurrentAcceptingDrag = this;
            GUI.changed = true;
        }

        [NonSerialized] private readonly ConnectionResponse _connectionResponse;

        [NonSerialized] private readonly DisconnectResponse _disconnectResponse;

        public bool ConnectionState { get; set; }
    }
}
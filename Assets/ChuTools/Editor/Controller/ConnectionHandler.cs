using ChuTools.Scripts;
using System;

namespace ChuTools.EventSystems
{
    public class ConnectionHandler : IListener
    {
        public ConnectionHandler(Action<object[]> response, EventType eventType)
        {
            Response = response;
            Event = eventType == EventType.Connect ? ChuTools.Globals.ConnectedEvent : ChuTools.Globals.DisconnectEvent;
        }

        ~ConnectionHandler()
        {
            Unsubscribe();
        }

        public void OnEventRaised(object[] args)
        {
            Response.Invoke(args);
        }

        public void Subscribe()
        {
            Event.RegisterListener(this);
        }

        public void Unsubscribe()
        {
            Event.UnregisterListener(this);
        }

        public Action<object[]> Response;
        public ISubscribeable Event { get; set; }
    }
}
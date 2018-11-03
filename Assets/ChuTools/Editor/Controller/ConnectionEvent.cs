using ChuTools.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace ChuTools.Controller
{
    [System.Serializable]
    public class ConnectionEvent : ISubscribeable
    {
        public virtual void Raise(params object[] args)
        {
            for (var i = Listeners.Count - 1; i >= 0; i--)
            {
                Debug.Log("Raise Event " + args[0]);
                Listeners[i].OnEventRaised(args);
            }
        }

        public virtual void RegisterListener(IListener listener)
        {
            if (Listeners.Contains(listener))
            {
                Debug.LogError("listener is already in list");
                return;
            }

            Listeners.Add(listener);
        }

        public virtual void UnregisterListener(IListener listener)
        {
            if (!Listeners.Contains(listener))
            {
                Debug.LogError("listener is not in list");
                return;
            }

            Listeners.Remove(listener);
        }

        public List<IListener> Listeners = new List<IListener>();
    }
}
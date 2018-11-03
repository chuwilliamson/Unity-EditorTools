using System.Collections.Generic;
using UnityEngine;

namespace ChuTools.Scripts
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject, ISubscribeable
    {
        public List<IListener> Listeners = new List<IListener>();

        public void Raise(params object[] args)
        {
            for (var i = Listeners.Count - 1; i >= 0; i--)
                Listeners[i].OnEventRaised(args);
        }

        public void RegisterListener(IListener listener)
        {
            if (Listeners.Contains(listener))
            {
                Debug.LogError("listener is already in list");
                return;
            }

            Listeners.Add(listener);
        }

        public void UnregisterListener(IListener listener)
        {
            if (!Listeners.Contains(listener))
            {
                Debug.LogError("listener is not in list");
                return;
            }

            Listeners.Remove(listener);
        }
    }
}
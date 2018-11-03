using System.Collections.Generic;
using UnityEngine;

namespace ChuTools.Scripts
{
    public class GameEventListener : MonoBehaviour, IListener
    {
        public ISubscribeable GameEvent;
        public List<IResponder> Responses;

        public void OnEventRaised(object[] args)
        {
            Responses.ForEach(r => r.Invoke(args));
        }

        public void Subscribe()
        {
            GameEvent.RegisterListener(this);
        }

        public void Unsubscribe()
        {
            GameEvent.UnregisterListener(this);
        }

        public virtual void OnEnable()
        {
            Subscribe();
        }

        public virtual void OnDisable()
        {
            Unsubscribe();
        }
    }
}
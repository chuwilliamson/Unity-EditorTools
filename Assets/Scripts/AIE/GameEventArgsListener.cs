using System.Collections.Generic;
using UnityEngine;

public class GameEventArgsListener : MonoBehaviour, IListener
{
    public ISubscribeable GameEvent;
    public List<GameEventArgsResponse> Responses;

    //public virtual void OnEnable()
    //{
    //    Subscribe();
    //}

    //public virtual void OnDisable()
    //{
    //    Unsubscribe();
    //}

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


}
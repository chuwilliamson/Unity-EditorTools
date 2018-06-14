using System.Collections.Generic;
using UnityEngine;

public class GameEventArgsListener : MonoBehaviour, IListener
{
    public GameEventArgs GameEvent;
    public List<GameEventArgsResponse> Responses;

    public object sender;

    void OnEnable()
    {
        Subscribe();
    }

    void OnDisable()
    {
        Unsubscribe();
    }

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
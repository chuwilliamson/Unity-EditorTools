using UnityEngine;

public interface IListener
{
    void OnEventRaised(object[] args);
    void Subscribe();
    void Unsubscribe();
}
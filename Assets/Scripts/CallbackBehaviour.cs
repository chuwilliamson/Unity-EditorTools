
using UnityEngine;
using Object = UnityEngine.Object;
public class CallbackBehaviour : MonoBehaviour
{
    private GameEventArgs StartEvent => Resources.Load("OnStart") as GameEventArgs;

    public void Start()
    {
        StartEvent.Raise(gameObject);
    }


    public void OnStartCallback(object obj)
    {
        var objects = obj as object[];
        if (objects == null)
            return;
        var sender = objects[0] as GameObject;
        if (sender != gameObject)
            return;
        Debug.Log("start");
    }
}
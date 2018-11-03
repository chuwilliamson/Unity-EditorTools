using UnityEngine;

namespace ChuTools.Scripts
{
    public class CallbackBehaviour : MonoBehaviour
    {
        private GameEventArgs StartEvent => Resources.Load("OnStart") as GameEventArgs;

        public string Data { get; set; }

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
}
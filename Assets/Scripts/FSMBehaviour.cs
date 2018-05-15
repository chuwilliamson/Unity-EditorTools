using StatePattern.Contexts.Concrete;
using StatePattern.States.Concrete;
using UnityEngine;

namespace StatePattern
{
    public class FSMBehaviour : MonoBehaviour
    {
        readonly AntContext AntContext = new AntContext {CurrentState = new GoHomeState()};
        public string CurrentStateName;
        private void Update()
        {
            CurrentStateName = AntContext.CurrentState.ToString();
            AntContext.Update(this);
        }
    }
}
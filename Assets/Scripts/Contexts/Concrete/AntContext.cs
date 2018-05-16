
using System.Collections.Generic;
using States;
using UnityEngine;

namespace Contexts.Concrete
{
    [System.Serializable]
    public class AntContext : Context
    {
        public Stack<IState> Stack;

        public void Update(object sender)
        {
            CurrentState.Update(this);
        }

        public AntContext(IState initial)
        {
            Stack = new Stack<IState>();
            CurrentState = initial;
            CurrentState.Context = this;
            Stack.Push(CurrentState);
        }

        public override void PushState(IState state)
        {
            //push the incoming state onto the stack only if it is not what the current state is
            if (state == CurrentState)
                return;

            Stack.Push(item: state);

            CurrentState = Stack.Peek();
            CurrentState.OnEnter(this);
        }

        public override void PopState()
        {
            if (Stack.Count <= 1)
                return;
            CurrentState.OnExit(this);
            CurrentState = Stack.Peek();
        }
    }
}
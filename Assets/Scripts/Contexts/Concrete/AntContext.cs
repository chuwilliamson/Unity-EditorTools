
using System.Collections.Generic;
using States;

namespace Contexts.Concrete
{
    [System.Serializable]
    public class AntContext : Context
    {
        public Stack<IState> Stack => new Stack<IState>();

        public void Update(object sender)
        {
            CurrentState.Update(this);
        }
        public IState LastState { get; set; }
        public AntContext(IState initial)
        {
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
        }

        public override void PopState()
        {
            CurrentState.OnExit(this);
            LastState = Stack.Pop();
            CurrentState = Stack.Peek();
            CurrentState.OnEnter(this);
        }
    }
}
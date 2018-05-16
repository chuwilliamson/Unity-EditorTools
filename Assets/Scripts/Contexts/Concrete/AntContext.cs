
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

        public override void Push(IState state)
        {
            if (state == CurrentState || state == Stack.Peek())
                return;
            Stack.Push(item: state);
        }

        public override void Pop()
        {
            CurrentState = Stack.Pop();
        }
    }
}
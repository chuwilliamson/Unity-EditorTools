using States;

namespace Contexts
{
    public abstract class Context : IContext
    {
        public IState CurrentState { get; set; }

        public void ChangeState(IState state)
        {
            CurrentState.OnEnter(this);
            CurrentState = state;
            CurrentState.OnExit(this);
        }
    }
}
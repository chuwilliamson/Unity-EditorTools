using States;

namespace Contexts
{
    public abstract class Context : IContext, IStackContext
    {
        public IState CurrentState { get; set; }

        public void ChangeState(IState state)
        {
            CurrentState.OnEnter(this);
            CurrentState = state;
            CurrentState.OnExit(this);
        }

        public abstract void PushState(IState state);
        public abstract void PopState();
    }
}
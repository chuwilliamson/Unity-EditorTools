using StatePattern.States;

namespace StatePattern.Contexts
{
    public interface IContext
    {
        IState CurrentState { get; }
        void ChangeState(IState state);
    }
}
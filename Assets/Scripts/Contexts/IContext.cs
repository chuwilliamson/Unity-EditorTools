using States;

namespace Contexts
{
    public interface IContext
    {
        IState CurrentState { get; }
        void ChangeState(IState state);
    }
}
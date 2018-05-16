using States;

namespace Contexts
{
    public interface IContext : IStackContext //b/c the states are referencing some context
    {
        IState CurrentState { get; }
        void ChangeState(IState state);
    }

    public interface IStackContext
    {
        void Push(IState state);
        void Pop();
    }
}
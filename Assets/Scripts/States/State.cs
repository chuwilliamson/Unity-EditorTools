using Contexts;

namespace States
{
    public abstract class State : IState
    {
        public string CurrentStateName => GetType().Name;

        public IContext Context { get; set; }

        public string StateInfo { get; private set; }

        public virtual void Update(IContext context)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnEnter(IContext context)
        {
            StateInfo = "Enter => " + this;
        }

        public virtual void OnExit(IContext context)
        {
            StateInfo = "Exit => " + this;
        }
    }
}
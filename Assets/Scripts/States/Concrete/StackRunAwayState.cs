using Contexts;
using Contexts.Concrete;
using Data;

namespace States.Concrete
{
    public class StackRunAwayState : State
    {
        protected AntData Data;
        public override void OnEnter(IContext context)
        {
            Data = ((AntContext)context).Data;
            base.OnEnter(context);
        }
        public override void Update(IContext context)
        {
            Data.Velocity = (Data.AntPosition - Data.CursorPosition).normalized;
            if (Data.CursorDistance > 5)
            {
                context.PopState();
            }
        }
    }
}
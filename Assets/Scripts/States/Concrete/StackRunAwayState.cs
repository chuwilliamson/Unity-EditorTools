using Contexts;

namespace States.Concrete
{
    public class StackRunAwayState : RunAwayState
    {
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
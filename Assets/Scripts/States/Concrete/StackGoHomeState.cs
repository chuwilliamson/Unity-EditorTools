using Contexts;

namespace States.Concrete
{
    public class StackGoHomeState : GoHomeState
    {
        public override void Update(IContext context)
        {
            Data.Velocity = (Data.HomePosition - Data.AntPosition).normalized;

            if (Data.HomeDistance <= 1)
            {
                context.PopState();
                context.PushState(new StackFindLeafState { Context = context });
            }

            if (Data.CursorDistance <= 2)
            {
                context.PushState(new StackRunAwayState { Context = context });
            }
                

        }
    }
}
using StatePattern.Contexts;

namespace StatePattern.States.Concrete
{
    public class RunAwayState : State
    {
        float cursor_distance;
        
        public override void Update(IContext context)
        {
            if (cursor_distance > 120)
                context.ChangeState(new FindLeafState { Context = context });
        }
    }
}
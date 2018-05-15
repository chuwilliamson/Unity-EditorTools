using StatePattern.Contexts;

namespace StatePattern.States.Concrete
{
    public class GoHomeState : State
    {
        float home_distance;
        public override void Update(IContext context)
        {
            if(home_distance <= 10)
                context.ChangeState(new FindLeafState {Context = context});
        }

    }
}
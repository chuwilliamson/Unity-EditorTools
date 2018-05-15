using Contexts;
using Data;

namespace States.Concrete
{
    public class GoHomeState : State
    {
        float home_distance;
        private AntData Data => UnityEngine.Resources.Load<AntData>("AntData");
        public override void Update(IContext context)
        {
            home_distance = Data.HomeDistance;

            if(home_distance <= 10)
                context.ChangeState(new FindLeafState {Context = context});
        }
    }
}
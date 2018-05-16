using Contexts;
using Data;

namespace States.Concrete
{
    public class FindLeafState : State
    {
        protected AntData Data => UnityEngine.Resources.Load<AntData>("AntData");

        public override void Update(IContext context)
        {
            Data.Velocity = (Data.LeafPosition - Data.AntPosition).normalized; 

            if (Data.CursorDistance <= 2)
                context.ChangeState(new RunAwayState { Context = context });
            if (Data.LeafDistance <= 1)
                context.ChangeState(new GoHomeState { Context = context });
        }
    }
}
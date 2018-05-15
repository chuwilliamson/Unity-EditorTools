using Contexts;
using Data;

namespace States.Concrete
{
    public class FindLeafState : State
    {
        private AntData Data => UnityEngine.Resources.Load<AntData>("AntData");
        public float cursor_distance;

        public override void Update(IContext context)
        {
            Data.Velocity = (Data.LeafPosition - Data.AntPosition).normalized;

            cursor_distance = Data.CursorDistance;

            if (Data.CursorDistance <= 2)
                context.ChangeState(new RunAwayState { Context = context });
            if (Data.LeafDistance <= 1)
                context.ChangeState(new GoHomeState { Context = context });
        }
    }
}
using Contexts;
using Data;

namespace States.Concrete
{
    public class RunAwayState : State
    {
        public float cursor_distance;
        private AntData Data => UnityEngine.Resources.Load<AntData>("AntData");
        public override void Update(IContext context)
        {
            cursor_distance = Data.CursorDistance;
            if (cursor_distance > 120)
                context.ChangeState(new FindLeafState { Context = context });
        }
    }
}
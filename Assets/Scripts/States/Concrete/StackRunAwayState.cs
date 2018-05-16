using Contexts;
using Data;

namespace States.Concrete
{
    public class StackRunAwayState : State
    {
        protected AntData Data => UnityEngine.Resources.Load<AntData>("AntData");
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
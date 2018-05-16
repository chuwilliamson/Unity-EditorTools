using Contexts;
using Data;

namespace States.Concrete
{
    public class RunAwayState : State
    { 
        protected AntData Data => UnityEngine.Resources.Load<AntData>("AntData");
        public override void Update(IContext context)
        {
            Data.Velocity = (Data.AntPosition - Data.CursorPosition).normalized;
            
            if (Data.CursorDistance > 2)
                context.ChangeState(new FindLeafState { Context = context });
        }
    }
}
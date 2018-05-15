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
            cursor_distance = Data.CursorDistance;
            if (cursor_distance <= 120)
                context.ChangeState(new RunAwayState {Context = context});
        } 
    }
}
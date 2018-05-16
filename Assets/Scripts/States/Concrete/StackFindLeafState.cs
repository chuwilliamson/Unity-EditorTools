using Contexts;
using Data;

namespace States.Concrete
{
    public class StackFindLeafState : State
    {
        protected AntData Data => UnityEngine.Resources.Load<AntData>("AntData");
        public override void Update(IContext context)
        {
            Data.Velocity = (Data.LeafPosition - Data.AntPosition).normalized;
            if (Data.LeafDistance <= 1)
            {
                context.PopState();
                context.PushState(new StackCollectLeafState { Context = context });
            }
            if (Data.CursorDistance <= 2)
                context.PushState(new StackRunAwayState { Context = context });   
        }
    }
}
using Contexts;

namespace States.Concrete
{
    public class StackFindLeafState : FindLeafState
    {
        public override void Update(IContext context)
        {
            Data.Velocity = (Data.LeafPosition - Data.AntPosition).normalized;

            if (Data.CursorDistance <= 2)
                context.Push(new StackRunAwayState { Context = context });
            if (Data.LeafDistance <= 1)
            {
                context.Pop();
                context.Push(new StackGoHomeState { Context = context });
            }
                
        }
    }
}
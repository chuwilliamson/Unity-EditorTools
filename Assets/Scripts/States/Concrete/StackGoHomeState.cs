using System.Collections.Generic;
using Contexts;
using Contexts.Concrete;
using Data;
using UnityEngine;

namespace States.Concrete
{
    public class StackGoHomeState : State
    {
        protected AntData Data;

        public override void OnEnter(IContext context)
        {
            Data = ((AntContext)context).Data;
            base.OnEnter(context);
        }

        public override void Update(IContext context)
        {
            Data.Velocity = (Data.HomePosition - Data.AntPosition).normalized;
            if (Data.HomeDistance <= 1)
            {
                context.PopState();
                context.PushState(new StackDropOffState { Context = context });
            }
            if (Data.CursorDistance <= 2)
                context.PushState(new StackRunAwayState { Context = context });
        }
    }
}
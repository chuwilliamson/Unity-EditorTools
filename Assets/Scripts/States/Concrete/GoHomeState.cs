﻿using Contexts;
using Data;

namespace States.Concrete
{
    public class GoHomeState : State
    {
        protected AntData Data => UnityEngine.Resources.Load<AntData>("AntData");

        public override void Update(IContext context)
        {
            Data.Velocity = (Data.HomePosition - Data.AntPosition).normalized;

            if (Data.HomeDistance <= 2)
                context.ChangeState(new FindLeafState { Context = context });
            if (Data.CursorDistance <= 2)
                context.ChangeState(new RunAwayState { Context = context });
        }
    }
}
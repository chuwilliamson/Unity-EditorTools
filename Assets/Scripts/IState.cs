﻿using StatePattern.Contexts;

namespace StatePattern.States
{
    public interface IState
    {
        IContext Context { get; set; }
        void OnEnter(IContext context);
        void Update(IContext context);
        void OnExit(IContext context);
    }
}
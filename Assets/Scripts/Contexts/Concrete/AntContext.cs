
using System.Collections.Generic;
using Data;
using States;
using UnityEngine;

namespace Contexts.Concrete
{
    [System.Serializable]
    public class AntContext : Context
    {
        public Stack<IState> Stack;
        public AntData Data { get; set; }
        public void Update(object sender)
        {
            CurrentState.Update(this);
        }

        public AntContext(IState initial, AntData data) : this(initial)
        {
            Data = data;
            CurrentState.OnEnter(this);
        }

        public AntContext(IState initial)
        {
            Stack = new Stack<IState>();
            CurrentState = initial;
            CurrentState.Context = this;
            Stack.Push(CurrentState);
        }

        public override void PushState(IState state)
        {
            if (state == CurrentState)
                return;

            Stack.Push(state);
            CurrentState = Stack.Peek();
            CurrentState.OnEnter(this);
        }

        public override void PopState()
        {
            if (Stack.Count <= 1)
                return;
            CurrentState.OnExit(this);
            Stack.Pop();
            CurrentState = Stack.Peek();
            CurrentState.OnEnter(this);
        }
    }
}
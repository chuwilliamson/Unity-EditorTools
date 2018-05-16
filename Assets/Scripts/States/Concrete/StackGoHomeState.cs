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
        protected BankData BankData => Resources.Load<BankData>("BankData");
        private float timer = 0f;

        public override void OnEnter(IContext context)
        {
            Data = ((AntContext) context).Data;
            base.OnEnter(context);
        }

        public override void Update(IContext context)
        {
            Data.Velocity = (Data.HomePosition - Data.AntPosition).normalized;
            if (Data.HomeDistance <= 1)
            {
                timer += Time.deltaTime;
                foreach (var item in Data.Inventory)
                {
                    BankData.Bank.Add(item);        
                }
                Data.Inventory.Clear();

                if (timer >= 3f)
                {
                    Debug.Log("Bank: " + BankData.Bank.Count + " / Inventory: " + Data.Inventory.Count);
                    context.PopState();
                    context.PushState(new StackFindLeafState { Context = context });
                }
            }
            if (Data.CursorDistance <= 2)
                context.PushState(new StackRunAwayState { Context = context });
        }
    }
}
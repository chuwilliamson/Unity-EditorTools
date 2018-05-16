using Contexts;
using Contexts.Concrete;
using Data;
using UnityEngine;

namespace States.Concrete
{
    public class StackDropOffState : State
    {
        protected AntData Data;
        protected BankData BankData => Resources.Load<BankData>("BankData");

        public override void OnEnter(IContext context)
        {
            Data = ((AntContext)context).Data;
            base.OnEnter(context);
        }

        private float timer = 0f;

        public override void Update(IContext context)
        {
            Data.Velocity = Vector3.zero;
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
    }
}
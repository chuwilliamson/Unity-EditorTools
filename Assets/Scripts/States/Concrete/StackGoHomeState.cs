using System.Collections.Generic;
using Contexts;
using Data;
using UnityEngine;

namespace States.Concrete
{
    public class StackGoHomeState : State
    {
        protected AntData Data => UnityEngine.Resources.Load<AntData>("AntData");
        private List<string> Bank = new List<string>();
        private float timer = 0f;
        public override void Update(IContext context)
        {
            Data.Velocity = (Data.HomePosition - Data.AntPosition).normalized;
            if (Data.HomeDistance <= 1)
            {
                timer += Time.deltaTime;
                foreach (var item in Data.Inventory)
                {
                    Bank.Add(item);        
                }
                Data.Inventory.Clear();

                if (timer >= 3f)
                {
                    Debug.Log("Bank Amount: " + Bank.Count + " Inventory Amount:" + Data.Inventory.Count);
                    context.PopState();
                    context.PushState(new StackFindLeafState { Context = context });
                }
            }
            if (Data.CursorDistance <= 2)
                context.PushState(new StackRunAwayState { Context = context });
        }
    }
}
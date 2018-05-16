using Data;
using Contexts;
using Contexts.Concrete;
using UnityEngine;

namespace States.Concrete
{
    public class StackCollectLeafState : State
    {
        protected AntData Data;
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
            if (Data.Inventory.Count <= 4)
            {
                Data.Inventory.Add("Leaf");
            }

            if (timer >= 3f)
            {
                Debug.Log("Inventory: " + Data.Inventory.Count);
                context.PopState();
                context.PushState(new StackGoHomeState { Context = context });
            }
        }
    }
}
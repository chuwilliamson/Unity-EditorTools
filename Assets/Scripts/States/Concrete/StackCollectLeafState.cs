using Data;
using Contexts;
using UnityEngine;

namespace States.Concrete
{
    public class StackCollectLeafState : State
    {
        protected AntData Data => Resources.Load<AntData>("AntData");
        private float timer = 0f;
        public override void Update(IContext context)
        {
            Data.Velocity = (Data.LeafPosition - Data.AntPosition).normalized;
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
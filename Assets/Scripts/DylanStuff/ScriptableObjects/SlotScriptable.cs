using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScriptableObjects
{
    public class SlotScriptable
    {
        public bool IsEmpty { get; private set; }
        public ItemScriptable Item { get; private set; }

        public void AddItem(ItemScriptable item)
        {
            if (IsEmpty)
                Item = item;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DylanTools
{
    [CreateAssetMenu]
    public class StatScriptable : ScriptableObject
    {
        public string _Name;
        public string _Value;
        public string _Description;

        public void ApplyStatValue(StatScriptable stat)
        {
            if (_Name == stat._Name)
                _Value += stat._Value;
        }

        public override string ToString()
        {
            var data = _Name + "Value: " + _Value + "\n" + _Description;
            return data;
        }
    }
}

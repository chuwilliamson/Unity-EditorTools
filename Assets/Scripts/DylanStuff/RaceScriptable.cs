using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DylanTools
{
    [CreateAssetMenu]
    public class RaceScriptable : ScriptableObject
    {
        public string _Name;
        public List<StatScriptable> _Stats;
    }
}
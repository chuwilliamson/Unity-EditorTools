using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DylanTools
{
    [CreateAssetMenu]
    public class JobScriptable : ScriptableObject
    {
        public string _Name;
        public List<StatScriptable> _Stats;
        public List<RaceScriptable> _RacePrereqs;

        public bool CheckPrereqs(RaceScriptable characterRace)
        {
            foreach (var race in _RacePrereqs)
            {
                if (race.GetType() == characterRace.GetType())
                    return true;
            }

            return false;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DylanTools
{
    [CreateAssetMenu]
    public class CharacterScriptable : ScriptableObject
    {        
        [NonSerialized]
        public Dictionary<string, StatScriptable> _Stats;
        public RaceScriptable _Race;        
        public JobScriptable _Job;

        public void ChangeRace(RaceScriptable race)
        {
            _Race = race;
            foreach (var stat in _Race._Stats)
            {
                ApplyStat(stat);
            }
        }

        public void ChangeJob(JobScriptable job)
        {
            _Job = (job.CheckPrereqs(_Race)) ? job : _Job;
        }

        public void ApplyStat(StatScriptable stat)
        {
            _Stats[stat._Name]?.ApplyStatValue(stat);
        }
    }
}
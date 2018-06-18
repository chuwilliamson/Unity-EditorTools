using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assessments.CrossPlatformDevelopment
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        public float Value;
        private string _name;
        public int Id;
        
        protected virtual void OnEnable()
        {
            _name = name;
            Id = Animator.StringToHash(_name);
        }
    }
}
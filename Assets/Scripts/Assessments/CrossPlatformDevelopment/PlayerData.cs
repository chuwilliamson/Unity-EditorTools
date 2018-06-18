using UnityEngine;
namespace Assessments.CrossPlatformDevelopment
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        [SerializeField]
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
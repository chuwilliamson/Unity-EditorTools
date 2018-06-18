using UnityEngine;

namespace Assessments.CrossPlatformDevelopment
{
    public class CharacterAnimationBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator _anim;

        [SerializeField] private PlayerData HealthData;

        [SerializeField] private PlayerData HorizontalSpeed;

        [SerializeField] private PlayerData VerticalSpeed;

        private void Start()
        {
            _anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            _anim.SetFloat(HealthData.Id, HealthData.Value);
            _anim.SetFloat(HorizontalSpeed.Id, HorizontalSpeed.Value);
            _anim.SetFloat(VerticalSpeed.Id, VerticalSpeed.Value);
        }
    }
}
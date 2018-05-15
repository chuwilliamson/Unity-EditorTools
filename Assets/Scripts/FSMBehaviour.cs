using Contexts.Concrete;
using Data;
using States.Concrete;
using UnityEngine;

public class FSMBehaviour : MonoBehaviour
{
    [SerializeField]
    private AntData antData;
    public Transform LeafTransform;
    public Transform HomeTransform;
    readonly AntContext AntContext = new AntContext
    {
        CurrentState = new GoHomeState()
    };

    public string CurrentStateName;

    private void Update()
    {
        antData.AntPosition = transform.position;
        antData.HomePosition = HomeTransform.position;
        antData.LeafPosition = LeafTransform.position;
        AntContext.Update(this);
    }
}
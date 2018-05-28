#if LOBODESTROYO
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine;

public class MuttStaticReferenceCondition : BehaviourMachine.ConditionNode
{
    public StaticReference _ref;
    public GameObjectVar objectSet;
    public override Status Update()
    {
        if (_ref && _ref.gameObject)
        {
            objectSet.Value = _ref.gameObject;
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }
    }
}
#endif
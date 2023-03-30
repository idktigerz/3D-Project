using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/FlashedCondition")]
public class FlashedCondition : Condition
{
    [SerializeField] private bool negation;
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    public override bool Test(FiniteStateMachine fsm)
    {
        Vector3 direction = fsm.GetNavMeshAgent().target.position - fsm.GetNavMeshAgent().transform.position;
        if (direction.magnitude < viewDistance && fsm.GetNavMeshAgent().target.GetComponent<PlayerController>().isFlashing)
        {
            float angle = Vector3.Angle(direction.normalized, fsm.GetNavMeshAgent().transform.forward);
            if (angle < viewAngle && fsm.GetNavMeshAgent().target.GetComponent<PlayerController>().isFlashing)
            {
                return !negation;
            }
        }

        return negation;
    }
}
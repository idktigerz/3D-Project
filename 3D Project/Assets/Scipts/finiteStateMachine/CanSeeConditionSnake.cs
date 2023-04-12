using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/CanSeeConditionSnake")]
public class CanSeeConditionSnake : Condition
{
    [SerializeField] private bool negation;
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    public override bool Test(FiniteStateMachine fsm)
    {
        Vector3 direction = fsm.GetNavMeshAgent().target.position - fsm.GetNavMeshAgent().transform.position;
        if (direction.magnitude < viewDistance)
        {
            float angle = Vector3.Angle(direction.normalized, fsm.GetNavMeshAgent().transform.forward);
            if (angle < viewAngle)
            {
                Vector3 direction2 = fsm.GetNavMeshAgent().transform.position - fsm.GetNavMeshAgent().target.position;
                if (direction2.magnitude < viewDistance)
                {
                    float angle2 = Vector3.Angle(direction.normalized, fsm.GetNavMeshAgent().target.forward);
                    if (angle2 < viewAngle)
                    {
                        return !negation;
                    }
                }
            }
        }
        return negation;
    }
}
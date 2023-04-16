using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/CanSeeAnimalsCondition")]
public class CanSeeAnimalsCondition : Condition
{
    [SerializeField] private bool negation;
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    public override bool Test(FiniteStateMachine fsm)
    {
        foreach (var ani in fsm.GetNavMeshAgent().listOfAnimals)
        {
            Vector3 direction = ani.transform.position - fsm.GetNavMeshAgent().transform.position;
            if (direction.magnitude < viewDistance)
            {
                float angle = Vector3.Angle(direction.normalized, fsm.GetNavMeshAgent().transform.forward);
                if (angle < viewAngle)
                {
                    return !negation;
                }
            }
        }
        return negation;
    }
}
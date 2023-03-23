using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Patrol")]

public class PatrolAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        Debug.Log($"PATROLING");
        fsm.GetNavMeshAgent().GoToNextPatrolWaypoint();
        if(fsm.GetNavMeshAgent().IsAtDestination())
        {
            Debug.Log($"new dest");
           
            fsm.GetNavMeshAgent().GoToNextPatrolWaypoint();
        }
    }
}

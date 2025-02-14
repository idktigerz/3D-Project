using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        fsm.GetNavMeshAgent().timeStaring = 0;
        fsm.GetNavMeshAgent().GoToTarget();
    }
}

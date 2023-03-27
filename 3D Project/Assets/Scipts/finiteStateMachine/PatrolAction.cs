using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Patrol")]

public class PatrolAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        //Debug.Log($"PATROLING");
        if (fsm.GetNavMeshAgent().IsAtDestination())
        {
            if (fsm.name == "Crocodile")
            {
                fsm.GetNavMeshAgent().StartCoroutine("WalkingPause", 5f);
            }
            else if (fsm.name == "Owl")
            {
                fsm.GetNavMeshAgent().StartCoroutine("WalkingPause", 10f);

            }
        }
    }
}

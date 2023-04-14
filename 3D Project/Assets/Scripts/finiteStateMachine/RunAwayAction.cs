using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/RunAway")]
public class RunAwayAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        if (fsm.name == "Crocodile")
        {
            fsm.GetNavMeshAgent().CrocodileRunAway();
        }
        else if (fsm.name == "Owl")
        {
            fsm.GetNavMeshAgent().OwlRunAway();
        }

    }
}

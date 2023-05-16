using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/RunAway")]
public class RunAwayAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        if (fsm.name.Contains("Crocodile"))
        {
            fsm.GetNavMeshAgent().CrocodileRunAway();
        }
        else if (fsm.name.Contains("Owl"))
        {
            fsm.GetNavMeshAgent().OwlRunAway();
        }
        else if (fsm.name.Contains("Snake"))
        {
            fsm.GetNavMeshAgent().SnakeRunAway();
        }

    }
}

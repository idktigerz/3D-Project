using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Rest")]

public class RestAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {

        if (fsm.name.Contains("Owl") && fsm.GetNavMeshAgent())
        {
            fsm.GetNavMeshAgent().OwlRest();
        }

    }
}
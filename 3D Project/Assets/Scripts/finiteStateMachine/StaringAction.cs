using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Staring")]
public class StaringAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        if (fsm.gameObject.name.Contains("Snake")) fsm.GetNavMeshAgent().SnakeStaring();

    }
}

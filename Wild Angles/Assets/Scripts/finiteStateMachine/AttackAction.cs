using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Attack")]
public class AttackAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        if (fsm.gameObject.name.Contains("Crocodile")) fsm.GetNavMeshAgent().CrocodileAttack();
        else if (fsm.gameObject.name.Contains("Snake")) fsm.GetNavMeshAgent().SnakeAttack();
        else if (fsm.gameObject.name.Contains("Tiger")) fsm.GetNavMeshAgent().TigerAttack();

    }
}

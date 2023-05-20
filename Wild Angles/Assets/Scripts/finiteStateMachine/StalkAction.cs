using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Stalk")]

public class StalkAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {

        fsm.GetNavMeshAgent().TigerStalk();
    }
}
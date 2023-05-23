using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Follow")]

public class FollowAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        if (fsm.GetNavMeshAgent().following) fsm.GetNavMeshAgent().GoToPlayerBabyTiger(fsm.GetNavMeshAgent().target.position,"following");
        else if (fsm.GetNavMeshAgent().homming) fsm.GetNavMeshAgent().GoToPlayerBabyTiger(fsm.GetNavMeshAgent().tent.transform.position,"homming");

    }
}
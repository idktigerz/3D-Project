using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Alerting")]
public class AlertingAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        fsm.GetNavMeshAgent().Ponto.SetActive(true);
        fsm.GetNavMeshAgent().agent.isStopped = true;
        fsm.GetNavMeshAgent().source.clip = fsm.GetNavMeshAgent().talkSound;
        fsm.GetNavMeshAgent().source.enabled = true;

    }
}

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
            fsm.GetNavMeshAgent().timeStaring = 0;
            if (fsm.name == "Crocodile")
            {
                fsm.GetNavMeshAgent().StartCoroutine("WalkingPause", 5f);
            }
            else if (fsm.name == "Owl" && fsm.GetNavMeshAgent().canFly)
            {
                fsm.GetNavMeshAgent().StartCoroutine("WalkingPause", 10f);
            }
            else if (fsm.name == "Butterfly" && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().StartCoroutine("WalkingPause", 1f);
            }
            else if (fsm.name == "Bug" && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().GoToNextPatrolWaypointBug();
            }
            else if (fsm.name == "Frog" && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().GoToNextPatrolWaypointFrog();
            }
            else if (fsm.name == "Baby Tiger" && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().GoToNextPatrolWaypointBabyTiger();
            }
            else if (fsm.name == "Snake" && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().GoToNextPatrolWaypointSnake();
            }


        }
    }
}

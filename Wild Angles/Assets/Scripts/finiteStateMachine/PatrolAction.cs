using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Patrol")]

public class PatrolAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        fsm.GetNavMeshAgent().agent.isStopped = false;

        if (fsm.GetNavMeshAgent().source.isPlaying && fsm.name.Contains("Crocodile") && !fsm.name.Contains("Frog"))
        {
            fsm.GetNavMeshAgent().source.Stop();
            fsm.GetNavMeshAgent().source.loop = false;
        }
        else if (!fsm.GetNavMeshAgent().source.isPlaying || fsm.GetNavMeshAgent().source.clip == fsm.GetNavMeshAgent().hissSound && !fsm.name.Contains("Frog"))
        {
            fsm.GetNavMeshAgent().source.clip = fsm.GetNavMeshAgent().walkingSound;
            fsm.GetNavMeshAgent().source.Play();
            fsm.GetNavMeshAgent().source.loop = true;
        }


        //Debug.Log($"PATROLING");
        if (fsm.GetNavMeshAgent().IsAtDestination() || fsm.name.Contains("Baby Tiger") && Vector3.Distance(fsm.gameObject.transform.position, fsm.GetNavMeshAgent().target.position) > 15)
        {
            fsm.GetNavMeshAgent().timeStaring = 0;
            if (fsm.name.Contains("Crocodile"))
            {
                fsm.GetNavMeshAgent().StartCoroutine("WalkingPause", 5f);
            }
            else if (fsm.name.Contains("Owl") && fsm.GetNavMeshAgent().canFly)
            {
                fsm.GetNavMeshAgent().StartCoroutine("WalkingPause", 10f);
            }
            else if (fsm.name.Contains("Butterfly") && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().StartCoroutine("WalkingPause", 1f);
            }
            else if (fsm.name.Contains("Bug") && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().GoToNextPatrolWaypointBug();
            }
            else if (fsm.name.Contains("Frog") && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().GoToNextPatrolWaypointFrog();
            }
            else if (fsm.name.Contains("Baby Tiger") && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().Ponto.SetActive(false);
                fsm.GetNavMeshAgent().GoToNextPatrolWaypointBabyTiger();
            }
            else if (fsm.name.Contains("Snake") && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().GoToNextPatrolWaypointSnake();
            }
            else if (fsm.name.Contains("Tiger") && fsm.GetNavMeshAgent())
            {
                fsm.GetNavMeshAgent().TigerPatrol();
            }



        }

    }
}

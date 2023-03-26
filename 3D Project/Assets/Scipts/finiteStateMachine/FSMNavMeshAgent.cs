using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FSMNavMeshAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] patrolWaypoints;
    public Transform target;

    public GameObject bulletPrefab;
    public float shootTimeInterval = 1;
    private float shootTimer = 0;

    private float maxEnergy = 100;
    public float energy = 100;
    float timer = 0;
    float cooldownTime = 2f;

    public bool canHear;
    [Header("Owl Stuff")]
    public bool canRun;

    public List<Transform> OwlWaypoints;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public bool IsAtDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void GoToNextPatrolWaypoint()
    {

        agent.isStopped = false;
        //agent.SetDestination(patrolWaypoints[Random.Range(0, patrolWaypoints.Length)].position);
        //var NewPos = Random.insideUnitCircle.normalized * 100;
        var NewPos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        if (Vector3.Distance(transform.position, NewPos) > 3)
        {
            agent.SetDestination(transform.position + NewPos);
            Debug.Log("start walking");
        }
        else
        {
            if (gameObject.name == "Crocodile")
            {
                GoToNextPatrolWaypoint();
            }
            else if (gameObject.name == "Owl")
            {
                GoToNextPatrolWaypointOwl();
            }

        }
    }
    public IEnumerator WalkingPause(float time)
    {

        Debug.Log("walking pause");
        agent.isStopped = true;
        yield return new WaitForSeconds(time);
        agent.isStopped = false;
        GoToNextPatrolWaypoint();

    }


    public void Stop()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void GoToTarget()
    {
        agent.SetDestination(target.position);
    }

    public void CrocodileAttack()
    {

        agent.isStopped = true;
        shootTimer += Time.deltaTime;
        if (shootTimer > shootTimeInterval)
        {
            energy -= 5;
            shootTimer = 0;
            Vector3 diretion = (target.position - transform.position).normalized;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position + transform.forward, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = diretion * 20;
        }
    }
    public void ButterflyAttack()
    {
        Debug.Log($"AQUII");
    }
    public void GoToNextPatrolWaypointOwl()
    {

        agent.isStopped = false;
        agent.SetDestination(OwlWaypoints[Random.Range(0, OwlWaypoints.Count)].position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "NoiseBubble")
        {
            canHear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "NoiseBubble")
        {
            canHear = false;
        }
    }
}

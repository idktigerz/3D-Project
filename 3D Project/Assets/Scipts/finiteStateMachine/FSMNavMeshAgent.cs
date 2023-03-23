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

    public bool canRun;

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
        agent.SetDestination(patrolWaypoints[Random.Range(0, patrolWaypoints.Length)].position);
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

    public void Attack()
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

    public void Recover()
    {
        agent.isStopped = false;
        Debug.Log($"RECOVERING");
        if (energy < maxEnergy)
        {
            energy += 5 * Time.deltaTime;
        }
    }

    public void RunAway()
    {
        agent.isStopped = false;
        Transform point = null;
        Debug.Log($"RUNNING AWAY");
        {
            float minDist = 0;
            Vector3 currentPos = target.position;
            foreach (Transform w in patrolWaypoints)
            {
                float dist = Vector3.Distance(w.position, currentPos);
                if (minDist < dist)
                {
                    point = w;
                    minDist = dist;
                    agent.SetDestination(point.position);
                }
            }
        }
        energy += 5 * Time.deltaTime;
        timer = 0;
    }
}

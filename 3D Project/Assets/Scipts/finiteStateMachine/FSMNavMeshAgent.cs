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

    private float initialSpeed;

    public Vector3 currentDest;
    private bool canFly;
    public GameObject OwlBody;

    private void Awake()
    {
        OwlWaypoints.Clear();
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        initialSpeed = agent.speed;
        if (gameObject.name == "Owl") currentDest = OwlBody.transform.position;
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
        canFly = false;
        agent.isStopped = true;
        agent.speed = 0;
        yield return new WaitForSecondsRealtime(time);
        agent.isStopped = false;
        canFly = true;
        agent.speed = initialSpeed;
        if (gameObject.name == "Crocodile")
        {
            GoToNextPatrolWaypoint();
        }
        else if (gameObject.name == "Owl")
        {
            Debug.Log($"ESTOU AQUI");
            GoToNextPatrolWaypointOwl();
        }

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
        if (OwlWaypoints.Count != 0)
        {
            agent.isStopped = false;
            Vector3 pos = OwlWaypoints[Random.Range(0, OwlWaypoints.Count)].position;
            currentDest = pos;
            agent.SetDestination(pos);
            canFly = false;
        }
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

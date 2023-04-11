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
    [Header("Animal Stuff")]
    public bool flashed;
    private Rigidbody rb;
    [SerializeField] bool canAttack;
    private bool attacking;
    public bool canRun;

    [Header("Owl Stuff")]


    public List<Transform> OwlWaypoints;

    private float initialSpeed;

    public Vector3 currentDest;
    public bool canFly;
    public GameObject OwlBody;

    public GameObject[] listOfAnimals;


    private void Awake()
    {
        OwlWaypoints.Clear();
    }
    void Start()
    {
        listOfAnimals = GameObject.FindGameObjectsWithTag("Animal");
        canFly = true;
        agent = GetComponent<NavMeshAgent>();
        initialSpeed = agent.speed;
        if (gameObject.name == "Owl") currentDest = OwlBody.transform.position;
        rb = GetComponent<Rigidbody>();
        canAttack = true;
        GameObject[] trees = GameObject.FindGameObjectsWithTag("OwlTrees");
        foreach (GameObject tree in trees)
        {
            OwlWaypoints.Add(FindChildGameObjectByName(tree, "OwlWaypoint").transform);
        }
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
                //GoToNextPatrolWaypointOwl();
            }


        }
    }
    public void GoToNextPatrolWaypointButter()
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
            GoToNextPatrolWaypointButter();
        }
    }
    public void GoToNextPatrolWaypointBug()
    {

        agent.isStopped = false;
        var NewPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        agent.SetDestination(transform.position + NewPos);
    }
    public void GoToNextPatrolWaypointFrog()
    {
        agent.isStopped = false;
        var NewPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        agent.SetDestination(transform.position + NewPos);
    }
    public void GoToNextPatrolWaypointBabyTiger()
    {
        agent.isStopped = false;
        var NewPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        agent.SetDestination(target.position + NewPos);
    }
    public void GoToNextPatrolWaypointOwl()
    {
        if (OwlWaypoints.Count != 0)
        {
            agent.isStopped = false;
            Vector3 pos = OwlWaypoints[Random.Range(0, OwlWaypoints.Count)].position;
            currentDest = pos;
            agent.SetDestination(pos);
            energy -= 10;
        }
    }
    public void GoToNextPatrolWaypointSnake()
    {
        agent.isStopped = false;
        var NewPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        agent.SetDestination(transform.position + NewPos);
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
            GoToNextPatrolWaypointOwl();
        }
        else if (gameObject.name == "Butterfly")
        {
            GoToNextPatrolWaypointButter();
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
        if (canAttack)
        {
            rb.constraints = RigidbodyConstraints.None;
            Vector3 direction = transform.position - target.transform.position;
            float force = 2;
            GetComponent<Rigidbody>().AddForce(-direction * force, ForceMode.Impulse);
            canAttack = false;
            attacking = true;
            StartCoroutine("CrocAttackWait");


        }
    }
    public void SnakeAttack()
    {
        if (canAttack)
        {
            rb.constraints = RigidbodyConstraints.None;
            Vector3 direction = transform.position - target.transform.position;
            float force = 2;
            GetComponent<Rigidbody>().AddForce(-direction * force, ForceMode.Impulse);
            canAttack = false;
            attacking = true;
            StartCoroutine("CrocAttackWait"); 
        }
    }
    public void SnakeStaring()
    {
        
    }
    IEnumerator CrocAttackWait()
    {
        yield return new WaitForSeconds(0.3f);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        attacking = false;
        yield return new WaitForSeconds(2);
        canAttack = true;
    }


    public void OwlRunAway()
    {

        Transform longestWaypoint = null;
        float longestDistance = 0;
        if (gameObject.name == "Owl")
        {
            agent.isStopped = false;
            agent.speed = initialSpeed;
            foreach (var w in OwlWaypoints)
            {
                if (Vector3.Distance(target.position, w.position) > longestDistance)
                {
                    longestWaypoint = w;
                    longestDistance = Vector3.Distance(target.position, w.position);
                }
            }
            currentDest = longestWaypoint.position;
            agent.SetDestination(longestWaypoint.position);
        }
    }
    public void CrocodileRunAway()
    {
        if (gameObject.name == "Crocodile" && canRun)
        {
            var NewPos = ((transform.position - target.position));
            agent.SetDestination(transform.position + NewPos);
            canRun = false;
        }
    }

    public void OwlRest()
    {
        if (IsAtDestination())
        {
            agent.speed = 0;
        }
        agent.isStopped = false;
        energy += 2 * Time.deltaTime;
    }
    IEnumerator StopHear()
    {
        yield return new WaitForSeconds(5);
        canHear = false;
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
            StartCoroutine("StopHear");
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && attacking)
        {
            Debug.Log($"o diogo é gay");
        }
    }
    private GameObject FindChildGameObjectByName(GameObject topParentObject, string gameObjectName)
    {
        for (int i = 0; i < topParentObject.transform.childCount; i++)
        {
            if (topParentObject.transform.GetChild(i).name == gameObjectName)
            {
                return topParentObject.transform.GetChild(i).gameObject;
            }

            GameObject tmp = FindChildGameObjectByName(topParentObject.transform.GetChild(i).gameObject, gameObjectName);

            if (tmp != null)
            {
                return tmp;
            }
        }
        return null;
    }
}

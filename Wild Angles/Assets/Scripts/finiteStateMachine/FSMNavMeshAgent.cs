using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FSMNavMeshAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] patrolWaypoints;
    public Transform target;

    public GameObject bulletPrefab;
    public float shootTimeInterval = 1;
    public float energy = 100;
    public bool canHear;
    [Header("Animal Stuff")]
    public bool flashed;
    private Rigidbody rb;
    public bool canAttack;
    public bool canRun;
    public float timeStaring;
    public bool snakeCanAttack;
    public GameObject Ponto;

    [Header("Owl Stuff")]


    public List<Transform> OwlWaypoints;

    private float initialSpeed;

    public Vector3 currentDest;
    public bool canFly;
    public GameObject OwlBody;

    public GameObject[] listOfAnimals;

    private bool canDamagePlayer = false;

    public PlayerController playerController;
    [Header("baby tiger")]
    public bool following;
    public bool homming;
    public GameObject tent;

    [Header("animations")]
    public Animator animator;
    [Header("Sounds")]
    public AudioSource source;
    public AudioClip walkingSound;
    [SerializeField] private AudioClip attackSound;

    public AudioClip hissSound;
    [SerializeField] private AudioClip talkSound;






    void Start()
    {
        source = GetComponent<AudioSource>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        canFly = true;
        agent = gameObject.GetComponent<NavMeshAgent>();
        initialSpeed = agent.speed;
        if (gameObject.name.Contains("Owl")) currentDest = OwlBody.transform.position;
        rb = GetComponent<Rigidbody>();
        canAttack = true;
        if (gameObject.name.Contains("Owl")) StartCoroutine("OwlWaypointsAdder");
        else if (gameObject.name.Contains("Baby Tiger")) StartCoroutine("AnimalListAdder");



    }
    private IEnumerator OwlWaypointsAdder()
    {
        yield return new WaitForSeconds(1f);
        GameObject[] trees = GameObject.FindGameObjectsWithTag("OwlTrees");
        foreach (GameObject tree in trees)
        {
            Debug.Log(tree.name);
            OwlWaypoints.Add(FindChildGameObjectByName(tree, "OwlWaypoint").transform);
        }
    }
    private IEnumerator AnimalListAdder()
    {
        yield return new WaitForSeconds(0.5f);
        listOfAnimals = GameObject.FindGameObjectsWithTag("Animal");
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

        source.clip = null;
        source.loop = false;
        Debug.Log($"stop sorce");
        agent.isStopped = false;
        var NewPos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        agent.SetDestination(transform.position + NewPos);
        agent.speed = initialSpeed;
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
        agent.speed = 7;
        var NewPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        agent.SetDestination(target.position + NewPos);
    }

    public void GoToPlayerBabyTiger(Vector3 targetPos, string var)
    {
        agent.isStopped = false;
        if (homming) agent.speed = 5; else if (following) agent.speed = 10;
        Vector3 NewPos = targetPos;
        agent.SetDestination(NewPos);
        if (Vector3.Distance(NewPos, transform.position) < 3f)
        {
            if (var == "following") following = false; else if (var == "homming") homming = false;

        }
    }
    public void GoToTentBabyTiger()
    {
        Debug.Log($"sdfsafsadfsdf");
        agent.isStopped = false;
        agent.speed = 5;
        var NewPos = tent.transform.position;
        agent.SetDestination(NewPos);
        if (Vector3.Distance(tent.transform.position, transform.position) < 2f)
        {
            homming = false;
        }
    }
    public void GoToNextPatrolWaypointOwl()
    {
        if (OwlWaypoints.Count != 0)
        {
            animator.SetBool("Sleeping", false);
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
        agent.speed = initialSpeed;
        var NewPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        agent.SetDestination(transform.position + NewPos);
    }

    public IEnumerator WalkingPause(float time)
    {
        source.clip = null;
        source.loop = false;
        canFly = false;
        //agent.isStopped = true;
        if (agent.GetComponent<FiniteStateMachine>().currentState.name.Contains("PatrolState"))
        {
            agent.speed = 0;
        }

        if (gameObject.name.Contains("Owl"))
        {
            animator.SetBool("Sleeping", true);
            source.clip = talkSound;
            source.Play();
            source.loop = false;
        }
        yield return new WaitForSecondsRealtime(time);
        //agent.isStopped = false;
        canFly = true;
        agent.speed = initialSpeed;
        if (gameObject.name.Contains("Crocodile"))
        {

            GoToNextPatrolWaypoint();
        }
        else if (gameObject.name.Contains("Owl"))
        {
            GoToNextPatrolWaypointOwl();
            animator.SetBool("Sleeping", false);
        }
        else if (gameObject.name.Contains("Butterfly"))
        {
            GoToNextPatrolWaypointButter();
        }
        else if (gameObject.name.Contains("Tiger"))
        {
            TigerPatrol();
        }
    }
    public void GoToTarget()
    {
        agent.speed = initialSpeed * 1.5f;
        agent.isStopped = false;
        agent.SetDestination(target.position);
        if (source != null && hissSound != null && source.clip != hissSound)
        {
            source.clip = hissSound;
            source.Play();
            source.loop = true;
        }
    }

    public void CrocodileAttack()
    {
        agent.SetDestination(target.position);
        if (canAttack)
        {
            if (source != null)
            {
                source.clip = attackSound;
                source.Play();
            }
            animator.Play("CrocodileAttack");
            rb.constraints = RigidbodyConstraints.None;
            Vector3 direction = transform.position - target.transform.position;
            float force = 2;
            GetComponent<Rigidbody>().AddForce(-direction * force, ForceMode.Impulse);
            canAttack = false;

            StartCoroutine("CrocAttackWait");


            if (canDamagePlayer)
            {
                playerController.DamagePlayer(25f);
                canDamagePlayer = false;
            }

        }
    }
    IEnumerator CrocAttackWait()
    {
        yield return new WaitForSeconds(0.3f);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(2);
        canAttack = true;
    }
    public void SnakeAttack()
    {
        agent.SetDestination(target.position);
        if (canAttack)
        {
            if (source != null)
            {
                source.clip = attackSound;
                source.Play();
            }
            rb.constraints = RigidbodyConstraints.None;
            Vector3 direction = transform.position - target.transform.position;
            float force = 4;
            GetComponent<Rigidbody>().AddForce(-direction * force, ForceMode.Impulse);
            canAttack = false;
            StartCoroutine("CrocAttackWait");

            if (canDamagePlayer)
            {
                playerController.DamagePlayer(15f);
                canDamagePlayer = false;
            }
            StartCoroutine("CrocAttackWait");
        }
    }
    public void SnakeStaring()
    {
        source.clip = talkSound;
        source.Play();
        source.loop = false;
        agent.SetDestination(transform.position);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        timeStaring += 1 * Time.deltaTime;
        if (timeStaring > 5)
        {
            snakeCanAttack = true;
            timeStaring = 0;
        }
    }
    public void SnakeRunAway()
    {
        if (gameObject.name.Contains("Snake") && canRun)
        {
            var NewPos = ((transform.position - target.position));
            agent.SetDestination(transform.position + NewPos);
            canRun = false;
        }
    }



    public void OwlRunAway()
    {
        source.Stop();
        Transform longestWaypoint = null;
        float longestDistance = 0;
        if (gameObject.name.Contains("Owl"))
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
            animator.SetBool("Sleeping", false);
        }
    }
    public void CrocodileRunAway()
    {
        if (gameObject.name.Contains("Crocodile") && canRun)
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
            animator.SetBool("Sleeping", true);
        }
        agent.isStopped = false;
        energy += 2 * Time.deltaTime;

    }
    public void TigerPatrol()
    {
        agent.speed = initialSpeed;
        agent.isStopped = false;
        var NewPos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        agent.SetDestination(transform.position + NewPos);
    }
    public void TigerStalk()
    {
        agent.isStopped = false;
        if (Vector3.Distance(target.transform.position, transform.position) < 10)
        {
            agent.speed = initialSpeed / 1.5f;
        }
        else
        {
            agent.speed = initialSpeed;
        }
        GoToTarget();
    }
    public void TigerRunAway()
    {
        if (gameObject.name.Contains("Tiger") && canRun)
        {
            var NewPos = ((transform.position - target.position));
            agent.SetDestination(transform.position + NewPos);
            canRun = false;
        }
    }
    public void TigerAttack()
    {
        if (canAttack)
        {
            if (source != null)
            {
                source.clip = attackSound;
                source.Play();
            }
            rb.constraints = RigidbodyConstraints.None;
            Vector3 direction = transform.position - target.transform.position;
            float force = 4;
            GetComponent<Rigidbody>().AddForce(-direction * force, ForceMode.Impulse);
            canAttack = false;
            StartCoroutine("CrocAttackWait");

            if (canDamagePlayer)
            {
                playerController.DamagePlayer(15f);
                canDamagePlayer = false;
            }
            StartCoroutine("CrocAttackWait");
        }
    }
    IEnumerator StopHear()
    {
        yield return new WaitForSeconds(5);
        canHear = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("NoiseBubble"))
        {
            canHear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("NoiseBubble"))
        {
            StartCoroutine("StopHear");
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && canDamagePlayer == false)
        {
            canDamagePlayer = true;
        }
    }
    /*private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && attacking && canDamagePlayer == true)
        {
            canDamagePlayer = false;
        }
    }*/
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

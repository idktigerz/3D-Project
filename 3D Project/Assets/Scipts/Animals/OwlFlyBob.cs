using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OwlFlyBob : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] private float flyBobSpeed;
    [SerializeField] private float flyBobAmmount;
    private float defaultYPos;
    private float timer;

    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        defaultYPos = 5.46f;
    }

    // Update is called once per frame
    void Update()
    {
        FlyBobHandler();
    }
    private void FlyBobHandler()
    {
        Debug.Log(agent.speed);
        if (Mathf.Abs(gameObject.transform.forward.x) > 0.1f && agent.speed > 0.1f || Mathf.Abs(gameObject.transform.forward.z) > 0.1f && agent.speed > 0.1f)
        {
            timer += Time.deltaTime * flyBobSpeed;
            gameObject.transform.localPosition = new Vector3(
                transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * flyBobAmmount,
                transform.localPosition.z);
        }
        else if (transform.position != agent.GetComponent<FSMNavMeshAgent>().currentDest)
        {
            Debug.Log($"ABBAABABABBABBA");
            transform.position = Vector3.MoveTowards(transform.position, agent.GetComponent<FSMNavMeshAgent>().currentDest, 0.1f);
        }

    }
}

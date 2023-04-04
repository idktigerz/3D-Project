using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ButterflyBob : MonoBehaviour
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
        if (agent.speed > 0.1f)
        {
            timer += Time.deltaTime * flyBobSpeed;
            gameObject.transform.localPosition = new Vector3(
                transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * flyBobAmmount,
                transform.localPosition.z);
        }

    }
}

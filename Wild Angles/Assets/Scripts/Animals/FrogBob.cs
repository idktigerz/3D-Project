using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FrogBob : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] private float flyBobSpeed;
    [SerializeField] private float flyBobAmmount;
    private float defaultYPos;
    private float timer;

    private Vector3 moveDirection;
    private bool canBob;

    // Start is called before the first frame update
    void Start()
    {
        canBob = false;
        defaultYPos = 1.104f;
        StartCoroutine("FlyBobHandlertime");
    }

    // Update is called once per frame
    void Update()
    {
        if (canBob) FlyBobHandler();
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
    private IEnumerator FlyBobHandlertime()
    {
        yield return new WaitForSeconds(Random.Range(1, 7));
        canBob = true;
    }
}

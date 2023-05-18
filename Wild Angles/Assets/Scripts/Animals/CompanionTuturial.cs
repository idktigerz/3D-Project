using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CompanionTuturial : MonoBehaviour
{
    [SerializeField] bool canStartMove;
    public bool canInteract;
    public GameObject CompanionUI;



    // Start is called before the first frame update
    void Start()
    {
        canStartMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract && Input.GetKey(KeyCode.F))
        {
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<FSMNavMeshAgent>().enabled = true;
            GetComponent<FiniteStateMachine>().enabled = true;
            GameObject.Destroy(CompanionUI);
        }

    }
}

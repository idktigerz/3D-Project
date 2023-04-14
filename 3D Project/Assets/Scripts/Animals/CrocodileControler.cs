using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileControler : MonoBehaviour
{
    [SerializeField] float attackRadius;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "NoiseBubble")
        {
            Debug.Log($"LISTENING");
            Follow(other.gameObject.transform.position);
        }
    }

    private void Follow(Vector3 position)
    {
        Debug.Log($"FOLOWING");
        transform.position = Vector3.MoveTowards(transform.position, position, 3 * Time.deltaTime);
    }
}

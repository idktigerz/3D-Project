using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photographable : MonoBehaviour
{
    public PlayerCam playerCam;

    public enum PhotographableID { Snake, Butterfly, Parrot, sloth, frog, owl, crocodile, tiger, otter, bug, tree };

    [Header("animal stuff")]

    public PhotographableID photographableId;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"a tua mae é uma moto quatro");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public PhotographableID GetID()
    {
        return photographableId;

    }
    private void OnBecameVisible()
    {
        Debug.Log("IM VISIBLE");
        playerCam.currentAnimalsInTheframe.Add(gameObject);
    }

    private void OnBecameInvisible()
    {
        Debug.Log("IM INVISIBLE");
        playerCam.currentAnimalsInTheframe.Remove(gameObject);
    }
}

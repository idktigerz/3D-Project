using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photographable : MonoBehaviour
{
    public PlayerCam playerCam;
    public bool haveBeenSeen;

    public enum PhotographableID { Snake, Butterfly, Parrot, Sloth, Frog, Owl, Crocodile, Tiger, Otter, Bug, WhiteOrchid, PurpleOrchid, CocoaTree, BananaTree, Helconia };

    [Header("animal stuff")]

    public PhotographableID photographableId;
    // Start is called before the first frame update
    void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>();
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
        playerCam.currentAnimalsInTheframe.Add(gameObject);
    }

    private void OnBecameInvisible()
    {
        playerCam.currentAnimalsInTheframe.Remove(gameObject);
    }
}

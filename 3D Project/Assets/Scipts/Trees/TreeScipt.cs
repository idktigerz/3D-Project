using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScipt : MonoBehaviour
{
    public FSMNavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        GameObject waypoint = FindChildGameObjectByName(gameObject, "OwlWaypoint");
        //agent.OwlWaypoints.Add(waypoint.transform);
    }

    // Update is called once per frame
    void Update()
    {

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

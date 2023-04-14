using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject ResourcePrefab;
    public float spawnChance;

    [Header("Raycast Settings")]

    public float distanceBetweenChecks;

    public float heightOfCheck = 10f, rangeOfCheck = 30;
    public LayerMask layerMask;
    public Vector2 positivePosition, negativePosition;

    private void Start()
    {
        SpawnResources();
    }

    void SpawnResources()
    {
        for (float x = negativePosition.x; x < positivePosition.x; x += distanceBetweenChecks)
        {
            for (float z = negativePosition.y; z < positivePosition.y; z += distanceBetweenChecks)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, layerMask))
                {
                    if (spawnChance > Random.Range(0, 101))
                    {
                        Instantiate(ResourcePrefab, hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 350), 0)), transform);
                    }
                }
            }
        }
    }

}

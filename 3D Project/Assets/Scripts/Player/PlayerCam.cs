using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;
    public PlayerController playerController;
    [Header("photographable stuff")]

    public int[] picCounter;

    public string[] animalList;
    public string[] plantList;

    public List<GameObject> currentAnimalsInTheframe;
    public List<GameObject> currentPlantsInTheframe;


    private void Start()
    {
        picCounter = new int[11];
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        for (int i = 0; i < picCounter.Length; i++)
        {
            picCounter[i] = 0;
        }
        animalList = new string[11];
        animalList[0] = "Snake";
        animalList[1] = "Butterfly";
        animalList[2] = "Parrot";
        animalList[3] = "Sloth";
        animalList[4] = "Frog";
        animalList[5] = "Owl";
        animalList[6] = "Crocodile";
        animalList[7] = "Tiger";
        animalList[8] = "Otter";
        animalList[9] = "Bug";
        animalList[10] = "Tree";

        plantList = new string[11];
    }

    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && GetComponent<Camera>().fieldOfView > 20 && playerController.cameraON)
        {
            GetComponent<Camera>().fieldOfView--;

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && GetComponent<Camera>().fieldOfView < 60 && playerController.cameraON)
        {
            GetComponent<Camera>().fieldOfView++;
        }
    }

    public GameObject GetClosestPhotographable()
    {
        GameObject closest = null;
        float closestDistance = 100;
        float distance;
        for (int i = 0; i < currentAnimalsInTheframe.Count; i++)
        {
            distance = Vector3.Distance(playerController.transform.position, currentAnimalsInTheframe[i].transform.position);
            if (distance < closestDistance)
            {
                closest = currentAnimalsInTheframe[i];
                closestDistance = distance;
            }
        }
        return closest;
    }
    public GameObject GetClosestPhotographableAngle()
    {
        GameObject closest = null;
        float closestAngle = Mathf.Infinity;
        float angle;
        foreach (GameObject obj in currentAnimalsInTheframe)
        {
            Vector3 targetDir = obj.transform.position - transform.position;
            angle = Vector3.Angle(targetDir, transform.forward);

            if (angle < closestAngle)
            {
                closestAngle = angle;
                closest = obj;
            }

        }
        return closest;
    }
}

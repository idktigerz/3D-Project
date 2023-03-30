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

    public List<GameObject> currentAnimalsInTheframe;


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
        animalList[0] = "snake";
        animalList[1] = "butterfly";
        animalList[2] = "parrot";
        animalList[3] = "sloth";
        animalList[4] = "frog";
        animalList[5] = "owl";
        animalList[6] = "crocodile";
        animalList[7] = "tiger";
        animalList[8] = "otter";
        animalList[9] = "bug";
        animalList[10] = "tree";
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

    public GameObject getClosestPhotographable()
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
}

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

    public string[] photographableList;

    public List<GameObject> currentAnimalsInTheframe;
    public List<GameObject> currentPlantsInTheframe;

    public float closestDistance;
    public bool snakeSeen;
    public bool bugSeen;

    public bool frogSeen;
    public bool tigerSeen;
    public bool butterflySeen;
    public bool owlSeen;
    public bool crocodileSeen;


    private void Start()
    {
        picCounter = new int[11];
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        for (int i = 0; i < picCounter.Length; i++)
        {
            picCounter[i] = 0;
        }
        photographableList = new string[12];
        photographableList[0] = "Snake";
        photographableList[1] = "Butterfly";
        photographableList[2] = "Frog";
        photographableList[3] = "Owl";
        photographableList[4] = "Crocodile";
        photographableList[5] = "Tiger";
        photographableList[6] = "Bug";
        photographableList[7] = "White Orchid";
        photographableList[8] = "Purple Orchid";
        photographableList[9] = "Cocoa Tree";
        photographableList[10] = "Banana Tree";
        photographableList[11] = "Helconia";
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
            closestDistance++;
            if (closestDistance > 100)
            {
                closestDistance = 100;
            }

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && GetComponent<Camera>().fieldOfView < 60 && playerController.cameraON)
        {
            GetComponent<Camera>().fieldOfView++;
            closestDistance--;
            if (closestDistance < 10)
            {
                closestDistance = 10;
            }
        }
        if (!playerController.cameraON)
        {
            closestDistance = 30;
        }
    }

    public GameObject GetClosestPhotographable()
    {

        GameObject closest = null;
        float distance;
        float closestDistanceAnimal = 100;
        for (int i = 0; i < currentAnimalsInTheframe.Count; i++)
        {
            Vector3 direction = currentAnimalsInTheframe[i].transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            distance = Vector3.Distance(playerController.transform.position, currentAnimalsInTheframe[i].transform.position);
            if (distance < closestDistanceAnimal && distance < closestDistance && IsInTheFrame(currentAnimalsInTheframe[i]))
            {
                closest = currentAnimalsInTheframe[i];
                closestDistanceAnimal = distance;
            }
        }
        return closest;
    }
    public GameObject GetClosestPhotographableAngle()
    {
        GameObject closest = null;
        float closestAngle = Mathf.Infinity;
        float angle;
        float distance;
        foreach (GameObject obj in currentAnimalsInTheframe)
        {
            Vector3 targetDir = obj.transform.position - transform.position;
            angle = Vector3.Angle(targetDir, transform.forward);
            distance = Vector3.Distance(playerController.transform.position, obj.transform.position);
            if (angle < closestAngle && distance < closestDistance && IsInTheFrame(obj))
            {
                closestAngle = angle;
                closest = obj;
            }
        }
        return closest;
    }
    private bool IsInTheFrame(GameObject thing)
    {
        Vector3 screenPos = GetComponent<Camera>().WorldToScreenPoint(thing.transform.position);
        if (330 < screenPos.x && screenPos.x < 1579 && screenPos.y < 871 && screenPos.y > 91)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool MoreThenOneInFrame(string name)
    {
        int count = 0;
        foreach (var item in currentAnimalsInTheframe)
        {
            if (item.name == name && IsInTheFrame(item))
            {
                count++;
                Debug.Log(item.name);
            }
        }
        if (count > 1) return true; else return false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    //public bool canBloom = false;
    [SerializeField] private float openTime;
    [SerializeField] private float closeTime;

    private TimeSpan openHour;
    private TimeSpan closeHour;
    [SerializeField]
    private GameObject plant;
    public BoxCollider collider;

    public TimeController timeController;
    public PlayerController playerController;

    [SerializeField]
    private int healAmount;

    private bool plantEaten;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timeController = GameObject.FindGameObjectWithTag("TimeController").GetComponent<TimeController>();
        plantEaten = false;
        //render.enabled = false;
        openHour = TimeSpan.FromHours(openTime);
        closeHour = TimeSpan.FromHours(closeTime);

        if (openHour < timeController.currentTime.TimeOfDay && plantEaten == false)
        {
            plant.SetActive(false);
            collider.enabled = false;
        }
        else
        {
            plant.SetActive(true);
            collider.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (openHour > closeHour)
        {
            if (openHour > timeController.currentTime.TimeOfDay && closeHour < timeController.currentTime.TimeOfDay && plantEaten == false)
            {
                plant.SetActive(false);
                collider.enabled = false;
            }
            else
            {
                plant.SetActive(true);
                collider.enabled = true;

            }
        }
        else if (openHour < closeHour)
        {
            if (openHour < timeController.currentTime.TimeOfDay && closeHour > timeController.currentTime.TimeOfDay && plantEaten == false)
            {
                plant.SetActive(false);
                collider.enabled = false;
            }
            else
            {
                plant.SetActive(true);
                collider.enabled = true;

            }
        }
        if (Input.GetKeyDown(playerController.interactKey) && playerController.canInteractPlant && playerController.health <= 100f && plantEaten == false)
        {
            plantEaten = true;
            playerController.HealPlayer(healAmount);
            StartCoroutine(ChangePlantState());
        }

    }
    private IEnumerator ChangePlantState()
    {
        plant.SetActive(false);

        yield return new WaitForSeconds(5);

        plantEaten = false;

        if (openHour <= timeController.currentTime.TimeOfDay)
        {
            plant.SetActive(true);
            collider.enabled = true;
        }
        else
        {
            plant.SetActive(false);
            collider.enabled = false;
        }

    }

}

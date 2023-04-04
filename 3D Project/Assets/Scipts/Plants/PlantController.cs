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
    private GameObject orchid;

    public TimeController timeController;
    void Start()
    {
        orchid.SetActive(false);
        //render.enabled = false;
        openHour = TimeSpan.FromHours(openTime);
        closeHour = TimeSpan.FromHours(closeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(openHour > timeController.currentTime.TimeOfDay && closeHour < timeController.currentTime.TimeOfDay)
        {
            orchid.SetActive(false);
        }
        else
        {
            orchid.SetActive(true);

        }
    }
}

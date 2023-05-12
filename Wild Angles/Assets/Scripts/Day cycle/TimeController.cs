using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private float timeMultiplier;

    [Header("Time starts")]
    [SerializeField]
    private float startHour;
    [SerializeField]
    private float resetHour;
    [SerializeField]
    private float sunriseHour;
    [SerializeField]
    private float sunsetHour;
    [SerializeField]
    public DateTime currentTime;
    [SerializeField]
    private float restHour;
    [SerializeField]
    private float midDayHour;


    [Header("Lighting")]
    [SerializeField]
    private Light sunLight;
    [SerializeField]
    private Color dayAmbientLight;
    [SerializeField]
    private Color nightAmbientLight;
    [SerializeField]
    private AnimationCurve lightChangeCurve;
    [SerializeField]
    private float maxSunLightIntensity;
    [SerializeField]
    private Light moonLight;
    [SerializeField]
    private float maxMoonLightIntensity;
    [SerializeField]
    private Light playerLight;
    [SerializeField]
    private Light flashLight;

    [Header("Skybox texture")]
    [SerializeField]
    private Material nightSky;
    [SerializeField]
    private Material sunSky;

    [Header("Other stuff")]
    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;
    private TimeSpan resetTime;
    public TimeSpan restTime;
    public TimeSpan midDayTime;
    public int dayCounter;
    public int nightCounter;
    public bool canToggleNightVision;
    private bool canPassDay;

    [Header("Debugging text")]
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI dayText;

    // Start is called before the first frame update
    void Start()
    {
        canPassDay = true;
        dayCounter = 0;
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);

        resetTime = TimeSpan.FromHours(resetHour);

        restTime = TimeSpan.FromHours(restHour);

        midDayTime = TimeSpan.FromHours(midDayHour);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            dayCounter = 6;
        }
        //StartCoroutine("UpdateDayTest()");
        RotateSun();
        UpdateLightSettings();
        dayText.text = "Day: " + dayCounter.ToString();
        //UpdateDay();
    }

    private void FixedUpdate()
    {
        UpdateTimeOfDay();

    }

    public void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
        if (currentTime.TimeOfDay >= resetTime && canPassDay)
        {
            dayCounter++;

            canPassDay = false;
            StartCoroutine("UpdateDayTest");
        }
    }


    private IEnumerator UpdateDayTest()
    {
        yield return new WaitForSeconds(5);
        canPassDay = true;
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);

            //RenderSettings.skybox = sunSky;
            RenderSettings.ambientLight = dayAmbientLight;
            RenderSettings.ambientIntensity = 1f;
            RenderSettings.reflectionIntensity = 1f;
            playerLight.intensity = 0f;
            flashLight.intensity = 1f;
            canToggleNightVision = false;
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);

            //RenderSettings.skybox = nightSky;

            RenderSettings.ambientLight = nightAmbientLight;
            RenderSettings.reflectionIntensity = 0.1f;
            playerLight.intensity = 5f;
            flashLight.intensity = 10f;
            canToggleNightVision = true;
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }
}

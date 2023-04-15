using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBarController : MonoBehaviour
{
    [SerializeField]
    private Image batteryBar;

    [SerializeField]
    private float reduceSpeed = 2;

    private float target = 1;

    public void UpdateBatteryBar(float maxBattery, float currentBattery)
    {
        target = currentBattery / maxBattery;
    }

    void Update()
    {
        batteryBar.fillAmount = Mathf.MoveTowards(batteryBar.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
}

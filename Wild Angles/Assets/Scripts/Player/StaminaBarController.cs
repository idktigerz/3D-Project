using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    [SerializeField]
    private Image staminaBar;

    [SerializeField]
    private float reduceSpeed = 2;

    private float target = 1;

    public void UpdateStaminaBar(float maxStamina, float currentStamina)
    {
        target = currentStamina / maxStamina;
    }

    void Update()
    {
        staminaBar.fillAmount = Mathf.MoveTowards(staminaBar.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
}
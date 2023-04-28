using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    [SerializeField]
    private Image healthbar;

    [SerializeField]
    private float reduceSpeed = 2;

    private float target = 1;

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;
    }

    void Update()
    {
        healthbar.fillAmount = Mathf.MoveTowards(healthbar.fillAmount, target, reduceSpeed * Time.deltaTime);
    }

}

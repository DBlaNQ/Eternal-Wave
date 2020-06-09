using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI healthPacksText;

    private void Start()
    {
        slider = GetComponent<Slider>();
        healthPacksText = GameObject.Find("HealthPacksNumber").GetComponent<TextMeshProUGUI>();
    }

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void SetHealthPacks(int healthPacks)
    {
        healthPacksText.text = Convert.ToString(healthPacks);
    }
}

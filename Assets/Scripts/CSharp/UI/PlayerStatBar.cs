using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image energyImage;
    public Image expImage;

    private void Update()
    {
        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime * 0.5f;
        }
    }

    public void SetHealthPercentage(float percentage)
    {
        healthImage.fillAmount = percentage;
    }

    public void SetEnergyPercentage(float percentage)
    {
        energyImage.fillAmount = percentage;
    }

    public void SetExpPercentage(float percentage)
    {
        expImage.fillAmount = percentage;
    }
}

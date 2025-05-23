using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    
    [Header("监听事件")]
    public PlayerEventSO healthEvent;
    public PlayerEventSO energyEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        energyEvent.OnEventRaised += OnEnergyEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        energyEvent.OnEventRaised -= OnEnergyEvent;
    }

    private void OnHealthEvent(Player player)
    {
        float percentage = player.currentHealth / player.maxHealth;
        playerStatBar.SetHealthPercentage(percentage);
    }
    
    private void OnEnergyEvent(Player player)
    {
        float percentage = player.currentEnergy / player.maxEnergy;
        playerStatBar.SetEnergyPercentage(percentage);
    }
}

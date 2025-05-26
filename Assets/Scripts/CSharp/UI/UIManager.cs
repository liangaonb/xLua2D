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

    [Header("技能UI")]
    public SkillIconUI[] skillIcons;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        energyEvent.OnEventRaised += OnEnergyEvent;
    }

    private void Start()
    {
        Player player = PlayerManager.Instance.player;
        for (int i = 0; i < skillIcons.Length; i++)
        {
            BaseSkill skill = SkillManager.Instance.GetSkill(player.CharacterID, i);
            if (skill != null)
            {
                skillIcons[i].SetSkill(skill);
            }
        }
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

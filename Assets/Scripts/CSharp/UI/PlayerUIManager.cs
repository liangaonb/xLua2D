using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("监听事件")]
    public PlayerEventSO healthEvent;
    public PlayerEventSO energyEvent;
    public PlayerEventSO expEvent;

    [Header("技能UI")]
    public SkillIconUI[] skillIcons;

    [Header("等级UI")]
    public TextMeshProUGUI levelText;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        energyEvent.OnEventRaised += OnEnergyEvent;
        expEvent.OnEventRaised += OnExpEvent;

        PlayerManager.Instance.player.onLevelUp.AddListener(OnPlayerLevelUp);
    }

    private void Start()
    {
        Player player = PlayerManager.Instance.player;
        for (int i = 0; i < skillIcons.Length; i++)
        {
            BaseSkill skill = SkillManager.Instance.GetSkill(player.CharacterID, i + 1); // 跳过NormalAttack
            if (skill != null)
            {
                skillIcons[i].SetSkill(skill);
            }
        }

        UpdateLevelDisplay(player);
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        energyEvent.OnEventRaised -= OnEnergyEvent;
        expEvent.OnEventRaised -= OnExpEvent;

        PlayerManager.Instance.player.onLevelUp.RemoveListener(OnPlayerLevelUp);
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
    
    private void OnPlayerLevelUp(Player player)
    {
        UpdateLevelDisplay(player);
    }

    private void UpdateLevelDisplay(Player player)
    {
        levelText.text = $"Lv: {player.level}";
    }

    private void OnExpEvent(Player player)
    {
        float percentage = player.currentExp / player.expToNextLevel;
        playerStatBar.SetExpPercentage(percentage);
    }
}

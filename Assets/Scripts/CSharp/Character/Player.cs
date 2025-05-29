using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Player : BaseCharacter, ISkillUser
{
    [Header("等级系统")]
    public int level = 1;
    public float currentExp;
    public float expToNextLevel = 100;
    
    [Header("技能预制体")] 
    public NormalAttackSkill normalAttackSkillPrefab;
    public FireballSkill fireballSkillPrefab;
    public RecoverySkill recoverySkillPrefab;
    
    [Header("能量属性")]
    public float maxEnergy;
    public float currentEnergy;
    public float energyGainPerAttack;

    public UnityEvent<Player> onHealthChanged;
    public UnityEvent<Player> onEnergyChanged;
    public UnityEvent<Player> onLevelUp;
    public UnityEvent<Player> onExpChanged;

    private int _characterID;
    [HideInInspector] public int CharacterID => _characterID;
    [HideInInspector] public Vector3 Position => transform.position;
    [HideInInspector] public Vector3 Scale => transform.localScale;

    protected override void Awake()
    {
        base.Awake();
        _characterID = GetInstanceID();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged.Invoke(this);
        
        InitializeSkills();
    }

    public void InitializeSkills()
    {
        var normalAttack = Instantiate(normalAttackSkillPrefab);
        normalAttack.Initialize(this);
        normalAttack.skillIndex = 0;
        SkillManager.Instance.AddSkill(CharacterID, normalAttack);
        
        var fireballSkill = Instantiate(fireballSkillPrefab);
        fireballSkill.Initialize(this);
        fireballSkill.skillIndex = 1;
        SkillManager.Instance.AddSkill(CharacterID, fireballSkill);
        
        var recoverySkill = Instantiate(recoverySkillPrefab);
        recoverySkill.Initialize(this);
        recoverySkill.skillIndex = 2;
        SkillManager.Instance.AddSkill(CharacterID, recoverySkill);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //Debug.Log($"{gameObject.name} takes {damage} damage ");

        if (currentHealth <= 0)
        {
            Die();
        }

        onTakenDamage?.Invoke();
        onHealthChanged?.Invoke(this);
    }

    public override void Die()
    {
        base.Die();
        Debug.Log($"{gameObject.name} died.");

        onDied?.Invoke();
    }

    public void GainEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        onEnergyChanged?.Invoke(this);
    }

    public bool TryUseEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            onEnergyChanged?.Invoke(this);
            return true;
        }
        return false;
    }
    
    public void GainExp(float amount)
    {
        currentExp += amount;
        onExpChanged?.Invoke(this);
        
        while (currentExp >= expToNextLevel)
        {
            LevelUp();
            onLevelUp.Invoke(this);
        }
    }

    private void LevelUp()
    {
        currentExp -= expToNextLevel;
        level++;
        expToNextLevel *= 1f; // 每级所需经验提高
        onLevelUp?.Invoke(this);
    }
    
    public void NormalAttack()
    {
        SkillManager.Instance.UseSkill(CharacterID, 0, Position, Scale);
        
        Debug.Log("Player:NormalAttack");
    }

    public void UseFireballSkill()
    {
        SkillManager.Instance.UseSkill(CharacterID, 1, Position, Scale);
    }

    public void UseRecoverySkill()
    {
        SkillManager.Instance.UseSkill(CharacterID, 2, Position, Scale);
    }
    
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(attackPos, attackSize);
    // }
}

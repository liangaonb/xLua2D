using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : BaseCharacter, ISkillUser
{

    [Header("基础属性")]
    public float maxEnergy;
    public float currentEnergy;
    public float energyGainPerAttack;

    public UnityEvent<Player> OnHealthChanged;
    public UnityEvent<Player> OnEnergyChanged;

    private int _characterID;
    [HideInInspector] public int CharacterID => _characterID;
    [HideInInspector] public Vector3 Position => transform.position;
    [HideInInspector] public Vector3 Scale => transform.localScale;

    [SerializeField] private FireballSkill fireballSkillPrefab;

    protected override void Awake()
    {
        base.Awake();
        _characterID = GetInstanceID();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(this);

        // 创建火球技能实例并添加到技能管理器
        var fireballSkill = Instantiate(fireballSkillPrefab);
        SkillManager.instance.AddSkill(CharacterID, fireballSkill);
    }

    public void NormalAttack()
    {
        attackPos = transform.position;
        //处理玩家朝向
        if (transform.localScale.x > 0)
        {
            offsetX = Mathf.Abs(offsetX);
        }
        if (transform.localScale.x < 0)
        {
            offsetX = -Mathf.Abs(offsetX);
        }
        attackPos.x += offsetX;
        attackPos.y += offsetY;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackPos, attackSize, 0f, targetLayer);
        if (hitColliders != null)
        {
            currentEnergy += energyGainPerAttack * hitColliders.Length;
        }
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<BaseCharacter>().TakeDamage(attackDamage);
        }
        OnEnergyChanged.Invoke(this);
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireCube(attackPos, attackSize);
    // }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        Debug.Log($"{gameObject.name} takes {damage} damage ");

        if (currentHealth <= 0)
        {
            Die();
        }

        onTakenDamage?.Invoke();
        OnHealthChanged?.Invoke(this);
    }

    public override void Die()
    {
        base.Die();
        Debug.Log($"{gameObject.name} died.");

        onDied?.Invoke();
    }
    
    public void UseFireballSkill()
    {
        if (SkillManager.instance != null)
        {
            SkillManager.instance.UseSkill(CharacterID, 0, Position, Scale);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCharacter : MonoBehaviour
{
    [Header("基础属性")]
    public float maxHealth;
    public float currentHealth;
    public float attackDamage;
    public float defense;
    public UnityEvent onTakeDamage;
    public UnityEvent onDie;

    protected Rigidbody2D rb;
    protected Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public virtual void Attack()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // TODO: 添加死亡动画和特效
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }

}

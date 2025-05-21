using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCharacter : MonoBehaviour
{
    [Header("基础属性")]
    public float maxHealth;
    public float currentHealth;
    public float attack;
    public float defense;
    public UnityEvent onTakeDamage;
    public UnityEvent onDie;

    public void Attack(BaseCharacter target)
    {
        float damage = attack - target.defense;
        if (damage < 0) damage = 0;

        target.TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public virtual void Die()
    {
        // TODO: 添加死亡动画和特效
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }

}

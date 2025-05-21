using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : BaseCharacter
{
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        onTakeDamage?.Invoke();
        if (currentHealth <= 0)
        {
            onDie?.Invoke();
        }
    }
}

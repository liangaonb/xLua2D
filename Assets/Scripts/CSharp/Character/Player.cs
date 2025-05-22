using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : BaseCharacter
{
    [Header("攻击范围参数")]
    public Vector2 attackSize;
    public Vector2 attackPos;
    public float offsetX = 1f;
    public float offsetY = 1f;
    public LayerMask enemyLayer;

    public void NormalAttack()
    {
        attackPos = transform.position;
        //处理玩家朝向
        if (transform.localScale.x > 0)
        {
            offsetX =  Mathf.Abs(offsetX);
        }
        if (transform.localScale.x < 0)
        {
            offsetX =  -Mathf.Abs(offsetX);
        }
        attackPos.x += offsetX;
        attackPos.y += offsetY;
        
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackPos, attackSize, 0f, enemyLayer);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<BaseCharacter>().TakeDamage(attackDamage);
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireCube(attackPos, attackSize);
    // }

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

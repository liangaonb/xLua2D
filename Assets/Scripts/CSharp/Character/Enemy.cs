using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseCharacter
{
    public int faceDir;
    private BaseState<Enemy> currentState;
    private readonly BaseState<Enemy> moveState = new EnemyMoveState();
    private readonly BaseState<Enemy> combatState = new EnemyCombatState();

    private void OnEnable()
    {
        currentState = moveState;
        currentState.EnterState(this);

        // 创建血条
        if (EnemyHealthBarManager.Instance != null)
        {
            EnemyHealthBarManager.Instance.CreateHealthBar(this);
        }
    }

    private void Update()
    {
        currentState.LogicUpdate();
        animator.SetBool("isInCombat", isInCombat);
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.ExitState();
    }

    public void Move()
    {
        // 获取玩家位置
        Vector2 playerPos = PlayerManager.Instance.player.transform.position;
        Vector2 enemyPos = transform.position;
    
        // 计算方向
        float directionToPlayer = playerPos.x - enemyPos.x;
        faceDir = (directionToPlayer > 0) ? 1 : -1;
    
        // 移动
        rb.velocity = new Vector2(moveSpeed * faceDir * Time.deltaTime, rb.velocity.y);
    }

    // public override void PlayAttackAnim()
    // {
    //     animator.SetBool("isInCombat", true);
    //     isAttack = true;
    // }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log($"{this.gameObject.name} takes {damage} damage ");

        onTakenDamage.Invoke();
    }

    public void ChangeState(EnemyStates state)
    {
        var newState = state switch
        {
            EnemyStates.Move => moveState,
            EnemyStates.Combat => combatState,
            _ => null
        };
        if (newState != null)
        {
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState(this);
        }
    }

    public void Attack()
    {
        attackPos = transform.position;
        
        attackPos.x += offsetX;
        attackPos.y += offsetY;
        
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackPos, attackSize, 0f, targetLayer);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<BaseCharacter>().TakeDamage(attackDamage);
        }
    }

    public override void Die()
    {
        base.Die();
        Debug.Log($"{gameObject.name} died.");
        WaveManager.Instance.OnEnemyDefeated();
        onDied.Invoke(); //返还到对象池
    }

    public void ResetEnemy()
    {
        currentHealth = maxHealth;
        isInCombat = false;
        
        onTakenDamage.RemoveAllListeners();
        onDied.RemoveAllListeners();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos, attackSize);
    }
}

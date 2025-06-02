using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseCharacter
{
    public int faceDir;
    public float expValue = 50f;
    private Player _player;
    private BaseState<Enemy> _currentState;
    private EnemyMoveState _moveState = new EnemyMoveState();
    private EnemyCombatState _combatState = new EnemyCombatState();

    protected virtual void OnEnable()
    {
        _currentState = _moveState;
        _currentState.EnterState(this);

        // 创建血条
        if (EnemyHealthBarManager.Instance != null)
        {
            EnemyHealthBarManager.Instance.CreateHealthBar(this);
        }
    }

    protected virtual void Start()
    {
        _player = PlayerManager.Instance.player;
    }

    protected virtual void Update()
    {
        _currentState.LogicUpdate();
        animator.SetBool("isInCombat", isInCombat);
    }

    protected virtual void FixedUpdate()
    {
        _currentState.PhysicsUpdate();
    }

    protected virtual void OnDisable()
    {
        _currentState.ExitState();
    }

    public virtual void Move()
    {
        // 获取玩家位置
        Vector2 playerPos = PlayerManager.Instance.player.transform.position;
        Vector2 enemyPos = transform.position;
        
        float directionToPlayer = playerPos.x - enemyPos.x;
        faceDir = (directionToPlayer > 0) ? 1 : -1;
        
        rb.velocity = new Vector2(moveSpeed * faceDir * Time.deltaTime, rb.velocity.y);
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (currentHealth <= 0)
        {
            Die();
        }
        //Debug.Log($"{this.gameObject.name} takes {damage} damage ");

        onTakenDamage.Invoke();
    }

    public void ChangeState(EnemyStates state)
    {
        BaseState<Enemy> newState = state switch
        {
            EnemyStates.Move => _moveState,
            EnemyStates.Combat => _combatState,
            _ => null
        };
        if (newState != null)
        {
            _currentState.ExitState();
            _currentState = newState;
            _currentState.EnterState(this);
        }
    }

    public virtual void Attack()
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
        _player.GainExp(expValue);
        
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

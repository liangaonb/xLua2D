using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseCharacter
{
    private int _faceDir;
    private CheckCondition checkCondition;

    private void Awake()
    {
        base.Awake();
        checkCondition = GetComponent<CheckCondition>();
    }

    private void Update()
    {
        _faceDir = -(int)transform.localScale.x;
        animator.SetBool("isEngaged", isEngaged);
        if (checkCondition.Check())
        {
            if (!isEngaged)
            {
                isEngaged = true;
                PlayAttackAnim();
            }
        }
        else
        {
            isEngaged = false;
            Move();
        }
    }

    public void Move()
    {
        rb.velocity = new Vector2(moveSpeed * _faceDir *  Time.deltaTime, rb.velocity.y);
    }

    public override void PlayAttackAnim()
    {
        animator.SetTrigger("attack");
        animator.SetBool("isAttack", true);
        isAttack = true;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        Debug.Log($"{this.name} takes damage {damage}");
    }
}

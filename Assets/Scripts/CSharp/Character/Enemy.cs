using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseCharacter
{
    public float moveSpeed;
    private int _faceDir;
    private void Update()
    {
        _faceDir = -(int)transform.localScale.x;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        rb.velocity = new Vector2(moveSpeed * _faceDir *  Time.deltaTime, rb.velocity.y);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        Debug.Log($"{this.name} takes damage {damage}");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private PlayerController _playerController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        _animator.SetFloat("velocityX", math.abs(_rb.velocity.x));
    }
    
    public void PlayHurtAnim()
    {
        _animator.SetTrigger("hurt");
    }

    public void PlayAttackAnim()
    {
        _animator.SetTrigger("attack");
        _rb.velocity = new Vector2(0, _rb.velocity.y); // 停止水平移动
    }

    public void AttackAnimationEnd()
    {
        _playerController.isAttacking = false;
    }

    public void PlayDeathAnim()
    {
        _animator.SetBool("isDead", true);
    }
}

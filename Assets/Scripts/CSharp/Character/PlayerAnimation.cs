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
        SetParams();
    }

    void SetParams()
    {
        _animator.SetFloat("velocityX", math.abs(_rb.velocity.x));
        _animator.SetBool("isDead", _playerController.isDead);
    }

    public void PlayHurt()
    {
        _animator.SetTrigger("hurt");
    }

    public void PlayAttackAnim()
    {
        _animator.SetTrigger("attack");
    }
}

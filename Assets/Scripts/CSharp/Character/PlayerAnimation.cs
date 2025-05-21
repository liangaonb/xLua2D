using System;
using System.Collections;
using System.Collections.Generic;
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
        _animator.SetFloat("velocityX", _rb.velocity.x);
        _animator.SetBool("isDead", _playerController.isDead);
    }

    public void PlayHurt()
    {
        _animator.SetTrigger("hurt");
    }
}

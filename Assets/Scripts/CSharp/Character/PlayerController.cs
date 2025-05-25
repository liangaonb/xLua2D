using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputControl _inputControl;
    public float moveSpeed = 200f;
    private Rigidbody2D _rb;
    private PlayerAnimation _playerAnimation;
    private Vector2 _inputDirection;
    private int _faceDir;
    public bool isDead;

    private void Awake()
    {
        _inputControl = new PlayerInputControl();
        _rb = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();

        _inputControl.Gameplay.Attack.started += PlayerAttack;
        _inputControl.Gameplay.Skill1.started += PlayerSkill1;
    }


    private void Start()
    {
        //_rb.velocity = new Vector2(moveSpeed, 0);
    }

    private void OnEnable()
    {
        _inputControl.Enable();
    }

    private void OnDisable()
    {
        _inputControl.Disable();
    }

    private void Update()
    {
        _inputDirection = _inputControl.Gameplay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // 保持恒定速度
        //_rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);

        Move();
    }

    public void Move()
    {
        _rb.velocity = new Vector2(_inputDirection.x * moveSpeed * Time.deltaTime, _rb.velocity.y);
        _faceDir = (int)transform.localScale.x;
        if (_inputDirection.x > 0)
        {
            _faceDir = 1;
        }

        if (_inputDirection.x < 0)
        {
            _faceDir = -1;
        }
        transform.localScale = new Vector3(_faceDir, 1, 1);
    }

    public void PlayerDead()
    {
        isDead = true;
        _playerAnimation.PlayDeathAnim();
        _inputControl.Gameplay.Disable();
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        _playerAnimation.PlayAttackAnim();
    }

    
    private void PlayerSkill1(InputAction.CallbackContext context)
    {
        PlayerManager.instance.player.UseFireballSkill();
    }
}
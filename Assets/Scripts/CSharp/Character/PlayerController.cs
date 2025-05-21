using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public float moveSpeed = 200f;
    private Rigidbody2D _rb;

    public bool isDead;

    private void Awake()
    {
        inputControl = new PlayerInputControl();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rb.velocity = new Vector2(moveSpeed, 0);
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void FixedUpdate()
    {
        // 保持恒定速度，防止其他力影响
        _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
    }

    public void PlayerDead()
    {
        isDead = true;
        // TODO: 添加死亡处理逻辑
    }
}
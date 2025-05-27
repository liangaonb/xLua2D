using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputControl _inputControl;
    public float walkSpeed = 100f;
    public float runSpeed = 200f;
    public float moveSpeed; // 当前速度
    private Rigidbody2D _rb;
    private PlayerAnimation _playerAnimation;
    private int _faceDir = 1;
    public bool isDead;
    public bool isAttacking;
    public bool isInCombat;

    private BaseState<PlayerController> _currentState = new PlayerWalkState();
    private PlayerWalkState _walkState = new PlayerWalkState();
    private PlayerRunState _runState = new PlayerRunState();
    private PlayerCombatState _combatState = new PlayerCombatState();
    public CheckCondition checkCondition;

    private void Awake()
    {
        _inputControl = new PlayerInputControl();
        _rb = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        checkCondition = GetComponent<CheckCondition>();

        _inputControl.Gameplay.Skill0.started += PlayerAttack;
        _inputControl.Gameplay.Skill1.started += PlayerSkill1;
        _inputControl.Gameplay.Skill2.started += PlayerSkill2;
        
    }

    private void OnEnable()
    {
        _inputControl.Gameplay.Enable();
    }

    private void Start()
    {
        // 初始状态为walk
        ChangeState(PlayerStates.Walk);
    }

    private void Update()
    {
        // 在Running状态下检测敌人
        if (_currentState == _runState && checkCondition.CheckTarget())
        {
            isInCombat = true;
            ChangeState(PlayerStates.Combat);
        }
        _currentState.LogicUpdate();       
    }

    private void FixedUpdate()
    {
        _currentState.PhysicsUpdate();
    }

    public void AutoMove()
    {
        if (isAttacking) return;
        _rb.velocity = new Vector2(moveSpeed * Time.deltaTime * _faceDir, _rb.velocity.y);
    }

    public void ChangeState(PlayerStates state)
    {
        BaseState<PlayerController> newState = state switch
        {
            PlayerStates.Walk => _walkState,
            PlayerStates.Run => _runState,
            PlayerStates.Combat => _combatState,
            _ => null
        };

        if (newState != null)
        {
            _currentState?.ExitState();
            _currentState = newState;
            _currentState.EnterState(this);
        }
    }

    // 当所有敌人被击败后退出战斗状态
    public void ExitCombat()
    {
        if (_currentState == _combatState)
        {
            isInCombat = false;
            ChangeState(PlayerStates.Run);
        }
    }

    public void PlayerDead()
    {
        isDead = true;
        _playerAnimation.PlayDeathAnim();
        _inputControl.Gameplay.Disable();
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if (isAttacking) return;
        isAttacking = true;
        _playerAnimation.PlayAttackAnim();
    }

    
    private void PlayerSkill1(InputAction.CallbackContext context)
    {
        PlayerManager.Instance.player.UseFireballSkill();
    }

    private void PlayerSkill2(InputAction.CallbackContext context)
    {
        PlayerManager.Instance.player.UseRecoverySkill();
    }
}
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
        float speed = math.abs(_rb.velocity.x);
        _animator.SetFloat("velocityX", speed);
        
        if (speed > 0)
        {
            bool isRunning = GameStateManager.Instance.currentGameState == GameState.Playing;
            _animator.SetBool("isRunning", isRunning);
        }
    }
    
    public void PlayHurtAnim()
    {
        _animator.SetTrigger("hurt");
    }

    public void PlayAttackAnim()
    {
        _animator.SetTrigger("attack");
        _rb.velocity = new Vector2(0, _rb.velocity.y); // 攻击时停止移动
        
        Debug.Log("PlayerAnimation:PlayerAttack");
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

using UnityEngine;
using UnityEngine.Events;

public class BaseCharacter : MonoBehaviour
{
    [Header("基础属性")]
    public float maxHealth;
    public float currentHealth;
    public float attackDamage;
    public float moveSpeed;

    [Header("攻击范围参数")]
    public Vector2 attackSize;
    public Vector2 attackPos;
    public float offsetX = 1f;
    public float offsetY = 1f;
    public LayerMask targetLayer;

    [Header("对象状态")]
    public bool isInCombat = false; // 是否进战

    public UnityEvent onTakenDamage;
    public UnityEvent onDied;
    protected Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CheckCondition checkCondition;
    protected float damageMultiplier = 1f; // 伤害加成系数

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        checkCondition = GetComponent<CheckCondition>();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public virtual void Die()
    {
    }
    
    public virtual void OnDestroy()
    {
    }
}

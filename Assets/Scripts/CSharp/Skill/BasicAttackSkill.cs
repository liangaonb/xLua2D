using UnityEngine;


public class BasicAttackSkill : BaseSkill
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask targetLayer;

    public override void UseSkill()
    {
        // 在这里实现具体的攻击逻辑
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayer);

        foreach (Collider2D hit in hits)
        {
           
            hit.GetComponent<BaseCharacter>().TakeDamage(damage);
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}


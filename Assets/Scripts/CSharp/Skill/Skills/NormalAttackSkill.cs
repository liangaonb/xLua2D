using Unity.VisualScripting;
using UnityEngine;

public class NormalAttackSkill : BaseSkill
{
    public override void UseSkill(float damageMultiplier)
    {
        //Debug.Log("NormalAttackSkill");
        if (!CanUseSkill()) return;
        
        base.UseSkill(damageMultiplier);

        Vector2 attackPos = skillUser.Position;
        float offsetX = config.offsetFromPlayer.x * (skillUser.Scale.x > 0 ? 1 : -1);
        attackPos.x += offsetX;
        attackPos.y += config.offsetFromPlayer.y;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackPos, config.attackSize, 0f, config.affectLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<BaseCharacter>(out var character))
            {
                character.TakeDamage(config.damage * damageMultiplier);
            }
        }
        
        // 玩家使用普通攻击增加能量
        if (skillUser is Player player)
        {
            player.GainEnergy(player.energyGainPerAttack * hitColliders.Length);
        }
    }
}
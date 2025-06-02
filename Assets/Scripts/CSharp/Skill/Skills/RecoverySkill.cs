using UnityEngine;

public class RecoverySkill : BaseSkill
{
    public override void UseSkill(float damageMultiplier)
    {
        if (base.CanUseSkill())
        {
            base.UseSkill(damageMultiplier);
            if (skillUser is Player player)
            {
                // 消耗所有能量回血，值为消耗能量的33%
                player.currentHealth = Mathf.Clamp(player.currentHealth + player.currentEnergy / 3f, 0, player.maxHealth);
                player.onHealthChanged?.Invoke(player);
                player.currentEnergy = 0;
                player.onEnergyChanged?.Invoke(player);
            }
        }      
    }
}
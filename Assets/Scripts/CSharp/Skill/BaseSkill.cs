using UnityEngine;


public abstract class BaseSkill : MonoBehaviour
{
    [Header("技能基本参数")]
    public float cooldownTime = 1f;
    public float damage = 20f;
    public float energyCost = 20f;

    protected bool isInCooldown = false;
    protected float cooldownTimer = 0f;

    protected virtual void Update()
    {
        if (isInCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownTime)
            {
                isInCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    public virtual bool CanUseSkill()
    {
        return !isInCooldown;
    }

    public virtual void UseSkill()
    {
        isInCooldown = true;
    }

    public float GetCooldownPercent()
    {
        return isInCooldown ? cooldownTimer / cooldownTime : 0f;
    }
}


using UnityEngine;


public abstract class BaseSkill : MonoBehaviour
{
    public SkillConfigSO config;

    protected bool isInCooldown = false;
    protected float cooldownTimer = 0f;
    protected ISkillUser skillUser;

    protected virtual void Update()
    {
        if (isInCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= config.cooldownTime)
            {
                isInCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    public void Initialize(ISkillUser user)
    {
        skillUser = user;
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
        return isInCooldown ? cooldownTimer / config.cooldownTime : 0f;
    }
}


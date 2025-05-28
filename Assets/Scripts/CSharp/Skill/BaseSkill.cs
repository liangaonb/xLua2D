using UnityEngine;


public abstract class BaseSkill : MonoBehaviour
{
    public BaseSkillConfigSO config;
    public SkillUnlockConfigSO unlockConfig;
    public int skillIndex;

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
        if (!isInCooldown && unlockConfig != null)
        {
            // 使用SkillPanelUI中的解锁信息
            var skillPanel = FindObjectOfType<SkillPanelUI>();
            if (skillPanel != null)
            {
                return skillPanel.IsSkillUnlocked(unlockConfig.skillUnlockData[skillIndex].skillName);
            }
        }
        return false;
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


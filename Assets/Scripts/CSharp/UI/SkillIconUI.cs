using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconUI : MonoBehaviour
{
    public Image skillIcon;
    public Image cooldownMask;
    public TextMeshProUGUI cooldownText;

    private BaseSkill _skill;

    private void Update()
    {
        if (_skill != null)
        {
            float cooldownPercent = _skill.GetCooldownPercent();
            cooldownMask.fillAmount = cooldownPercent;
            
            if (cooldownPercent > 0)
            {
                cooldownText.text = Mathf.Ceil(_skill.config.cooldownTime * (1 - cooldownPercent)).ToString();
                cooldownText.gameObject.SetActive(true);
            }
            else
            {
                cooldownText.gameObject.SetActive(false);
            }
        }
    }

    public void SetSkill(BaseSkill skill)
    {
        _skill = skill;
    }
}

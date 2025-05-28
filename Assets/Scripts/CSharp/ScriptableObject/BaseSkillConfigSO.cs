using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BaseSkillConfigSO")]
public class BaseSkillConfigSO : ScriptableObject
{
    [Header("技能描述")]
    public string skillName;
    public string description;
    public Sprite skillIcon;
    
    [Header("技能参数")]
    public float damage = 20f;
    public float cooldownTime = 2f;
    public float energyCost = 0f;
    public LayerMask affectLayer;
    
    [Header("攻击范围")]
    public Vector2 attackSize = new Vector2(1.3f, 2f);
    public Vector2 offsetFromPlayer = new Vector2(1f, 1f);
}
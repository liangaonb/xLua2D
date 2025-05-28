using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SkillUnlockConfigSO")]
public class SkillUnlockConfigSO : ScriptableObject
{
    [System.Serializable]
    public class SkillUnlockData
    {
        public string skillName;
        public int requiredLevel;
        public Sprite skillIcon;
        [TextArea] public string description;
        public bool isUnlocked;
    }

    public SkillUnlockData[] skillUnlockData;
}

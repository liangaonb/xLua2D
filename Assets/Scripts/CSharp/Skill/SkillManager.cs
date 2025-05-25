using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    // 存储每个实体的技能列表
    private Dictionary<int, List<BaseSkill>> characterSkills = new Dictionary<int, List<BaseSkill>>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddSkill(int characterID, BaseSkill skill)
    {
        if (!characterSkills.ContainsKey(characterID))
        {
            characterSkills[characterID] = new List<BaseSkill>();
        }

        if (!characterSkills[characterID].Contains(skill))
        {
            characterSkills[characterID].Add(skill);
            skill.transform.SetParent(null); // 防止技能随实体被销毁
            DontDestroyOnLoad(skill.gameObject);
        }
    }
    public void RemoveSkill(int characterID, BaseSkill skill)
    {
        if (characterSkills.ContainsKey(characterID) && characterSkills[characterID].Contains(skill))
        {
            characterSkills[characterID].Remove(skill);
            Destroy(skill.gameObject);
        }
    }

    public bool UseSkill(int characterID, int index, Vector3 userPosition, Vector3 userScale)
    {
        if (!characterSkills.ContainsKey(characterID))
            return false;

        var skills = characterSkills[characterID];
        if (index >= 0 && index < skills.Count)
        {
            if (skills[index].CanUseSkill())
            {
                // 技能使用时已施法者位置为基准
                skills[index].transform.position = userPosition;
                skills[index].transform.localScale = userScale;
                skills[index].UseSkill();
                return true;
            }
        }
        return false;
    }

    public BaseSkill GetSkill(int characterID, int index)
    {
        if (characterSkills.ContainsKey(characterID) && index >= 0 && index < characterSkills[characterID].Count)
        {
            return characterSkills[characterID][index];
        }
        return null;
    }

    // 清理指定角色的所有技能
    public void ClearEntitySkills(int characterID)
    {
        if (characterSkills.ContainsKey(characterID))
        {
            foreach (var skill in characterSkills[characterID])
            {
                if (skill != null)
                {
                    Destroy(skill.gameObject);
                }
            }
            characterSkills.Remove(characterID);
        }
    }

    private void OnDestroy()
    {
        // 清理所有技能
        foreach (var skillList in characterSkills.Values)
        {
            foreach (var skill in skillList)
            {
                if (skill != null)
                {
                    Destroy(skill.gameObject);
                }
            }
        }
        characterSkills.Clear();
    }
}


using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    // 存储每个角色的技能列表
    private Dictionary<int, List<BaseSkill>> _characterSkills = new Dictionary<int, List<BaseSkill>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddSkill(int characterID, BaseSkill skill)
    {
        if (!_characterSkills.ContainsKey(characterID))
        {
            _characterSkills[characterID] = new List<BaseSkill>();
        }

        if (!_characterSkills[characterID].Contains(skill))
        {
            _characterSkills[characterID].Add(skill);
        }
    }
    
    public void RemoveSkill(int characterID, BaseSkill skill)
    {
        if (_characterSkills.ContainsKey(characterID) && _characterSkills[characterID].Contains(skill))
        {
            _characterSkills[characterID].Remove(skill);
            Destroy(skill.gameObject);
        }
    }

    public bool UseSkill(int characterID, int index, Vector3 userPosition, Vector3 userScale, float damageMultiplier)
    {
        if (!_characterSkills.ContainsKey(characterID))
            return false;

        var skills = _characterSkills[characterID];
        if (index >= 0 && index < skills.Count)
        {
            if (skills[index].CanUseSkill())
            {
                // 技能使用时已施法者位置为基准
                skills[index].transform.position = userPosition;
                skills[index].transform.localScale = userScale;
                skills[index].UseSkill(damageMultiplier);
                return true;
            }
        }
        return false;
    }

    public BaseSkill GetSkill(int characterID, int index)
    {
        if (_characterSkills.ContainsKey(characterID) && index >= 0 && index < _characterSkills[characterID].Count)
        {
            return _characterSkills[characterID][index];
        }
        return null;
    }

    // 清理指定角色的所有技能
    public void ClearCharacterSkills(int characterID)
    {
        if (_characterSkills.ContainsKey(characterID))
        {
            foreach (var skill in _characterSkills[characterID])
            {
                if (skill != null)
                {
                    Destroy(skill.gameObject);
                }
            }
            _characterSkills.Remove(characterID);
        }
    }

    // 清理所有技能
    private void OnDestroy()
    {
        foreach (var skillList in _characterSkills.Values)
        {
            foreach (var skill in skillList)
            {
                if (skill != null)
                {
                    Destroy(skill.gameObject);
                }
            }
        }
        _characterSkills.Clear();
    }
}


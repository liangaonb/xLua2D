using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SkillPanelUI : MonoBehaviour
{
    public SkillUnlockConfigSO skillUnlockConfig;
    public Transform skillTreeContent; // 存放skillNode元素
    public GameObject skillTreePanel;
    private PlayerController _playerController;
    private Dictionary<string, bool> _runtimeSkillUnlockInfo = new Dictionary<string, bool>(); //存储初始解锁状态
    
    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerController.inputControl.UI.ToggleUIPanel.started += OnToggleSkillPanelUI;

        InitRuntimeUnlockInfo();
        
        // 订阅玩家升级事件
        PlayerManager.Instance.player.OnLevelUp.AddListener(OnPlayerLevelUp);
        
        InitializeSkillTree();
        skillTreePanel.SetActive(false);
    }

    private void InitRuntimeUnlockInfo()
    {
        foreach (var skillData in skillUnlockConfig.skillUnlockData)
        {
            _runtimeSkillUnlockInfo[skillData.skillName] = skillData.isUnlocked;
        }
    }

    public bool IsSkillUnlocked(string skillName)
    {
        return _runtimeSkillUnlockInfo[skillName];
    }

    private void OnPlayerLevelUp(Player player)
    {
        UpdateAllSkillNodes();
    }

    private void OnToggleSkillPanelUI(InputAction.CallbackContext obj)
    {
        ToggleSkillTree();
    }

    private void ToggleSkillTree()
    {
        bool isActive = !skillTreePanel.activeSelf;
        skillTreePanel.SetActive(isActive);
        Time.timeScale = isActive ? 0 : 1;
    }

    private void InitializeSkillTree()
    {
        // 遍历panel中的技能节点并写入SO中的数据
        for (int i = 0; i < skillTreeContent.childCount; i++)
        {
            GameObject node = skillTreeContent.GetChild(i).gameObject;
            string nodeName = node.name;
        
            // 查找对应技能数据
            var skillData = skillUnlockConfig.skillUnlockData.FirstOrDefault(data => data.skillName == nodeName);
        
            if (skillData != null)
            {
                SetupSkillNode(node, skillData);
            }
            else
            {
                Debug.LogWarning($"未找到名为 {nodeName} 的技能配置数据");
            }
        }
    }

    private void SetupSkillNode(GameObject node, SkillUnlockConfigSO.SkillUnlockData skillData)
    {
        // 设置SkillNode
        node.transform.Find("SkillIcon").GetComponent<Image>().sprite = skillData.skillIcon;
        node.transform.Find("SkillName").GetComponent<TextMeshProUGUI>().text = skillData.skillName;
        node.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = skillData.description;
        node.transform.Find("LevelRequirement").GetComponent<TextMeshProUGUI>().text = $"Required level: {skillData.requiredLevel}";
        
        // 设置解锁按钮
        Button unlockButton = node.transform.Find("UnlockButton").GetComponent<Button>();
        unlockButton.onClick.AddListener(() => TryUnlockSkill(skillData));
        
        UpdateSkillNodeStatus(node, skillData);
    }

    private void TryUnlockSkill(SkillUnlockConfigSO.SkillUnlockData skillData)
    {
        Player player = PlayerManager.Instance.player;
        if (!_runtimeSkillUnlockInfo[skillData.skillName] && player.level >= skillData.requiredLevel)
        {
            _runtimeSkillUnlockInfo[skillData.skillName] = true;
            
            // 更新UI显示
            UpdateAllSkillNodes();
        }
    }

    private void UpdateAllSkillNodes()
    {
        // 更新所有技能节点的显示状态
        for (int i = 0; i < skillTreeContent.childCount; i++)
        {
            var node = skillTreeContent.GetChild(i).gameObject;
            var skillData = skillUnlockConfig.skillUnlockData[i + 1]; // 跳过NormalAttack技能
            UpdateSkillNodeStatus(node, skillData);
        }
    }

    private void UpdateSkillNodeStatus(GameObject node, SkillUnlockConfigSO.SkillUnlockData skillUnlockData)
    {
        Button unlockButton = node.transform.Find("UnlockButton").GetComponent<Button>();
        Image lockIcon = node.transform.Find("LockIcon").GetComponent<Image>();
        
        bool isUnlocked = _runtimeSkillUnlockInfo[skillUnlockData.skillName];
        if (isUnlocked)
        {
            unlockButton.gameObject.SetActive(false);
            lockIcon.gameObject.SetActive(false);
        }
        else
        {
            unlockButton.gameObject.SetActive(true);
            lockIcon.gameObject.SetActive(true);
            
            Player player = PlayerManager.Instance.player;
            unlockButton.interactable = player.level >= skillUnlockData.requiredLevel;
        }
    }
}
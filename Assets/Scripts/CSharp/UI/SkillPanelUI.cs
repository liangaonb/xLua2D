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
    
    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerController.inputControl.UI.ToggleUIPanel.started += OnToggleSkillPanelUI;
        
        InitializeSkillTree();
        skillTreePanel.SetActive(false);
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
        // 遍历现有的技能节点
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
        if (!skillData.isUnlocked && player.level >= skillData.requiredLevel)
        {
            skillData.isUnlocked = true;
            // 更新UI显示
            UpdateSkillNodes();
        }
    }

    private void UpdateSkillNodes()
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
        
        if (skillUnlockData.isUnlocked)
        {
            unlockButton.gameObject.SetActive(false);
            lockIcon.gameObject.SetActive(false);
        }
        else
        {
            unlockButton.gameObject.SetActive(true);
            lockIcon.gameObject.SetActive(true);
            
            // 检查是否达到等级要求
            Player player = PlayerManager.Instance.player;
            unlockButton.interactable = player.level >= skillUnlockData.requiredLevel;
        }
    }
}
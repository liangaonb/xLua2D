using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class UIPanelsController : MonoBehaviour
{
    public SkillUnlockConfigSO skillUnlockConfig;
    public GameObject rootPanel; // 各panel的父物体
    public Transform skillTreeContent; // 存放skillNode元素
    public GameObject skillTreePanel;
    private PlayerController _playerController;
    private Dictionary<string, bool> _runtimeSkillUnlockInfo = new Dictionary<string, bool>(); //存储初始解锁状态

    public GameObject storePanel;
    public GameObject servantPanel;

    [Header("背包系统")]
    public GameObject inventoryPanel;  // 背包面板
    public Transform itemContainer;    // 物品容器
    public GameObject itemSlotPrefab;  // 物品槽

    private Dictionary<Equipment, Button> _equipmentButtons = new Dictionary<Equipment, Button>(); // 保存装备和按钮的对应关系
    private Player _player;

    public Button preButton;
    public Button nextButton;

    private int _currentPanelIndex = 1; // 0,1,2,3对应storePanel,skillPanel,inventoryPanel,servantPanel
    private GameObject[] _panels;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _player = PlayerManager.Instance.player;
        _playerController.inputControl.UI.ToggleUIPanel.started += OnToggleRootPanelUI;

        InitRuntimeUnlockInfo();

        // 订阅玩家升级事件，升级后更新可解锁技能
        _player.onLevelUp.AddListener(OnPlayerLevelUp);

        // 订阅拾取和装备事件
        _player.onItemPickup.AddListener(AddItemToUI);
        _player.onItemEquipped.AddListener(UpdateEquipmentUI);

        InitializeSkillTree();

        _panels = new GameObject[] { storePanel, skillTreePanel, inventoryPanel, servantPanel };

        preButton.onClick.AddListener(ShowPreviousPanel);
        nextButton.onClick.AddListener(ShowNextPanel);

        rootPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_playerController != null && _playerController.inputControl != null)
        {
            _playerController.inputControl.UI.ToggleUIPanel.started -= OnToggleRootPanelUI;
        }
        if (PlayerManager.Instance != null && PlayerManager.Instance.player != null)
        {
            PlayerManager.Instance.player.onLevelUp.RemoveListener(OnPlayerLevelUp);
        }
        if (preButton != null)
        {
            preButton.onClick.RemoveListener(ShowPreviousPanel);
        }
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(ShowNextPanel);
        }
        if (_player != null)
        {
            _player.onItemPickup.RemoveListener(AddItemToUI);
            _player.onItemEquipped.RemoveListener(UpdateEquipmentUI);
        }
    }

    #region 面板控制
    private void OnToggleRootPanelUI(InputAction.CallbackContext obj)
    {
        ToggleRootPanel();
    }

    private void ToggleRootPanel()
    {
        if (rootPanel == null) return;

        bool isActive = !rootPanel.activeSelf;
        rootPanel.SetActive(isActive);
        UpdatePanelVisibility();
        Time.timeScale = isActive ? 0 : 1;
    }

    private void ShowPreviousPanel()
    {
        _currentPanelIndex--;
        if (_currentPanelIndex < 0)
        {
            _currentPanelIndex = _panels.Length - 1;
        }
        UpdatePanelVisibility();
    }

    private void ShowNextPanel()
    {
        _currentPanelIndex++;
        if (_currentPanelIndex >= _panels.Length)
        {
            _currentPanelIndex = 0;
        }
        UpdatePanelVisibility();
    }

    private void UpdatePanelVisibility()
    {
        if (rootPanel.activeSelf == true)
        {
            for (int i = 0; i < _panels.Length; i++)
            {
                if (_panels[i] != null)
                {
                    _panels[i].SetActive(i == _currentPanelIndex);
                }
            }
        }
    }
    #endregion

    #region 背包系统
    public void AddItemToUI(Equipment equipment)
    {
        GameObject slot = Instantiate(itemSlotPrefab, itemContainer);

        // 设置物品图标
        slot.GetComponentInChildren<Image>().sprite = equipment.icon;

        // 添加点击事件直接装备物品
        Button button = slot.GetComponent<Button>();
        button.onClick.AddListener(() => OnItemClick(equipment, button));

        // 保存装备和按钮的对应关系
        _equipmentButtons[equipment] = button;
    }

    private void OnItemClick(Equipment equipment, Button clickedButton)
    {
        if (equipment != null)
        {
            _player.EquipItem(equipment);
        }
    }

    private void UpdateEquipmentUI(Equipment equipment)
    {
        // 重置所有按钮下的图标颜色
        foreach (var btn in _equipmentButtons.Values)
        {
            var icon = btn.GetComponentInChildren<Image>();
            if (icon != null)
            {
                icon.color = Color.white;
            }
        }

        // 获取玩家所有已装备的物品并高亮显示
        foreach (var equippedItem in _player.equippedItems.Values)
        {
            if (_equipmentButtons.TryGetValue(equippedItem, out Button equippedButton))
            {
                var icon = equippedButton.GetComponentInChildren<Image>();
                if (icon != null)
                {
                    icon.color = Color.yellow;
                }
            }
        }
    }
    #endregion

    #region 技能系统 
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
    #endregion
}
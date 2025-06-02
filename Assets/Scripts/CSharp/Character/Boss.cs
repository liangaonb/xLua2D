using UnityEngine;

public class Boss : Enemy, ISkillUser
{
    [Header("Boss技能")]
    public FireballSkill fireballSkillPrefab;
    public float skillUseChance = 0.4f; // 使用技能的概率
    
    [HideInInspector] public int CharacterID => GetInstanceID();
    [HideInInspector] public Vector3 Position => transform.position;
    [HideInInspector] public Vector3 Scale => transform.localScale;

    private SkillManager _skillManager;

    protected override void Awake()
    {
        base.Awake();      
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
        _skillManager = SkillManager.Instance;
        InitializeSkills();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    // Boss被销毁时清理技能
    public override void OnDestroy()
    {
        if (_skillManager != null)
        {
            _skillManager.ClearEntitySkills(CharacterID);
        }
    }

    public void InitializeSkills()
    {
        // 初始化火球技能
        var fireballSkill = Instantiate(fireballSkillPrefab);
        fireballSkill.Initialize(this);
        fireballSkill.skillIndex = 0;
        _skillManager.AddSkill(CharacterID, fireballSkill);
    }

    // Boss攻击时有几率使用技能
    public override void Attack()
    {
        if (Random.value < skillUseChance)
        {
            UseFireballSkill();
        }
        else 
        {
            base.Attack();
        }
    }

    private void UseFireballSkill()
    {
        _skillManager.UseSkill(CharacterID, 0, Position, new Vector3(faceDir, 1, 1), damageMultiplier);
    }  
}
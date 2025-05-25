using UnityEngine;


public class FireballSkill : BaseSkill
{
    [Header("Fireball Settings")]
    public GameObject fireballPrefab; // 火球预制体
    public Vector2 spawnOffset = new Vector2(1f, 0f); // 生成位置偏移

    public override void UseSkill()
    {
        if (CanUseSkill())
        {
            base.UseSkill();

            // 获取角色朝向
            int faceDir = transform.localScale.x > 0 ? 1 : -1;
            Vector2 spawnPosition = (Vector2)transform.position + new Vector2(spawnOffset.x * faceDir, spawnOffset.y);
            // 实例化火球
            GameObject fireballObj = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
            // 初始化火球
            Fireball fireball = fireballObj.GetComponent<Fireball>();
            if (fireball != null)
            {
                fireball.Init(damage, faceDir);
            }
        }       
    }
}


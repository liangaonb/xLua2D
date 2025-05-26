using UnityEngine;

public class EnemyHealthBarManager : MonoBehaviour
{
    public static EnemyHealthBarManager Instance;
    public Transform EnemyHealthBarContainer; // 用于存放所有血条的父物体
    public GameObject healthBarPrefab;

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

    public void CreateHealthBar(Enemy enemy)
    {
        GameObject healthBarObj = Instantiate(healthBarPrefab, EnemyHealthBarContainer);
        EnemyHealthBar healthBar = healthBarObj.GetComponent<EnemyHealthBar>();
        healthBar.SetupHealthBar(enemy);
    }
}
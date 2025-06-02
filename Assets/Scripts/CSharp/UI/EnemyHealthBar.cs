using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthFillImage;
    private Camera _mainCamera;
    private Enemy _target;
    public Vector3 offset = new Vector3(0, 3, 0); // 血条在敌人头顶上方的偏移量

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            // 跟随目标
            Vector3 worldPosition = _target.transform.position + offset;
            transform.position = _mainCamera.WorldToScreenPoint(worldPosition);
        }
    }

    private void OnDestroy()
    {
        if (_target != null)
        {
            _target.onTakenDamage.RemoveListener(UpdateHealthBar);
            _target.onDied.RemoveListener(OnTargetDied);
        }
    }

    public void SetupHealthBar(Enemy target)
    {
        _target = target;
        UpdateHealthBar();
        _target.onTakenDamage.AddListener(UpdateHealthBar);
        _target.onDied.AddListener(OnTargetDied);
    }
    
    private void UpdateHealthBar()
    {
        if (_target != null)
        {
            float healthPercentage = _target.currentHealth / _target.maxHealth;
            healthFillImage.fillAmount = healthPercentage;
        }
    }

    private void OnTargetDied()
    {
        Destroy(gameObject);
    }
}

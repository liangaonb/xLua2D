using UnityEngine;


public class Fireball : MonoBehaviour
{
    public float damage;
    public float speed = 30f;
    public float maxDistance = 900f;
    public int maxHitCount = 3;
    private Vector2 _startPosition;
    private int _direction = 1; // 1为右，-1为左
    private int _currentHitCount = 0;

    public void Init(float damage, int faceDir)
    {
        this.damage = damage;
        _direction = faceDir;
        _startPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(_direction * speed * Time.deltaTime * Vector2.right);

        // 检查是否超出最大距离
        if (Vector2.Distance(_startPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰撞对象是否是敌人并造成伤害
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            ++_currentHitCount;
            if (_currentHitCount >= maxHitCount)
            {
                Destroy(gameObject);
            }
        }
    }
}


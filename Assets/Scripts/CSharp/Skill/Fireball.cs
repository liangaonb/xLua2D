using UnityEngine;


public class Fireball : MonoBehaviour
{
    public float damage;
    public float speed = 30f;
    public float maxDistance = 900f;
    private Vector2 startPosition;
    private int direction = 1; // 1为右，-1为左
    public int hitCount = 0;

    public void Init(float damage, int faceDir)
    {
        this.damage = damage;
        direction = faceDir;
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime * Vector2.right);

        // 检查是否超出最大距离
        if (Vector2.Distance(startPosition, transform.position) > maxDistance)
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
            ++hitCount;
            if (hitCount >= 3)
            {
                Destroy(gameObject);
            }
        }
    }
}


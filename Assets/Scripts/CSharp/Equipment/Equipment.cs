using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor
}

public class Equipment : MonoBehaviour
{
    public EquipmentType type;
    public string equipmentName;
    public Sprite icon;
    
    [Header("属性加成")]
    public float healthBonus;
    public float damageMultiplierBonus;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddToInventory(this);
                gameObject.SetActive(false);
            }
        }
    }
}
using UnityEngine;

public class EquipmentDatabase : MonoBehaviour
{
    public static EquipmentDatabase Instance;
    public GameObject[] equipmentPrefabs;
    
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
    
    public GameObject GetRandomEquipment()
    {
        if (equipmentPrefabs.Length == 0)
        {
            return null;
        }
           
        int randomIndex = Random.Range(0, equipmentPrefabs.Length);
        return equipmentPrefabs[randomIndex];
    }
}
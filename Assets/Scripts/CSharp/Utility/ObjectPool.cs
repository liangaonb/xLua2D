using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();

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

    public GameObject GetObject(GameObject prefab)
    {
        string key = prefab.name;
        
        // 没有则创建对应的Queue
        if (!_poolDictionary.ContainsKey(key))
        {
            _poolDictionary[key] = new Queue<GameObject>();
        }
        
        // 有则返回对象
        if (_poolDictionary[key].Count > 0)
        {
            GameObject obj = _poolDictionary[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        
        // 无可用对象，创建
        GameObject newObj = Instantiate(prefab);
        newObj.name = key;
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        string key = obj.name;
        obj.SetActive(false);

        //重置状态
        Enemy returnedEnemy = obj.GetComponent<Enemy>();
        if (returnedEnemy != null)
        {
            returnedEnemy.ResetEnemy();
        }
        
        if (!_poolDictionary.ContainsKey(key))
        {
            _poolDictionary[key] = new Queue<GameObject>();
        }
        _poolDictionary[key].Enqueue(obj);
    }
}

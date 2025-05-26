using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    private Vector3 _spawnPoint;
    private ObjectPool _objectPool;
    private Player _player;

    private void Start()
    {
        _objectPool = ObjectPool.Instance;
        _player = PlayerManager.Instance.player;
    }

    public void SpawnEnemy(string enemyType)
    {
        GameObject prefab = enemyPrefabs.Find(x => x.name == enemyType);
        if (prefab == null)
        {
            return;
        }
        
        // 在指定位置生成从对象池获取的敌人
        if (enemyType == "EnemyBee")
        {
            _spawnPoint = _player.transform.position + Vector3.right * 20 + Vector3.up * 2f;
        }
        else
        {
            _spawnPoint = _player.transform.position + Vector3.right * 20 + Vector3.up;
        }
        GameObject enemy = _objectPool.GetObject(prefab);
        enemy.transform.position = _spawnPoint;
        
        // 死亡后归还给对象池
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.onDied.AddListener(() => _objectPool.ReturnObject(enemy));
    }
}

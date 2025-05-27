using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public EnemySpawner enemySpawner;
    public List<WaveConfig> waveConfigs;
    public float timeBetweenWaves = 3f;
    public bool isWaveActive = false;
    public WaveEventSO waveEvent;
    
    private int _currentWaveIndex = 0;
    private int _remainingEnemies = 0;
    private float _currentWaveRemainingTime;

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

    public void StartWave()
    {
        StartCoroutine(StartWaveRoutine());
    }

    private IEnumerator StartWaveRoutine()
    {
        while (_currentWaveIndex < waveConfigs.Count)
        {
            // 第一波直接开始
            if (_currentWaveRemainingTime > 0)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            
            WaveConfig currentWave =  waveConfigs[_currentWaveIndex];
            isWaveActive = true;
            
            _remainingEnemies = currentWave.enemyCount;
            _currentWaveRemainingTime = currentWave.waveDuration;
            waveEvent.RaiseEvent(_currentWaveIndex + 1, _remainingEnemies);

            //生成敌人
            StartCoroutine(SpawnEnemyRoutine(currentWave));

            //等待敌人全部被消灭
            while(_currentWaveRemainingTime > 0 && _remainingEnemies > 0)
            {
                _currentWaveRemainingTime -= Time.deltaTime;
                yield return null;
            }
            
            isWaveActive = false;
            _currentWaveIndex++;

            if (_currentWaveIndex >= waveConfigs.Count)
            {
                Debug.Log("All enemies died, you win!");
                yield break;
            }
        }
    }

    private IEnumerator SpawnEnemyRoutine(WaveConfig currentWave)
    {
        for (int i = 0; i < currentWave.enemyCount; ++i)
        {
            string enemyType = currentWave.enemyTypes[Random.Range(0, currentWave.enemyTypes.Count)];
            enemySpawner.SpawnEnemy(enemyType);
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }
    }

    public void OnEnemyDefeated()
    {
        _remainingEnemies--;
        waveEvent.RaiseEvent(_currentWaveIndex + 1, _remainingEnemies);
    }

    public WaveConfig GetCurrentWave()
    {
        return waveConfigs[_currentWaveIndex];
    }

    public float GetWaveRemainingTime()
    {
        return _currentWaveRemainingTime;
    }
}

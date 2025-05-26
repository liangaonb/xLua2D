using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveInfoUI : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI timerText;
    
    [Header("监听时间")]
    public WaveEventSO waveEvent;

    private void OnEnable()
    {
        waveEvent.onWaveInfoChanged += UpdateWaveInfo;
    }

    private void Update()
    {
        float remainingTime = WaveManager.Instance.GetWaveRemainingTime();
        timerText.text = $"Time: {Mathf.Ceil(remainingTime)}";
    }

    private void OnDisable()
    {
        waveEvent.onWaveInfoChanged -= UpdateWaveInfo;
    }

    private void UpdateWaveInfo(int currentWave, int remainingEnemies)
    {
        waveText.text = $"Wave: {currentWave} / {WaveManager.Instance.waveConfigs.Count}";
        enemyCountText.text = $"Enemies: {remainingEnemies}";
    }
}

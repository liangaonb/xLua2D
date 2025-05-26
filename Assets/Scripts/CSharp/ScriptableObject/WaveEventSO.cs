using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/WaveEventSO")]
public class WaveEventSO : ScriptableObject
{
    public UnityAction<int, int> onWaveInfoChanged; // param1:当前波次  param2:波次剩余敌人

    public void RaiseEvent(int currentWave, int remainingEmemies)
    {
        onWaveInfoChanged?.Invoke(currentWave, remainingEmemies);
    }
}

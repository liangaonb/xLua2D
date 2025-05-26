using System.Collections.Generic;

[System.Serializable]
public class WaveConfig
{
    public int waveNumber; // 波次序号
    public int enemyCount; // 该波敌人数量
    public float spawnInterval; // 生成间隔
    public float waveDuration; // 持续时间
    public List<string> enemyTypes; // 可生成的敌人类型

    public WaveConfig(int waveNumber, int enemyCount, float spawnInterval, float waveDuration, List<string> enemyTypes)
    {
        this.waveNumber = waveNumber;
        this.enemyCount = enemyCount;
        this.spawnInterval = spawnInterval;
        this.waveDuration = waveDuration;
        this.enemyTypes = enemyTypes;
    }
}
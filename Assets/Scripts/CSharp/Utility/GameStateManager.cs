using UnityEngine;
using System;
using UnityEngine.Serialization;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState currentGameState;
    
    // 状态改变事件
    public event Action<GameState> OnGameStateChanged;

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
        
        // 初始状态为标题界面
        ChangeState(GameState.Title);
    }

    public void ChangeState(GameState newState)
    {
        currentGameState = newState;
        
        switch (newState)
        {
            case GameState.Title:
                HandleTitleState();
                break;
            case GameState.Playing:
                HandlePlayingState();
                break;
            case GameState.Paused:
                HandlePausedState();
                break;
            case GameState.GameOver:
                HandleGameOverState();
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleTitleState()
    {
        Time.timeScale = 1;
        // 停止波次生成
        if(WaveManager.Instance != null)
        {
            WaveManager.Instance.StopAllCoroutines();
        }
    }

    private void HandlePlayingState()
    {
        Time.timeScale = 1;
        // 开始波次生成
        if(WaveManager.Instance != null)
        {
            WaveManager.Instance.StartWave();
        }
    }

    private void HandlePausedState()
    {
        Time.timeScale = 0;
    }

    private void HandleGameOverState()
    {
        Time.timeScale = 0;
        // 停止波次生成
        if(WaveManager.Instance != null)
        {
            WaveManager.Instance.StopAllCoroutines();
        }
    }
}
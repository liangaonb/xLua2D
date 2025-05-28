using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public Button restartButton;
    public Button quitButton;
    public GameObject gameplayUI;

    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
        
        // 初始时隐藏GameOver界面
        gameObject.SetActive(false);
        
        // 订阅玩家死亡事件
        PlayerManager.Instance.player.onDied.AddListener(OnPlayerDead);
    }

    private void OnPlayerDead()
    {
        // 显示GameOver
        gameObject.SetActive(true);
        gameplayUI.SetActive(false);
        
        GameStateManager.Instance.ChangeState(GameState.GameOver);
    }

    private void RestartGame()
    {
        // 重新加载当前场景
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
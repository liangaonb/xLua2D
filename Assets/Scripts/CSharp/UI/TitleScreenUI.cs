using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    // 标题界面隐藏gameplay相关UI
    public GameObject gameplayUI;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        
        gameplayUI.SetActive(false);
    }

    private void StartGame()
    {
        gameObject.SetActive(false); // 隐藏标题UI
        GameStateManager.Instance.ChangeState(GameState.Playing);
        gameplayUI.SetActive(true); // 显示gameplay UI
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            MainGameManager.Instance.TogglePauseGame();
        });
        optionsButton.onClick.AddListener(() =>
        {
            AudioAndOptionsManager.Instance.LoadScene("OptionsScene");
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            AudioAndOptionsManager.Instance.LoadScene("MainMenuScene");
        });

        Time.timeScale = 1f;
    }

    private void Start()
    {
        MainGameManager.Instance.OnGamePaused += MainGameManager_OnGamePaused;
        MainGameManager.Instance.OnGameUnpaused += MainGameManager_OnGameUnpaused;
        Hide();
    }

    private void OnDestroy()
    {
        MainGameManager.Instance.OnGamePaused -= MainGameManager_OnGamePaused;
        MainGameManager.Instance.OnGameUnpaused -= MainGameManager_OnGameUnpaused;
    }

    private void MainGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Hide();
    }

    private void MainGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Cursor.lockState = CursorLockMode.None;
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

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
        MainGameManager.Instance.OnPauseAction += MainGameManager_OnPauseAction;
        Hide();
    }

    private void OnDestroy()
    {
        MainGameManager.Instance.OnPauseAction -= MainGameManager_OnPauseAction;
    }

    private void MainGameManager_OnPauseAction(object sender, System.EventArgs e)
    {
        if (MainGameManager.Instance.isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Show();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
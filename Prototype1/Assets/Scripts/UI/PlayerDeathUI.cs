using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDeathUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            AudioAndOptionsManager.Instance.LoadScene("MainMenuScene");
        });
    }

    private void Start()
    {
        MainGameManager.Instance.OnPlayerDeath += MainGameManager_OnPlayerDeath;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MainGameManager.Instance.OnPlayerDeath -= MainGameManager_OnPlayerDeath;
    }

    private void MainGameManager_OnPlayerDeath(object sender, System.EventArgs e)
    {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
    }
}

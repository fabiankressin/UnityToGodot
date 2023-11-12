using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    private bool isInventoryOpen = false;
    [SerializeField] private MainGameManager mainGameManager;

    void Start()
    {
        Hide();
        FirstPersonController.instance.OnInventoryAction += FirstPersonController_OnInventoryAction;
        mainGameManager.OnGamePaused += MainGameManager_OnGamePaused;
    }

    void OnDestroy()
    {
        FirstPersonController.instance.OnInventoryAction -= FirstPersonController_OnInventoryAction;
        mainGameManager.OnGamePaused -= MainGameManager_OnGamePaused;
    }

    private void MainGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void FirstPersonController_OnInventoryAction(object sender, System.EventArgs e)
    {
        isInventoryOpen = !isInventoryOpen;
        if (isInventoryOpen)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Show()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}

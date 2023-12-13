using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    public static PlayerInventoryUI Instance { get; private set; }

    private bool isInventoryOpen = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        FirstPersonController.instance.OnInventoryAction += FirstPersonController_OnInventoryAction;
        MainGameManager.Instance.OnGamePaused += MainGameManager_OnGamePaused;
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        Instance = null;
        FirstPersonController.instance.OnInventoryAction -= FirstPersonController_OnInventoryAction;
        MainGameManager.Instance.OnGamePaused -= MainGameManager_OnGamePaused;
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
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }

    private void Show()
    {
        if (!MainGameManager.Instance.IsGamePaused())
        {
            Cursor.lockState = CursorLockMode.None;
            gameObject.SetActive(true);
        }
    }


    public void SetImageAndCountOnSlot(int slotIndex, Sprite image, int count)
    {
        // Find the InventorySlotUI GameObject by index
        Transform slotTransform = transform.GetChild(slotIndex);

        // Get the InventorySlotUIScript attached to the InventorySlotUI GameObject
        InventorySlotUIScript slotScript = slotTransform.GetComponent<InventorySlotUIScript>();

        // Set the image and count using the InventorySlotUIScript
        if (slotScript != null)
        {
            slotScript.UpdateSlotImage(image);
            slotScript.SetItemCount(count);
        }
    }

}

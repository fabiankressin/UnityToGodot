using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    private bool isInventoryOpen = false;

    void Start()
    {
        Hide();
        FirstPersonController.instance.OnInventoryAction += FirstPersonController_OnInventoryAction;
        MainGameManager.Instance.OnGamePaused += MainGameManager_OnGamePaused;
    }

    void OnDestroy()
    {
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
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Show()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    public static PlayerInventoryUI Instance { get; private set; }

    private bool isInventoryOpen = false;

    private int selectedSlot = 0;
    private bool isSlotSelected = false;

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

    public bool IsSlotSelected()
    {
        return isSlotSelected;
    }

    public int GetSelectedSLot()
    {
        return selectedSlot;
    }

    public void selectSlot(int index)
    {
        if (IsSlotSelected())
        {
            if (GetSelectedSLot() == index)
            {
                isSlotSelected = false;
                // TODO: visibly unmark selected slot in inventory
            }
            else
            {
                // TODO: add logic to swap slot content
                selectedSlot = index;
                // TODO: visibly mark selected slot in inventory
            }
        }
        else
        {
            selectedSlot = index;
            isSlotSelected = true;
            // TODO: visibly mark selected slot in inventory
        }


    }


    public void SetImageAndCountOnSlot(string id, Sprite image, int count, int? slotIndex = null)
    {
        if (slotIndex != null)
        {
            // Find the InventorySlotUI GameObject by index
            Transform slotTransform = transform.GetChild(slotIndex.Value);

            // Get the InventorySlotUIScript attached to the InventorySlotUI GameObject
            InventorySlotUIScript slotScript = slotTransform.GetComponent<InventorySlotUIScript>();

            if (slotScript != null)
            {
                slotScript.UpdateSlot(id, image, count);
                return;
            }
        }
        else
        {
            // Iterate over all children and find the first slot with the same id
            foreach (Transform child in transform)
            {
                // Get the InventorySlotUIScript attached to the InventorySlotUI GameObject
                InventorySlotUIScript slotScript = child.GetComponent<InventorySlotUIScript>();

                // Check if the slot has the same image or an empty image
                if (slotScript != null && slotScript.GetID() == id)
                {
                    slotScript.UpdateSlot(id, image, count);
                    return;
                }
            }
            // Iterate over all children and find the first slot with an empty id
            foreach (Transform child in transform)
            {
                // Get the InventorySlotUIScript attached to the InventorySlotUI GameObject
                InventorySlotUIScript slotScript = child.GetComponent<InventorySlotUIScript>();

                // Check if the slot has the same image or an empty image
                if (slotScript != null && slotScript.SlotIsEmpty())
                {
                    slotScript.UpdateSlot(id, image, count);
                    return;
                }
            }
        }
    }

}

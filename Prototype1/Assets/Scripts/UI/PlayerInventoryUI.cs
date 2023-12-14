using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    public static PlayerInventoryUI Instance { get; private set; }

    [SerializeField] InventorySlotUIScript[] storageSlots;
    [SerializeField] InventorySlotUIScript mainHandSlot;
    [SerializeField] InventorySlotUIScript[] craftingSlots;

    public enum SlotType
    {
        Storage,
        MainHand,
        Crafting
    }

    private bool isInventoryOpen = false;
    private int selectedSlot = 0;
    private SlotType selectedSlotType;
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

        foreach (InventorySlotUIScript slot in storageSlots)
        {
            slot.SetSlotType(SlotType.Storage);
        }

        //mainHandSlot.SetSlotType(SlotType.MainHand);

        foreach (InventorySlotUIScript slot in craftingSlots)
        {
            //slot.SetSlotType(SlotType.Crafting);
        }
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

    //InventoryStorage

    public bool IsSlotSelected()
    {
        return isSlotSelected;
    }

    public bool SelectedSlotMatches(SlotType slotType, int index)
    {
        return selectedSlot == index && selectedSlotType == slotType;
    }

    private void MarkChild(SlotType slotType, int index = 0)
    {
        InventorySlotUIScript slotScript = getSlotUIScript(slotType, index);
        slotScript.Mark();
    }

    private void UnMarkChild(SlotType slotType, int index = 0)
    {
        InventorySlotUIScript slotScript = getSlotUIScript(slotType, index);
        slotScript.UnMark();
    }

    public void SelectSlot(SlotType slotType, int index = 0)
    {
        if (IsSlotSelected())
        {
            if (SelectedSlotMatches(slotType, index))
            {

                UnMarkChild(slotType, index);

                isSlotSelected = false;
            }
            else
            {
                InventorySlotUIScript slotScript1 = getSlotUIScript(selectedSlotType, selectedSlot);
                InventorySlotUIScript slotScript2 = getSlotUIScript(slotType, index);

                string id1 = slotScript1.GetID();
                Sprite image1 = slotScript1.GetSlotImage();
                int count1 = slotScript1.GetCount();

                string id2 = slotScript2.GetID();
                Sprite image2 = slotScript2.GetSlotImage();
                int count2 = slotScript2.GetCount();

                slotScript1.UpdateSlot(id2, image2, count2 - count1);
                slotScript2.UpdateSlot(id1, image1, count1 - count2);

                UnMarkChild(selectedSlotType, selectedSlot);

                isSlotSelected = false;
            }
        }
        else
        {
            MarkChild(slotType, index);
            selectedSlot = index;
            selectedSlotType = slotType;
            isSlotSelected = true;
        }


    }

    public InventorySlotUIScript getSlotUIScript(SlotType slotType, int index = 0)
    {
        switch (slotType)
        {
            case SlotType.Storage:
                {
                    return storageSlots[index];
                }
            case SlotType.MainHand:
                {
                    return mainHandSlot;
                }
            case SlotType.Crafting:
                {
                    return craftingSlots[index];
                }
        }
        Debug.Log("getSlotUIScript: slotType not found. THIS SHOULD NEVER HAPPEN!");
        return null;
    }


    public void SetImageAndCountOnSlot(string id, Sprite image, int count, int? slotIndex = null, SlotType slotType = SlotType.Storage)
    {
        if (slotType != SlotType.Storage)
        {
            Debug.Log("SetImageAndCountOnSlot: slotType != SlotType.Storage");
            return;
        }

        if (slotIndex != null)
        {
            InventorySlotUIScript slotScript = getSlotUIScript(slotType, slotIndex.Value);

            if (slotScript != null)
            {
                slotScript.UpdateSlot(id, image, count);
                return;
            }
        }
        else
        {
            // Iterate over all children and find the first slot with the same id
            foreach (InventorySlotUIScript slotScript in storageSlots)
            {
                // Check if the slot has the same image or an empty image
                if (slotScript != null && slotScript.GetID() == id)
                {
                    slotScript.UpdateSlot(id, image, count);
                    return;
                }
            }
            // Iterate over all children and find the first slot with an empty id
            foreach (InventorySlotUIScript slotScript in storageSlots)
            {
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
